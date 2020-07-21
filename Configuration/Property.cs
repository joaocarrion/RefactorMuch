using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InteractiveMerge.Configuration
{
  public class Property
  {
    public string name;
    public object value;

    public static implicit operator string(Property property)
    {
      return property.value.ToString();
    }
  }

  public class StringProperty : Property
  {
    public StringProperty() { }
    public StringProperty(string value)
    {
      this.value = value;
    }

    public string Value { get => (string)value; set => this.value = value; }
    public static implicit operator string(StringProperty p) => p.Value;

    public override string ToString()
    {
      return Value;
    }
  }
}
