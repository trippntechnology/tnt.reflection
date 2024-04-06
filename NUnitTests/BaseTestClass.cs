using System.ComponentModel;

namespace NUnitTests;

class BaseTestClass
{
  public int IntProperty { get; set; }
  public string StringProperty { get; set; }
  public double DoubleProperty { get; set; }
  public List<int> IntListProperty { get; set; }
  public List<string> StringListProperty { get; set; }
  [Browsable(false)]
  public long HiddenProperty { get; set; }
  [ReadOnly(true)]
  public float ReadOnlyProperty { get; set; }
}
