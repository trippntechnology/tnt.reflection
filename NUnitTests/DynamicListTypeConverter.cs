using TNT.Reflection;

namespace NUnitTests;

class DynamicListTypeConverter : BaseTypeConverter
{
  protected override List<string> List
  {
    get
    {
      var obj = m_Object as TestClass1;
      return obj?.StringArrayProperty.ToList();
    }
  }
}