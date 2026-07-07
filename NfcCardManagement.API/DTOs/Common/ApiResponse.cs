namespace NfcCardManagement.API.DTOs.Common;

/// <summary>
/// Réponse standardisée pour toutes les réponses de l'API.
/// Format : { "success": bool, "message": string, "data": T?, "errors": string[] }
/// </summary>
/// <typeparam name="T">Type des données retournées.</typeparam>
public class ApiResponse<T>
{
    /// <summary>Indique si l'opération a réussi.</summary>
    public bool Success { get; set; }

    /// <summary>Message descriptif de l'opération.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Données retournées. Null en cas d'erreur.</summary>
    public T? Data { get; set; }

    /// <summary>Liste des erreurs de validation ou messages d'erreur. Vide en cas de succès.</summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Crée une réponse de succès avec données.
    /// </summary>
    /// <param name="data">Données à retourner.</param>
    /// <param name="message">Message de succès (optionnel).</param>
    public static ApiResponse<T> Ok(T data, string message = "Opération réussie")
        => new()
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };

    /// <summary>
    /// Crée une réponse d'erreur sans données.
    /// </summary>
    /// <param name="message">Message d'erreur.</param>
    /// <param name="errors">Liste de messages d'erreur détaillés (optionnel).</param>
    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null)
        => new()
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors?.ToList() ?? new List<string>()
        };

    /// <summary>
    /// Crée une réponse d'erreur de validation (HTTP 400).
    /// </summary>
    /// <param name="errors">Liste des messages de validation FluentValidation.</param>
    /// <param name="message">Message général (optionnel).</param>
    public static ApiResponse<T> ValidationFail(IEnumerable<string> errors, string message = "Erreur de validation")
        => new()
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors.ToList()
        };
}

/// <summary>
/// Version non-générique d'ApiResponse pour les réponses sans données.
/// </summary>
public class ApiResponse : ApiResponse<object?>
{
    /// <summary>
    /// Crée une réponse de succès sans données.
    /// </summary>
    /// <param name="message">Message de succès.</param>
    public static ApiResponse OkNoData(string message = "Opération réussie")
        => new()
        {
            Success = true,
            Message = message,
            Data = null,
            Errors = new List<string>()
        };

    /// <summary>
    /// Crée une réponse d'erreur sans données (version non-générique).
    /// </summary>
    /// <param name="message">Message d'erreur.</param>
    /// <param name="errors">Liste de messages d'erreur détaillés (optionnel).</param>
    public new static ApiResponse Fail(string message, IEnumerable<string>? errors = null)
        => new()
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors?.ToList() ?? new List<string>()
        };
}
