namespace Vernou.Swashbuckle.HttpResultsAdapter;

using global::Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Models;

using System.Diagnostics;
using System.Reflection;

/// <summary>
/// OperationFilter to generate OAS response to action that return HttpResults type
/// </summary>
public class HttpResultsOperationFilter : IOperationFilter
{
    /// <summary>
    /// this is the default for controllers
    /// </summary>
    private static readonly MediaTypeCollection defaultMediaTypes = new MediaTypeCollection
    {
        "application/json",
        "application/xml"
    };

    /// <summary>
    /// media type for text/plain
    /// </summary>
    private static readonly MediaTypeCollection textPlainMediaTypes = new MediaTypeCollection 
    { 
        "text/plain" 
    };

    /// <summary>
    /// empty MediaTypeCollection
    /// </summary>
    private static readonly MediaTypeCollection emptyMediaTypes = new MediaTypeCollection();

    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        try
        {
            // check if the HttpResults is nested in an Task, if so use the first generic argument
            var actionReturnType = context.MethodInfo.ReturnType.IsGenericType && context.MethodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                  ? context.MethodInfo.ReturnType.GetGenericArguments()[0]
                  : context.MethodInfo.ReturnType; 
            
            if (!IsHttpResults(actionReturnType)) return;

            if (!typeof(IEndpointMetadataProvider).IsAssignableFrom(actionReturnType))
            {
                // if not IEndpointMetadataProvider then check if it is UnauthorizedHttpResult
                if (actionReturnType == typeof(UnauthorizedHttpResult))
                {
                    operation.Responses.Clear();
                    operation.Responses.Add("401", new OpenApiResponse { Description = ReasonPhrases.GetReasonPhrase(401) });
                }
            }
            else
            {
                // get the non public, static method that MS does not want us to use
                var populateMetadataMethod = actionReturnType.GetMethod("Microsoft.AspNetCore.Http.Metadata.IEndpointMetadataProvider.PopulateMetadata", BindingFlags.Static | BindingFlags.NonPublic);
                if (populateMetadataMethod == null) return;

                var endpointBuilder = new MetadataEndpointBuilder();
                populateMetadataMethod.Invoke(null, new object[] { context.MethodInfo, endpointBuilder });

                // get the response types
                var responseTypes = endpointBuilder.Metadata.Cast<IProducesResponseTypeMetadata>().ToList();
                if (!responseTypes.Any()) return;
                operation.Responses.Clear();

                // get the Produces attribute of the method and if null try to get if from the implementing class
                // so we can get the ContentTyes
                // TODO: there has to be a more generic way ...
                var producesAttribute = context.MethodInfo.GetCustomAttributes(typeof(ProducesAttribute), true).FirstOrDefault() as ProducesAttribute;
                producesAttribute ??= context.MethodInfo.DeclaringType?.GetCustomAttributes(typeof(ProducesAttribute), true).FirstOrDefault() as ProducesAttribute;
                // get list of media types produced by the method or the default if null
                var mediaTypes = producesAttribute?.ContentTypes ?? defaultMediaTypes;

                foreach (var responseType in responseTypes)
                {
                    var statusCode = responseType.StatusCode;
                    var oar = new OpenApiResponse { Description = ReasonPhrases.GetReasonPhrase(statusCode) };

                    // if the type is set and not void...
                    if (responseType.Type != null && responseType.Type != typeof(void))
                    {
                        // generate a schema documentation for the response type
                        var schema = context.SchemaGenerator.GenerateSchema(responseType.Type, context.SchemaRepository);
                        // for each media type generate a OpenApiMediaType
                        foreach (var mediaType in mediaTypes)
                        {
                            oar.Content.Add(mediaType, new OpenApiMediaType { Schema = schema });
                        }
                    }

                    operation.Responses.Add(statusCode.ToString(), oar);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    /// <summary>
    /// everything from the namespace Microsoft.AspNetCore.Http.HttpResults
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsHttpResults(Type type)
        => type.Namespace == "Microsoft.AspNetCore.Http.HttpResults";

    /// <summary>
    /// just a dummy ?
    /// </summary>
    private sealed class MetadataEndpointBuilder : EndpointBuilder
    {
        public override Endpoint Build() => throw new NotImplementedException();
    }
}
