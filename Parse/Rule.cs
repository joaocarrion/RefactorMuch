using System.Text.RegularExpressions;

namespace RefactorMuch.Parse
{
  public class Rule
  {
    private Regex regex;

    public string Name { get; set; }
    public string Description { get; set; }

    public string Expression { get => regex.ToString(); set => regex = new Regex(value); }

    public Rule() { }

    public Rule(string name, string description, string expression)
    {
      Name = name;
      Description = description;
      Expression = expression;
    }

    public string Execute(string source, string[] parameters = null)
    {
      return regex.Replace(source, "");
    }
  }
}
