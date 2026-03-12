using System.Security.Claims;

namespace task1
{
    /// <summary>
    /// Builds message string: "The {Role} {Name} just {action}!"
    /// </summary>
    public static class ActionNotificationHelper
    {
        /// <summary>
        /// Gets the current user's display name from claims (JWT uses "name" and "email").
        /// </summary>
        public static string GetDisplayName(ClaimsPrincipal user)
        {
            var name = user?.FindFirst("name")?.Value
                ?? user?.FindFirst(ClaimTypes.Name)?.Value
                ?? user?.FindFirst("unique_name")?.Value;
            if (!string.IsNullOrWhiteSpace(name)) return name.Trim();
            var email = user?.FindFirst("email")?.Value
                ?? user?.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email)) return email.Trim();
            return "Someone";
        }

        public static string Format(string role, string name, string action)
        {
            var displayName = string.IsNullOrWhiteSpace(name) ? "Someone" : name.Trim();
            var displayRole = string.IsNullOrWhiteSpace(role) ? "User" : role.Trim();
            return $"The {displayRole} {displayName} just {action}!";
        }
    }
}
