namespace NpsApi.Models
{
  public class Question
  {
    public int Id { get; set; }
    public int FormId { get; set; }
    public string Content { get; set; } = string.Empty;
    public ICollection<Answer> Answers { get; set; }

    public Question()
    {
     Answers = new List<Answer>();
    }
  }
}
