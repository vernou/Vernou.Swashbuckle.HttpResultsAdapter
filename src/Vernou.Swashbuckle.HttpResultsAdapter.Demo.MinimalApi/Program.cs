using Microsoft.AspNetCore.Http.HttpResults;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.MinimalApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.EnableAnnotations());

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();

        // As HttpResultsController
        app.MapGetReturn<Ok>("ok");
        app.MapGetReturn<Ok>("ok-producestype", (int)HttpStatusCode.NoContent);
        app.MapGetReturn<Ok>("ok-annotation", metadata: new[] { new SwaggerResponseAttribute(200, "Custom Annotation") });
        app.MapGetReturn<Created>("created");
        app.MapGetReturn<Accepted>("accepted");
        app.MapGetReturn<AcceptedAtRoute>("acceptedatroute");
        app.MapGetReturn<NoContent>("nocontent");
        app.MapGetReturn<BadRequest>("badrequest");
        app.MapGetReturn<NotFound>("notfound");
        app.MapGetReturn<Conflict>("conflict");
        app.MapGetReturn<UnauthorizedHttpResult>("unauthorized", 401);
        app.MapGetReturn<UnprocessableEntity>("unprocessableentity");
        //app.MapGetReturn<ValidationProblem>("validationproblem");

        // As HttpResultsOfController
        app.MapGetReturn<Ok<Foo>>("ok-foo");
        app.MapGetReturn<Created<Foo>>("created-foo");
        app.MapGetReturn<CreatedAtRoute<Foo>>("createdatroute-foo");
        app.MapGetReturn<Accepted<Foo>>("accepted-foo");
        app.MapGetReturn<AcceptedAtRoute<Foo>>("acceptedatroute-foo");
        app.MapGetReturn<BadRequest<Foo>>("badrequest-foo");
        app.MapGetReturn<Conflict<Foo>>("conflict-foo");
        app.MapGetReturn<NotFound<Foo>>("notfound-foo");
        app.MapGetReturn<UnprocessableEntity<Foo>>("unprocessableentity-foo");

        // As ManyHttpResultsController
        app.MapGetReturn<Results<Ok, NoContent>>("ok_nocontent");
        app.MapGetReturn<Results<Ok, NoContent, NotFound>>("ok_nocontent_notfound");

        app.Run();
    }
}

internal static class IEndpointRouteBuilderExtentions
{
    public static void MapGetReturn<TResult>(this IEndpointRouteBuilder endpoints, string path, int? statusCode = null, object[]? metadata = null)
    {
        endpoints.MapGet(path, Return<TResult>).WithMetadata(metadata ?? Array.Empty<object>()).ProducesIf(statusCode);
        endpoints.MapGet(path + "-async", Return<Task<TResult>>).WithMetadata(metadata ?? Array.Empty<object>()).ProducesIf(statusCode);
        endpoints.MapGet(path + "-valuetask", Return<ValueTask<TResult>>).WithMetadata(metadata ?? Array.Empty<object>()).ProducesIf(statusCode);
    }

    private static void ProducesIf(this RouteHandlerBuilder builder, int? statusCode)
    {
        if(statusCode.HasValue)
        {
            builder.Produces(statusCode.Value);
        }
    }

    private static TResult Return<TResult>()
        => throw new NotImplementedException();
}

public sealed class Foo
{
    public int Id { get; set; }
    public string Label { get; set; } = "";
}