namespace NfcCardManagement.API.Exceptions;

/// <summary>Exception levée quand une ressource n'est pas trouvée (HTTP 404).</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
