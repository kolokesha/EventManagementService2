using EventManagementService.Application.DI;
using EventManagementService.Application.Events;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;
using EventManagementService.Infrastructure.DI;
using EventManagementService.Infrastructure.Repository;
using EventManagementService.Infrastructure.Services;
using EventManagementService.Middlewares;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

if (builder.Environment.IsDevelopment())
{
    builder.Host.UseDefaultServiceProvider(options =>
    {
        options.ValidateScopes = true;
        options.ValidateOnBuild = true;
    });
} 

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new
                {
                    Field = x.Key,
                    Error = e.ErrorMessage
                }))
                .ToList();

            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogError("Validation failed: {@Errors}", errors);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = "One or more validation errors occurred.",
                Extensions =
                {
                    ["errors"] = errors
                }
            };

            return new BadRequestObjectResult(problemDetails);
        };
    }); 
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPath
                            | HttpLoggingFields.RequestMethod
                            | HttpLoggingFields.ResponseStatusCode
                            | HttpLoggingFields.Duration;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.UseRequestLogging();

app.Run();