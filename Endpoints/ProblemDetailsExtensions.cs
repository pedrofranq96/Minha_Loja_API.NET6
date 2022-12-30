using Flunt.Notifications;

namespace ProdutosApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]>ConvertProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
                .GroupBy(g => g.Key) //agrupando as kies (nomes dos campos)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray());
    }
}
