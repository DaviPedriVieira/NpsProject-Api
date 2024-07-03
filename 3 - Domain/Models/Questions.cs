namespace NpsApi.Models
{
  public class Questions
  {
    public int Id { get; set; }
    public int FormId { get; set; }
    public string Content { get; set; } = string.Empty;

    public ICollection<Answers> Answers { get; set; }

    public Questions()
    {
      Answers = new List<Answers>();
    }
  }
}
