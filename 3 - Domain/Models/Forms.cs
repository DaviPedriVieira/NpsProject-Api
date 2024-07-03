namespace NpsApi.Models
{
  public class Forms
  {
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Questions> Questions { get; set; }

    public Forms()
    {
      Questions = new List<Questions>();
    }
  }
}
