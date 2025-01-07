using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => new Error("User.NotFound", "Cannot found the user");
    public static Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");
}
