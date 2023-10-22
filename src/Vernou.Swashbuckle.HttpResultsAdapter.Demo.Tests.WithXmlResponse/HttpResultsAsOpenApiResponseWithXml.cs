﻿namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

public class HttpResultsAsOpenApiResponseWithXml
{
    private class ApiWithXmlFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
                services.AddControllers().AddXmlSerializerFormatters()
            );
        }
    }

    private static readonly Lazy<OpenApiDocument> _document =
        new(() =>
        {
            var factory = new ApiWithXmlFactory();
            var client = factory.CreateClient();
            var httpResponse = client.GetAsync("/swagger/v1/swagger.json").Result;
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

    public static IEnumerable<object[]> TypedResultOfData()
    {
        return DuplicateWithAsync(new[] {
            ("ok-foo", "200"),
            //("created-foo", "201"),
            //("createdatroute-foo", "201"),
            //("accepted-foo", "202"),
            //("acceptedatroute-foo", "202"),
            //("badrequest-foo", "400"),
            //("notfound-foo", "404"),
            //("conflict-foo", "409"),
            //("unprocessableentity-foo", "422"),
        });
    }

    //[Theory]
    //[MemberData(nameof(TypedResultOfData))]
    //public void TypedResultOf(string path, string expected)
    //{
    //    // Arrange

    //    var responses = GetResponses(path);

    //    //Assert

    //    var response = responses.ShouldHaveSingleItem();
    //    response.Key.ShouldBe(expected);
    //    var content = response.Value.Content.ShouldHaveSingleItem();
    //    content.Key.ShouldBe("application/json");
    //    content.Value.Schema.Type.ShouldBe("object");
    //    content.Value.Schema.Reference.Id.ShouldBe("Foo");
    //}

    [Fact]
    public void SomeTest()
    {
        var responses = GetResponses("ok-foo");
    }
}