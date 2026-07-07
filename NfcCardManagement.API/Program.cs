using NfcCardManagement.API.Configuration;
using NfcCardManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ───────────────────────────────────────────────────────────────

// Base de données (EF Core + SQL Server)
builder.Services.AddDatabase(builder.Configuration);

// Repositories — couche d'accès aux données
builder.Services.AddRepositories();

// Services métier
builder.Services.AddServices();

// AutoMapper — profils de mapping DTO
builder.Services.AddAutoMapperProfiles();

// FluentValidation — validation automatique des modèles
builder.Services.AddFluentValidationServices();

// CORS — autoriser l'application Ionic/Angular
builder.Services.AddCorsPolicy();

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI (ajouté systématiquement, activé uniquement en développement)
builder.Services.AddSwaggerDocumentation();

// ─── Application ─────────────────────────────────────────────────────────────

var app = builder.Build();

// Middleware Swagger — Development + Production (accès serveur via Postman)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NFC Card Management API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// Middleware global de gestion des exceptions
app.UseMiddleware<ExceptionMiddleware>();

// CORS — doit être avant UseAuthorization et MapControllers
app.UseCors("AllowIonicApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

// Rendre la classe Program accessible pour les tests d'intégration (WebApplicationFactory)
public partial class Program { }
