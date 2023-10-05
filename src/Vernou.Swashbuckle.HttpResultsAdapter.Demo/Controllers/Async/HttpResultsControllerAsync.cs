namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers.Async;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

public class HttpResultsAsyncController : Controller
{
    [HttpGet("ok-async")]
    public async Task<Ok> ReturnOkAsync() => await Task.FromResult(TypedResults.Ok());

    [HttpGet("created-async")]
    public async Task<Created> ReturnCreatedAsync() => await Task.FromResult(TypedResults.Created(""));

    [HttpGet("accepted-async")]
    public async Task<Accepted> ReturnAcceptedAsync() => await Task.FromResult(TypedResults.Accepted(""));

    [HttpGet("acceptedatroute-async")]
    public async Task<AcceptedAtRoute> ReturnAcceptedAtRouteAsync() => await Task.FromResult(TypedResults.AcceptedAtRoute());

    [HttpGet("nocontent-async")]
    public async Task<NoContent> ReturnNoContentAsync() => await Task.FromResult(TypedResults.NoContent());

    [HttpGet("badrequest-async")]
    public async Task<BadRequest> ReturnBadRequestAsync() => await Task.FromResult(TypedResults.BadRequest());

    [HttpGet("notfound-async")]
    public async Task<NotFound> ReturnNotFoundAsync() => await Task.FromResult(TypedResults.NotFound());

    [HttpGet("conflict-async")]
    public async Task<Conflict> ReturnChallengeAsync() => await Task.FromResult(TypedResults.Conflict());

    [HttpGet("unauthorized-async")]
    public async Task<UnauthorizedHttpResult> ReturnUnauthorizedAsync() => await Task.FromResult(TypedResults.Unauthorized());

    [HttpGet("unprocessableentity-async")]
    public async Task<UnprocessableEntity> ReturnUnprocessableEntityAsync() => await Task.FromResult(TypedResults.UnprocessableEntity());

    [HttpGet("validationproblem-async")]
    public async Task<ValidationProblem> ReturnValidationProblemAsync() => await Task.FromResult(TypedResults.ValidationProblem(new Dictionary<string, string[]>()));
}
