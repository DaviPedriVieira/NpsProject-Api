using NpsApi._3___Domain.Enums;

namespace NpsApi.Models
{
  public class Users
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserType Type { get; set; }
  }
}
