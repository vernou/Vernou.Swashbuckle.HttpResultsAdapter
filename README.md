# Vernou.Swashbuckle.HttpResultsAdapter

**Vernou.Swashbuckle.HttpResultsAdapter** is a NuGet package that extend **Swashbuckle.AspNetCore** to generate Open Api responses to action that returns type from the namespace [`Microsoft.AspNetCore.Http.HttpResult`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpresults).

## Why?

.NET 7 introduce [**Typed Http Results**]((https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0#httpresults-type)), but **Swashbuckle** don't generate the Open Api Response from this types.\

See this issue for more detail :\
[TypedResults metadata are not inferred for API Controllers](https://github.com/dotnet/aspnetcore/issues/44988)

## Getting started

Install the package [Vernou.Swashbuckle.HttpResultsAdapter](https://www.nuget.org/packages/Vernou.Swashbuckle.HttpResultsAdapter) :

```sh
dotnet add package Vernou.Swashbuckle.HttpResultsAdapter
```

Add the operation filter like :
```csharp
builder.Services
    .AddSwaggerGen(options =>
    {
        ...
        options.OperationFilter<HttpResultsOperationFilter>();
    });
```

Enjoy convenient **Typed Http Results**.

## Contributing

This project welcomes contributions and suggestions.

## License

**Vernou.Swashbuckle.HttpResultsAdapter** is licensed under the **MIT license**.
