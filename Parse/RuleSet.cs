using Newtonsoft.Json.Linq;
using RefactorMuch.Configuration;
using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public class RuleSet
  {
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Rule> Rules { get; private set; } = new List<Rule>();

    public RuleSet(string name, string descritption)
    {
      Name = name;
      Description = descritption;
    }

    public string Execute(string source)
    {
      string result = source;
      foreach (var rule in Rules)
        result = rule.Execute(result);

      return result;
    }

    public void ToJson(JObject jObject)
    {
      jObject.Add("name", Name);
      jObject.Add("description", Description);
      var ruleArray = new JArray();
      foreach (var rule in Rules)
        ruleArray.Add(new JObject(
          new JProperty("name", rule.Name),
          new JProperty("descritpion", rule.Description),
          new JProperty("expression", rule.Expression),
          new JProperty("replace", rule.Replace)
          ));
    }

    public static RuleSet FromJson(JObject jObject)
    {
      var set = new RuleSet(jObject.Value<string>("name"), jObject.Value<string>("description"));
      var array = jObject.Value<JArray>("rules");
      foreach (JObject value in array)
        set.Rules.Add(new Rule
        {
          Name = value.Value<string>("name"),
          Description = value.Value<string>("description"),
          Expression = value.Value<string>("expression"),
          Replace = value.ContainsKey("replace") ? value.Value<string>("replace") : ""
        });

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
