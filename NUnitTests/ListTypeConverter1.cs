using TNT.Reflection;

namespace NUnitTests;

class ListTypeConverter1 : BaseTypeConverter
{
  List<string> _List = new List<string>(new string[] { "one", "two", "three", "four", "five" });

  protected override List<string> List => _List;
}