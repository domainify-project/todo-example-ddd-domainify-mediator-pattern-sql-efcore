using Presentation.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using Presentation.WebAPI;
using Domainify;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureLanguage(builder.Configuration);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerService(builder.Configuration);


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder =>
//        {
//            builder.WithOrigins("http://example.com")
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Add this line in Configure method in Startup.cs, before app.UseMvc()


builder.Logging.AddFilter("Application", LogLevel.Information);
builder.Logging.AddFilter("Domain", LogLevel.Information);
builder.Logging.AddFilter("Persistence", LogLevel.Information);
builder.Logging.AddFilter("Presentation", LogLevel.Information);

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseSwaggerService();

app.UseHttpsRedirection();
app.UseExceptionHandler(c => c.Run(async context =>
{
    var devError = ErrorHelper.GetDevError(context.Features.
            Get<IExceptionHandlerPathFeature>()!.Error);
 
    devError.RequestId = Activity.Current?.Id ?? context.TraceIdentifier;

    if (app.Environment.IsProduction())
        await context.Response.WriteAsJsonAsync((Error)devError);

    await context.Response.WriteAsJsonAsync(devError);
}));
app.MapControllers();

//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
