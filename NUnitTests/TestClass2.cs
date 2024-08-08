using System.ComponentModel;

namespace NUnitTests;

class TestClass2 : BaseTestClass
{
  [TypeConverter(typeof(ListTypeConverter2))]
  public string ListValue { get; set; } = "three";

  public int Int2 { get; set; }
  public string String2 { get; set; }
  public double Property1 { get; set; }
}
