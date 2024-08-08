using System.Diagnostics.CodeAnalysis;
using TNT.Reflection;

namespace NUnitTests;

[ExcludeFromCodeCoverage]
public class ExtensionTests
{
  [Test]
  public void Extension_GetPropertyDescriptors_Test()
  {
    var sut1 = new TestClass1();
    var sut2 = new TestClass2();

    var propDesc1 = sut1.GetPropertyDescriptors();
    Assert.That(propDesc1.Count, Is.EqualTo(13));
    var propDesc2 = sut2.GetPropertyDescriptors();
    Assert.That(propDesc2.Count, Is.EqualTo(11));

    propDesc1 = sut1.GetPropertyDescriptors(pd => { return pd.IsBrowsable && !pd.IsReadOnly; });
    Assert.That(propDesc1.Count, Is.EqualTo(10));
  }

  [Test]
  public void Extension_GetCommonPropertyDescriptors_Test()
  {
    var sut = new List<object>(new object[]
    {
      new TestClass1(),
      new TestClass2()
    });

    var result = sut.GetCommonPropertyDescriptors();
    Assert.That(result.Count, Is.EqualTo(8));

    result = sut.GetCommonPropertyDescriptors(pd => { return pd.IsBrowsable && !pd.IsReadOnly && pd.Converter is BaseTypeConverter; });
    Assert.That(result.Count, Is.EqualTo(1));
  }

  [Test]
  public void Extension_GetPropertyInfos_Test()
  {
    var sut1 = new TestClass1();
    var sut2 = new TestClass2();

    var result1 = sut1.GetPropertyInfos();
    Assert.That(result1.Count, Is.EqualTo(13));
    var result2 = sut2.GetPropertyInfos();
    Assert.That(result2.Count, Is.EqualTo(11));

    result1 = sut1.GetPropertyInfos(pi => { return pi.PropertyType == typeof(int); });
    Assert.That(result1.Count, Is.EqualTo(2));
  }

  [Test]
  public void Extension_GetDistinctDisplayNames_Test()
  {
    var sut = new List<object>(new object[]
      {
        new TestClass1(),
        new TestClass2()
      });

    var result = sut.GetDistinctDisplayNames();
    Assert.That(string.Join(",", result.ToArray()), Is.EqualTo("ListValue,Property1,IntProperty,StringProperty,DoubleProperty,IntListProperty,StringListProperty,HiddenProperty,ReadOnlyProperty"));

    result = sut.GetDistinctDisplayNames(pd => { return pd.Converter is BaseTypeConverter; });
    Assert.That(string.Join(",", result.ToArray()), Is.EqualTo("ListValue"));
  }

  [Test]
  public void Extension_GetStandardValues_Test()
  {
    var sut = new TestClass1();
    var sutProps = new List<object>(new object[] { sut }).GetDistinctDisplayNames();

    var pds = sut.GetPropertyDescriptors();

    pds.ForEach(pd =>
    {
      var values = sut.GetStandardValues(pd.DisplayName);

      if (pd.DisplayName == "ListValue" || pd.DisplayName == "DynamicProperty")
      {
        Assert.That(values.Count, Is.EqualTo(5));
      }
      else
      {
        Assert.That(values.Count, Is.EqualTo(0));
      }
    });
  }

  [Test]
  public void Extension_GetDistinctStandardValues_Test()
  {
    var objs = new List<object>(new object[] { new TestClass1(), new TestClass2() });
    var values = objs.GetDistinctStandardValues("ListValue");
    Assert.That(values.Count, Is.EqualTo(3));
  }

  [Test]
  public void Extension_Test()
  {
    var sut1 = new TestClass1();
    var sut2 = new TestClass2();
    var objs = new List<object>(new object[] { sut1, sut2 });

    // Get common properties
    var distinctProperties = objs.GetDistinctDisplayNames(pd =>
    {
      return pd.Converter is BaseTypeConverter;
    });

    distinctProperties.ForEach(propertyName =>
    {
      var distinctStandardValues = objs.GetDistinctStandardValues(propertyName);
      var commonValue = objs.GetCommonValue(propertyName);
      Assert.That(commonValue, Is.Null);
    });

    sut1.ListValue = sut2.ListValue;

    distinctProperties.ForEach(propertyName =>
    {
      var distinctStandardValues = objs.GetDistinctStandardValues(propertyName);
      var commonValue = objs.GetCommonValue(propertyName);
      Assert.That(commonValue, Is.EqualTo(sut1.ListValue));
    });
  }

  [Test]
  public void Extension_Invalid_Property_Test()
  {
    var sut = new TestClass1();
    Assert.That(sut.GetPropertyValue<string>("foo"), Is.Null);
  }
}