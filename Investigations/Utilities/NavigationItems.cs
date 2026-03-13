namespace Investigations.Utilities;

public static class NavigationItems
{
    public static List<NavigationItem> GetNavigationItems() => new()
    {
            new NavigationItem("Dashboard", "/Index"),
            new NavigationItem("Cases", "/Cases/Index"),
            new NavigationItem("Clients", "/Clients/Index"),
            new NavigationItem("Accounts", "/Account/Index"),
    };
}

public record NavigationItem(string Name, string PageRoute);
