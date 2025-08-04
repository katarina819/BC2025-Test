using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class NameIdentifierUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Ovo koristi ClaimTypes.NameIdentifier = ASP.NET Identity korisnički ID
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
