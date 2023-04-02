using Microsoft.AspNetCore.Mvc.Formatters;
using Notino.Documents.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDocumentProvider, InMemoryDocumentProvider>();
builder.Services.Decorate<IDocumentProvider, CachedDocumentProvider>();

builder.Services
    .AddControllers(opts =>
    {
        opts.RespectBrowserAcceptHeader = true;
        opts.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        // possibly add other custom or library formatters 
    });

builder.Services.AddCors(opts =>
    opts.AddDefaultPolicy(policy => 
        policy
            .WithOrigins("http://localhost:5000")
            .WithMethods("GET", "POST", "PUT")
    ));

var app = builder.Build();

app.UseCors();
app.MapControllers();
app.Run();