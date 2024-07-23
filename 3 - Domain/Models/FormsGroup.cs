using System.Xml.Linq;

namespace NpsApi.Models
{
  public class FormsGroup
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Form> Forms { get; set; }

    public FormsGroup()
    {
      Forms = new List<Form>();
    }
  }
}
