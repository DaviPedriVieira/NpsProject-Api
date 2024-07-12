namespace NpsApi.Models
{
  public class Answers
  {
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int Grade { get; set; }
    public string Notes { get; set; } = string.Empty;

  }
}
