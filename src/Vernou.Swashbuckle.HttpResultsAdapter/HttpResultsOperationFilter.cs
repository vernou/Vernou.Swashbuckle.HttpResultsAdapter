namespace Vernou.Swashbuckle.HttpResultsAdapter;

using global::Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// OperationFilter to generate OAS response to action that return HttpResults type
/// </summary>
public class HttpResultsOperationFilter : IOperationFilter
{
    private readonly Lazy<string[]> _contentTypes;

    /// <summary>
    /// Constructor to inject services
    /// </summary>
    /// <param name="mvc">MVC options to define response content types</param>
    public HttpResultsOperationFilter(IOptions<MvcOptions> mvc)
    {
        _contentTypes = new Lazy<string[]>(() =>
        {
            var apiResponseTypes = new List<string>();
            if(mvc.Value == null)
            {
                apiResponseTypes.Add("application/json");
            }
            else
            {
                var jsonApplicationType = mvc.Value.FormatterMappings.GetMediaTypeMappingForFormat("json");
                if(jsonApplicationType != null)
                    apiResponseTypes.Add(jsonApplicationType);
                var xmlApplicationType = mvc.Value.FormatterMappings.GetMediaTypeMappingForFormat("xml");
                if(xmlApplicationType != null)
                    apiResponseTypes.Add(xmlApplicationType);
            }
            return apiResponseTypes.ToArray();
        });
    }

    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        if(!IsControllerAction(context)) return;

        var actionReturnType = UnwrapTask(context.MethodInfo.ReturnType);
        if(!IsHttpResults(actionReturnType)) return;

        if(typeof(IEndpointMetadataProvider).IsAssignableFrom(actionReturnType))
        {
            var populateMetadataMethod = actionReturnType.GetMethod("Microsoft.AspNetCore.Http.Metadata.IEndpointMetadataProvider.PopulateMetadata", BindingFlags.Static | BindingFlags.NonPublic);
            if(populateMetadataMethod == null) return;

            var endpointBuilder = new MetadataEndpointBuilder();
            populateMetadataMethod.Invoke(null, new object[] { context.MethodInfo, endpointBuilder });

            var responseTypes = endpointBuilder.Metadata.Cast<IProducesResponseTypeMetadata>().ToList();
            if(!responseTypes.Any()) return;

            CleanSwashbuckleDefaultResponse200(operation, context);

            foreach(var responseType in responseTypes)
            {
                var statusCode = responseType.StatusCode.ToString();

                // If controller is documented with attributes, correctly documented response is already present but is missing schemas
                operation.Responses.TryAdd(statusCode, new OpenApiResponse { Description = GetResponseDescription(statusCode) });
                var oar = operation.Responses[statusCode];

                if(responseType.Type != null && responseType.Type != typeof(void))
                {
                    var schema = context.SchemaGenerator.GenerateSchema(responseType.Type, context.SchemaRepository);
                    foreach(var contentType in _contentTypes.Value)
                    {
                        if(!oar.Content.TryAdd(contentType, new OpenApiMediaType { Schema = schema }))
                        {
                            oar.Content[contentType].Schema = schema;
                        }
                    }
                }
            }
        }
        else if(actionReturnType == typeof(UnauthorizedHttpResult))
        {
            operation.Responses.Clear();
            operation.Responses.Add("401", new OpenApiResponse { Description = ReasonPhrases.GetReasonPhrase(401) });
        }
    }

    /// <summary>
    /// Remove the Swashbuckle default response 200 added
    /// when the operation has none response provided by ASP.NET Core,
    /// </summary>
    /// <remarks>
    /// See :
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/v6.9.0/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L890
    /// </remarks>
    private static void CleanSwashbuckleDefaultResponse200(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.SupportedResponseTypes.Count == 0 &&  operation.Responses.TryGetValue("200", out var ok))
        {
            operation.Responses.Remove("200");
        }
    }

    private static bool IsControllerAction(OperationFilterContext context)
        => context.ApiDescription.ActionDescriptor is ControllerActionDescriptor;

    private static bool IsHttpResults(Type type)
        => type.Namespace == "Microsoft.AspNetCore.Http.HttpResults";

    private static Type UnwrapTask(Type type)
    {
        if(type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            if(genericType == typeof(Task<>) || genericType == typeof(ValueTask<>))
            {
                return type.GetGenericArguments()[0];
            }
        }
        return type;
    }

    private static string? GetResponseDescription(string statusCode)
        => ResponseDescriptionMap
            .FirstOrDefault(entry => Regex.IsMatch(statusCode, entry.Key))
            .Value;

    private static readonly IReadOnlyCollection<KeyValuePair<string, string>> ResponseDescriptionMap = new[]
    {
        new KeyValuePair<string, string>("100", "Continue"),
        new KeyValuePair<string, string>("101", "Switching Protocols"),
        new KeyValuePair<string, string>("1\\d{2}", "Information"),

        new KeyValuePair<string, string>("200", "OK"),
        new KeyValuePair<string, string>("201", "Created"),
        new KeyValuePair<string, string>("202", "Accepted"),
        new KeyValuePair<string, string>("203", "Non-Authoritative Information"),
        new KeyValuePair<string, string>("204", "No Content"),
        new KeyValuePair<string, string>("205", "Reset Content"),
        new KeyValuePair<string, string>("206", "Partial Content"),
        new KeyValuePair<string, string>("2\\d{2}", "Success"),

        new KeyValuePair<string, string>("300", "Multiple Choices"),
        new KeyValuePair<string, string>("301", "Moved Permanently"),
        new KeyValuePair<string, string>("302", "Found"),
        new KeyValuePair<string, string>("303", "See Other"),
        new KeyValuePair<string, string>("304", "Not Modified"),
        new KeyValuePair<string, string>("305", "Use Proxy"),
        new KeyValuePair<string, string>("307", "Temporary Redirect"),
        new KeyValuePair<string, string>("308", "Permanent Redirect"),
        new KeyValuePair<string, string>("3\\d{2}", "Redirect"),

        new KeyValuePair<string, string>("400", "Bad Request"),
        new KeyValuePair<string, string>("401", "Unauthorized"),
        new KeyValuePair<string, string>("402", "Payment Required"),
        new KeyValuePair<string, string>("403", "Forbidden"),
        new KeyValuePair<string, string>("404", "Not Found"),
        new KeyValuePair<string, string>("405", "Method Not Allowed"),
        new KeyValuePair<string, string>("406", "Not Acceptable"),
        new KeyValuePair<string, string>("407", "Proxy Authentication Required"),
        new KeyValuePair<string, string>("408", "Request Timeout"),
        new KeyValuePair<string, string>("409", "Conflict"),
        new KeyValuePair<string, string>("410", "Gone"),
        new KeyValuePair<string, string>("411", "Length Required"),
        new KeyValuePair<string, string>("412", "Precondition Failed"),
        new KeyValuePair<string, string>("413", "Content Too Large"),
        new KeyValuePair<string, string>("414", "URI Too Long"),
        new KeyValuePair<string, string>("415", "Unsupported Media Type"),
        new KeyValuePair<string, string>("416", "Range Not Satisfiable"),
        new KeyValuePair<string, string>("417", "Expectation Failed"),
        new KeyValuePair<string, string>("421", "Misdirected Request"),
        new KeyValuePair<string, string>("422", "Unprocessable Content"),
        new KeyValuePair<string, string>("423", "Locked"),
        new KeyValuePair<string, string>("424", "Failed Dependency"),
        new KeyValuePair<string, string>("426", "Upgrade Required"),
        new KeyValuePair<string, string>("428", "Precondition Required"),
        new KeyValuePair<string, string>("429", "Too Many Requests"),
        new KeyValuePair<string, string>("431", "Request Header Fields Too Large"),
        new KeyValuePair<string, string>("451", "Unavailable For Legal Reasons"),
        new KeyValuePair<string, string>("4\\d{2}", "Client Error"),

        new KeyValuePair<string, string>("500", "Internal Server Error"),
        new KeyValuePair<string, string>("501", "Not Implemented"),
        new KeyValuePair<string, string>("502", "Bad Gateway"),
        new KeyValuePair<string, string>("503", "Service Unavailable"),
        new KeyValuePair<string, string>("504", "Gateway Timeout"),
        new KeyValuePair<string, string>("505", "HTTP Version Not Supported"),
        new KeyValuePair<string, string>("5\\d{2}", "Server Error"),

        new KeyValuePair<string, string>("default", "Error")
    };

    private sealed class MetadataEndpointBuilder : EndpointBuilder
    {
        public override Endpoint Build() => throw new NotImplementedException();
    }
}
