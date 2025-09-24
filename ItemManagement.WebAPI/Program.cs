using AutoMapper;
using FluentValidation;
using ItemManagement.Application.UseCases;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.Common.Validation;
using ItemManagement.Application.UseCases;
using ItemManagement.Application.Mapping;
using ItemManagement.Application.UserCases;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCasesInterfaces;
using ItemManagement.Domain.Repositories;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository; 
using ItemManagement.WebAPI.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Serilog;
using ItemManagement.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog

// Configure Serilog first before building the app
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // Read config from appsettings.json Serilog section
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/webapi-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog as logging provider

//add for db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);


//add service
builder.Services.AddScoped<IItemsUseCases, ItemsUseCases>();
builder.Services.AddScoped<ICategoriesUseCases, CategoriesUseCases>();
builder.Services.AddScoped<ITransactionsUseCases, TransactionsUseCases>();
builder.Services.AddScoped<ISellItemUseCase, SellItemUseCase>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//FluentValidation  
builder.Services.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
builder.Services.AddScoped<IValidator<ItemDto>, ItemDtoValidator>(); 
builder.Services.AddScoped<IValidator<SellOrderDto>, SellOrderDtoValidator>();
builder.Services.AddScoped<IValidator<TransactionDto>, TransactionDtoValidator>();

builder.Services.AddAutoMapper(cfg => {
    // optionally add extra mappings here
}, typeof(AutoMapperProfiles).Assembly,
   typeof(ViewModelMappingProfile).Assembly);

// Add services to the container.
builder.Services.AddControllers();

// Register the Swagger generator
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

//// Add Azure AD authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/6efc377b-a2e2-48a0-8371-7a435babb8a9/v2.0";  
        options.Audience = "205aaa0a-186c-406a-8fca-53fdc63801f0";  
         
    });

 
builder.Services.AddAuthorizationBuilder();


var app = builder.Build();
 
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // Serve Swagger UI at root /
    });
//}
// Add global exception handler middleware early
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        logger.LogError(exceptionFeature.Error, "Unhandled exception");

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    });
});
// Add Serilog or other request logging middleware if any
app.UseSerilogRequestLogging();


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
