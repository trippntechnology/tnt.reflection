using TNT.Reflection;

namespace NUnitTests;

class ListTypeConverter2 : BaseTypeConverter
{
  List<string> _list = new List<string>(new string[] { "one", "three", "five" });
  protected override List<string> List => _list;
}
