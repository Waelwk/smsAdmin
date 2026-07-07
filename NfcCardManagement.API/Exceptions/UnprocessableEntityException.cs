namespace NfcCardManagement.API.Exceptions;

/// <summary>Exception levée quand les conditions métier ne sont pas remplies (HTTP 422).</summary>
public class UnprocessableEntityException : Exception
{
    public UnprocessableEntityException(string message) : base(message) { }
}
