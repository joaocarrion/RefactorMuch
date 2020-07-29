using Newtonsoft.Json.Linq;
using RefactorMuch.Configuration;
using System.Collections.Generic;
using System.Text;

namespace RefactorMuch.Parse
{
  public class RuleSet
  {
    public string Name { get; set; }
    public string Description { get; set; }

    private List<Rule> rules { get; set; } = new List<Rule>();
    private Rule compiledRule = null;

    public RuleSet(string name, string descritption)
    {
      Name = name;
      Description = descritption;
    }

    public string Execute(string source)
    {
      if (compiledRule == null)
      {
        string result = source;
        foreach (var rule in rules)
          result = rule.Execute(result);

        return result;
      }
      else
        return compiledRule.Execute(source);
    }

    public void ToJson(JObject jObject)
    {
      jObject.Add("name", Name);
      jObject.Add("description", Description);
      var ruleArray = new JArray();
      foreach (var rule in rules)
        ruleArray.Add(new JObject(
          new JProperty("name", rule.Name),
          new JProperty("descritpion", rule.Description),
          new JProperty("expression", rule.Expression),
          new JProperty("replace", rule.Replace)
          ));
    }

    public static RuleSet FromJson(JObject jObject)
    {
      bool hasReplace = false;

      var set = new RuleSet(jObject.Value<string>("name"), jObject.Value<string>("description"));
      var array = jObject.Value<JArray>("rules");
      foreach (JObject value in array) {
        var replace = value.ContainsKey("replace");
        if (replace) hasReplace = replace;

        set.rules.Add(new Rule
        {
          Name = value.Value<string>("name"),
          Description = value.Value<string>("description"),
          Expression = value.Value<string>("expression"),
          Replace = replace ? value.Value<string>("replace") : ""
        });
      }

      if (!hasReplace) // can compile the rules
      {
        StringBuilder compiled = new StringBuilder();
        if (set.rules.Count > 0)
          compiled.Append(set.rules[0].Expression);
        for (int i = 1; i < set.rules.Count; ++i)
          compiled.Append("|" + set.rules[i].Expression);

        set.compiledRule = new Rule("Compiled", "Compiled", compiled.ToString());
      }

      return set;
    }

    private static object createLock = new object();
    private static Dictionary<string, RuleSet> configSets = null;

    public static Dictionary<string, RuleSet> FromConfiguration()
    {
      lock (createLock)
      {
        if (configSets == null)
        {
          configSets = new Dictionary<string, RuleSet>();
          var doc = ConfigData.GetInstance().doc;
          var sets = doc.Value<JArray>("rules");

          foreach (var ruleset in sets)
          {
            var rs = RuleSet.FromJson((JObject)ruleset);
            configSets.Add(rs.Name, rs);
          }
        }
      }

      return configSets;
    }
  }
}
