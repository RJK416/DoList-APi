using Microsoft.AspNetCore.Identity;

namespace FinalDoListAPI.Models;

public class AccountModel
{
    public int Id { get; set; }

    public string AccName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;

    public int AccountModelId { get; set; }
}
