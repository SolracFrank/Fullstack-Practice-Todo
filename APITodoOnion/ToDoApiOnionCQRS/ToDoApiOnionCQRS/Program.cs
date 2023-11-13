using Application;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Infrastructure;
using Infrastructure.CustomEntities;
using Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Filters;
using WebApi.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

#region Serilog

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console(
        LogEventLevel.Debug, builder.Configuration["Logging:OutputTemplate"], theme: SystemConsoleTheme.Colored)
    .WriteTo.File(Path.Combine(AppContext.BaseDirectory, builder.Configuration["Logging:Dir"]),
        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null)

    .CreateLogger();

builder.Host.UseSerilog();



#endregion

#region Routing
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
#endregion

#region Controllers
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("DefaultCache", new CacheProfile() { Duration = 20 });
    options.Filters.Add<ValidateModelStateFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
       StatusCodes.Status500InternalServerError));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
#endregion

#region Infrastructure

builder.Services.AddInfrastructure(builder.Configuration);

#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TodoAuth", policy =>
    {
        policy.RequireClaim("Active", "True");
        policy.AuthenticationSchemes.Add("TodoJWT");
    });
});
#endregion

#region Application

builder.Services.AddApplication();

#endregion

//TODO: USERCONTEXT

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
#region Swagger

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Description =
            "Bearer Scheme: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };

    options.AddSecurityRequirement(securityRequirement);

    options.OperationFilter<SwaggerDefaultValues>();
    options.EnableAnnotations();
});

#endregion

#region Versioning

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region ProblemDetails

builder.Services.AddProblemDetails();

#endregion

#region Redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
});
#endregion

#region Compression

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

#endregion

#region CORS BUILDER
if (isDevelopment)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}
#endregion

var app = builder.Build();

var apiVersionDescriptionDescriber = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsStaging() || app.Environment.IsDevelopment())
{
    #region Swagger

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionDescriber.ApiVersionDescriptions)
            options.SwaggerEndpoint($"swagger/{description.GroupName}/swagger.json",
                $"{builder.Configuration["Swagger:Name"]} V{description.ApiVersion}");

        options.DocumentTitle = builder.Configuration["Swagger:DocumentTitle"];
        options.RoutePrefix = builder.Configuration["Swagger:RoutePrefix"];
    });

    #endregion

    app.UseCors("AllowAll");
}

#region IdentitySeeder
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultUserBasic.SeedAsync(userManager, roleManager);
        await DefaultUserAdmin.SeedAsync(userManager, roleManager);
    }
    catch (Exception)
    {
        throw;
    }
};
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
