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

    private static IEnumerable<object[]> DuplicateWithAsync(IEnumerable<object[]> pathsWithExpected)
    {
        foreach(var path in pathsWithExpected)
        {
            yield return path;
            var pathAsync = (object[])path.Clone();
            pathAsync[0] = pathAsync[0].ToString() + "-async";
            yield return pathAsync;
        }
    }

    public static IEnumerable<object[]> TypedResultData()
    {
        return DuplicateWithAsync(new[] {
            new object[] { "ok", "200", "Success" },
            new object[] { "created", "201", "Created" },
            new object[] { "accepted", "202", "Accepted" },
            new object[] { "acceptedatroute", "202", "Accepted" },
            new object[] { "nocontent", "204", "No Content" },
            new object[] { "badrequest", "400", "Bad Request" },
            new object[] { "unauthorized", "401", "Unauthorized" },
            new object[] { "notfound", "404", "Not Found" },
            new object[] { "conflict", "409", "Conflict" }
        });
    }

    [Theory]
    [MemberData(nameof(TypedResultData))]
    [InlineData("validationproblem", "400", "Bad Request", Skip = "Fail")]
    [InlineData("unprocessableentity", "422", "Client Error", Skip = "Fail")]
    public void TypedResult(string path, string expectedStatusCode, string expectedDescription)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe(expectedStatusCode);
        response.Value.Description.ShouldBe(expectedDescription);
        response.Value.Content.ShouldBeEmpty();
    }

    public static IEnumerable<object[]> TypedResultOfData()
    {
        return DuplicateWithAsync(new[] {
            new object[] { "ok-foo", "200", "Success" },
            new object[] { "created-foo", "201", "Created" },
            new object[] { "createdatroute-foo", "201", "Created" },
            new object[] { "accepted-foo", "202", "Accepted" },
            new object[] { "acceptedatroute-foo", "202", "Accepted" },
            new object[] { "badrequest-foo", "400", "Bad Request" },
            new object[] { "notfound-foo", "404", "Not Found" },
            new object[] { "conflict-foo", "409", "Conflict" },
            new object[] { "unprocessableentity-foo", "422", "Client Error" }
        });
    }

    [Theory]
    [MemberData(nameof(TypedResultOfData))]
    public void TypedResultOf(string path, string expectedStatusCode, string expectedDescription)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe(expectedStatusCode);
        response.Value.Description.ShouldBe(expectedDescription);
        var content = response.Value.Content.ShouldHaveSingleItem();
        content.Key.ShouldBe("application/json");
        content.Value.Schema.Type.ShouldBe("object");
        content.Value.Schema.Reference.Id.ShouldBe("Foo");
    }

    public static IEnumerable<object[]> ManyTypedResultData()
    {
        return DuplicateWithAsync(new [] {
            new object[] { "ok_nocontent", new [] { ("200", "Success"), ("204", "No Content") } },
            new object[] { "ok_nocontent_notfound", new [] { ("200", "Success"), ("204", "No Content"), ("404", "Not Found") }}
        });
    }

    [Theory]
    [MemberData(nameof(ManyTypedResultData))]
    public void ManyTypedResult(string path, (string StatusCode, string Description)[] expected)
    {
        // Arrange

        var responses = GetResponses(path);

        //Assert

        responses.Count.ShouldBe(expected.Length);
        foreach(var e in expected)
        {
            var response = responses[e.StatusCode];
            response.Description.ShouldBe(e.Description);
            response.Content.ShouldBeEmpty();
        }
    }
}
