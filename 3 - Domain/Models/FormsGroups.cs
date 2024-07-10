namespace NpsApi.Models
{
  public class FormsGroups
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Forms> Forms { get; set; }

    public FormsGroups()
    {
      Forms = new List<Forms>();
    }
  }
}
