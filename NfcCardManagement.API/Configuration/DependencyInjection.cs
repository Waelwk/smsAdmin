using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NfcCardManagement.API.Data;
using NfcCardManagement.API.Repositories;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Configuration;

/// <summary>
/// Extension de configuration DI centralisée pour tous les services de l'application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Enregistre les services liés à la base de données (EF Core + SQL Server).
    /// </summary>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Priorité 1 : web.config <connectionStrings> (déploiement IIS)
        // Priorité 2 : appsettings.json (développement local)
        var connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["CST_ConnectionString"]?.ConnectionString
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Aucune chaîne de connexion trouvée. " +
                "Vérifiez <connectionStrings> dans web.config ou ConnectionStrings:DefaultConnection dans appsettings.json.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            });
        });

        return services;
    }

    /// <summary>
    /// Enregistre les repositories de la couche d'accès aux données.
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeRepository, EmployeRepository>();
        services.AddScoped<IVehiculeRepository, VehiculeRepository>();

        return services;
    }

    /// <summary>
    /// Enregistre les services de la couche métier.
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICTagService, CTagService>();
        services.AddScoped<IEmployeService, EmployeService>();
        services.AddScoped<IVehiculeService, VehiculeService>();
        services.AddScoped<ICarteService, CarteService>();

        return services;
    }

    /// <summary>
    /// Enregistre AutoMapper avec tous les profils de l'assembly courant.
    /// </summary>
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        return services;
    }

    /// <summary>
    /// Enregistre FluentValidation avec la validation automatique des modèles.
    /// </summary>
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }

    /// <summary>
    /// Enregistre la configuration CORS pour autoriser le frontend Ionic.
    /// </summary>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowIonicApp", policy =>
            {
                policy
                    .SetIsOriginAllowed(origin =>
                        origin.StartsWith("http://localhost") ||
                        origin.StartsWith("https://localhost") ||
                        origin.StartsWith("capacitor://") ||
                        origin.StartsWith("ionic://"))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    /// <summary>
    /// Enregistre Swagger/OpenAPI pour la documentation de l'API.
    /// </summary>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "NFC Card Management API",
                Version = "v1",
                Description = """
                    API de gestion des cartes NFC pour chauffeurs et véhicules.

                    ## Fonctionnalités
                    - **Chauffeurs** : consultation, génération de mot de passe, assignation de CTag
                    - **Véhicules** : consultation, assignation de CTag
                    - **Cartes NFC** : génération des données à écrire sur carte physique

                    ## Format de réponse
                    Toutes les réponses suivent le format :
                    ```json
                    { "success": true, "message": "...", "data": {...}, "errors": [] }
                    ```

                    ## Codes HTTP
                    | Code | Signification |
                    |------|---------------|
                    | 200  | Succès |
                    | 400  | Validation échouée |
                    | 404  | Ressource non trouvée |
                    | 409  | Conflit (mot de passe ou CTag déjà existant) |
                    | 422  | Conditions NFC non remplies |
                    | 500  | Erreur interne |
                    """,
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Support NFC Card Management"
                }
            });

            // Résoudre les conflits de schemaId pour les types homonymes dans des namespaces différents
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

            // Grouper les endpoints par tag
            options.TagActionsBy(api => new[]
            {
                api.GroupName ?? api.ActionDescriptor.RouteValues["controller"]
            });

            // Inclure les commentaires XML pour la documentation Swagger
            var xmlFile = $"{typeof(DependencyInjection).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}
