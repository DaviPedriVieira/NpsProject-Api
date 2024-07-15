namespace NpsApi.Models
{
  public class Form
  {
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; }

    public Form()
    {
      Questions = new List<Question>();
    }
  }
}
