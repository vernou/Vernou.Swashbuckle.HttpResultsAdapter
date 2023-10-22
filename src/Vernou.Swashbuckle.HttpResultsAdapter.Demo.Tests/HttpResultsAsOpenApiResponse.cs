namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Shouldly;

public class HttpResultsAsOpenApiResponse
{
    private static readonly Lazy<OpenApiDocument> _document =
        new(() =>
        {
            using var factory = new WebApplicationFactory<Program>();
            using var client = factory.CreateClient();
            using var httpResponse = client.GetAsync("/swagger/v1/swagger.json").Result;
            httpResponse.EnsureSuccessStatusCode();
            var document = new OpenApiStreamReader().Read(httpResponse.Content.ReadAsStream(), out var diagnostic);
            diagnostic.Errors.ShouldBeEmpty();
            diagnostic.Warnings.ShouldBeEmpty();
            return document;
        });
    private static OpenApiDocument Document => _document.Value;

    private static OpenApiResponses GetResponses(string path)
    {
        var responses = Document.Paths["/" + path].Operations[OperationType.Get].Responses;
        responses.ShouldNotBeNull();
        return responses;
    }

    private static IEnumerable<object[]> DuplicateWithAsync(IEnumerable<(string, string)> pathsWithExpectedStatus)
    {
        foreach(var path in pathsWithExpectedStatus)
        {
            yield return new object[] { path.Item1, path.Item2 };
            yield return new object[] { path.Item1 + "-async", path.Item2 };
        }
    }

    public static IEnumerable<object[]> TypedResultData()
    {
        return DuplicateWithAsync(new[] {
            ("ok", "200"),
            ("created", "201"),
            ("accepted", "202"),
            ("acceptedatroute", "202"),
            ("nocontent", "204"),
            ("badrequest", "400"),
            ("unauthorized", "401"),
            ("notfound", "404"),
            ("conflict", "409")
        });
    }

    [Theory]
    [MemberData(nameof(TypedResultData))]
    [InlineData("validationproblem", "400", Skip = "Fail")]
    [InlineData("unprocessableentity", "422", Skip = "Fail")]
    public void TypedResult(string path, string expected)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe(expected);
        response.Value.Content.ShouldBeEmpty();
    }

    public static IEnumerable<object[]> TypedResultOfData()
    {
        return DuplicateWithAsync(new[] {
            ("ok-foo", "200"),
            ("created-foo", "201"),
            ("createdatroute-foo", "201"),
            ("accepted-foo", "202"),
            ("acceptedatroute-foo", "202"),
            ("badrequest-foo", "400"),
            ("notfound-foo", "404"),
            ("conflict-foo", "409"),
            ("unprocessableentity-foo", "422"),
        });
    }

    [Theory]
    [MemberData(nameof(TypedResultOfData))]
    public void TypedResultOf(string path, string expected)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe(expected);
        var content = response.Value.Content.ShouldHaveSingleItem();
        content.Key.ShouldBe("application/json");
        content.Value.Schema.Type.ShouldBe("object");
        content.Value.Schema.Reference.Id.ShouldBe("Foo");
    }

    public static IEnumerable<object[]> ManyTypedResultData()
    {
        IEnumerable<object[]> DuplicateWithAsync(IEnumerable<(string, string[])> pathsWithExpectedStatus)
        {
            foreach(var path in pathsWithExpectedStatus)
            {
                yield return new object[] { path.Item1, path.Item2 };
                yield return new object[] { path.Item1 + "-async", path.Item2 };
            }
        }

        return DuplicateWithAsync(new[] {
            ("ok_nocontent", new [] { "200", "204" }),
            ("ok_nocontent_notfound", new [] { "200", "204", "404" }),
        });
    }

    [Theory]
    [MemberData(nameof(ManyTypedResultData))]
    public void ManyTypedResult(string path, string[] expected)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        responses.Count.ShouldBe(expected.Length);
        foreach(var e in expected)
        {
            var response = responses[e];
            response.Content.ShouldBeEmpty();
        }
    }
}
