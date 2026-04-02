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
                .Where(kv => kv.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value!.Errors.Select(e => e.ErrorMessage));
            
            var customResponse = new
            {
                Message = "Ошибки валидации",
                Errors = errors
            };
            
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            var errorsString = string.Join(",", errors.Select(kv => $"{kv.Key}: {kv.Value}"));

            logger.LogError($"Ошибка валидации: {errorsString}");

            return new BadRequestObjectResult(customResponse);
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