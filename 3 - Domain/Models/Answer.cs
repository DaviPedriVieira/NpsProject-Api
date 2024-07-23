namespace NpsApi.Models
{
  public class Answer
  {
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int Grade { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
  }
}
