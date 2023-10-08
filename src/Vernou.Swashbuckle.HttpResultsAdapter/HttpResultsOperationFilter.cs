namespace Vernou.Swashbuckle.HttpResultsAdapter;

using global::Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Models;
using System.Reflection;

/// <summary>
/// OperationFilter to generate OAS response to action that return HttpResults type
/// </summary>
public class HttpResultsOperationFilter : IOperationFilter
{
    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {
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
            operation.Responses.Clear();
            foreach(var responseType in responseTypes)
            {
                var statusCode = responseType.StatusCode;
                var oar = new OpenApiResponse { Description = ReasonPhrases.GetReasonPhrase(statusCode) };

                if(responseType.Type != null && responseType.Type != typeof(void))
                {
                    var schema = context.SchemaGenerator.GenerateSchema(responseType.Type, context.SchemaRepository);
                    oar.Content.Add("application/json", new OpenApiMediaType { Schema = schema });
                }

                operation.Responses.Add(statusCode.ToString(), oar);
            }
        }
        else if(actionReturnType == typeof(UnauthorizedHttpResult))
        {
            operation.Responses.Clear();
            operation.Responses.Add("401", new OpenApiResponse { Description = ReasonPhrases.GetReasonPhrase(401) });

        }
    }

    private static bool IsHttpResults(Type type)
        => type.Namespace == "Microsoft.AspNetCore.Http.HttpResults";

    private static Type UnwrapTask(Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>)
                  ? type.GetGenericArguments()[0]
                  : type;

    private sealed class MetadataEndpointBuilder : EndpointBuilder
    {
        public override Endpoint Build() => throw new NotImplementedException();
    }
}
