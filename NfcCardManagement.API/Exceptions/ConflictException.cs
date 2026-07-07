namespace NfcCardManagement.API.Exceptions;

/// <summary>Exception levée en cas de conflit métier (HTTP 409).</summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
