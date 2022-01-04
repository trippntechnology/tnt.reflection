using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TNT.Reflection;

namespace Tests
{
	[TestClass]
	public class ExtensionTests
	{
		[TestMethod]
		public void Extension_GetPropertyDescriptors_Test()
		{
			var sut1 = new TestClass1();
			var sut2 = new TestClass2();

			var propDesc1 = sut1.GetPropertyDescriptors();
			Assert.AreEqual(13, propDesc1.Count);
			var propDesc2 = sut2.GetPropertyDescriptors();
			Assert.AreEqual(11, propDesc2.Count);

			propDesc1 = sut1.GetPropertyDescriptors(pd => { return pd.IsBrowsable && !pd.IsReadOnly; });
			Assert.AreEqual(10, propDesc1.Count);
		}

		[TestMethod]
		public void Extension_GetCommonPropertyDescriptors_Test()
		{
			var sut = new List<object>(new object[]
			{
				new TestClass1(),
				new TestClass2()
			});

			var result = sut.GetCommonPropertyDescriptors();
			Assert.AreEqual(8, result.Count);

			result = sut.GetCommonPropertyDescriptors(pd => { return pd.IsBrowsable && !pd.IsReadOnly && pd.Converter is BaseTypeConverter; });
			Assert.AreEqual(1, result.Count);
		}

		[TestMethod]
		public void Extension_GetPropertyInfos_Test()
		{
			var sut1 = new TestClass1();
			var sut2 = new TestClass2();

			var result1 = sut1.GetPropertyInfos();
			Assert.AreEqual(13, result1.Count);
			var result2 = sut2.GetPropertyInfos();
			Assert.AreEqual(11, result2.Count);

			result1 = sut1.GetPropertyInfos(pi => { return pi.PropertyType == typeof(int); });
			Assert.AreEqual(2, result1.Count);
		}

		[TestMethod]
		public void Extension_GetDistinctDisplayNames_Test()
		{
			var sut = new List<object>(new object[]
				{
					new TestClass1(),
					new TestClass2()
				});

			var result = sut.GetDistinctDisplayNames();
			Assert.AreEqual("ListValue,Property1,IntProperty,StringProperty,DoubleProperty,IntListProperty,StringListProperty,HiddenProperty,ReadOnlyProperty", string.Join(",", result.ToArray()));

			result = sut.GetDistinctDisplayNames(pd => { return pd.Converter is BaseTypeConverter; });
			Assert.AreEqual("ListValue", string.Join(",", result.ToArray()));
		}

		[TestMethod]
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
					Assert.AreEqual(5, values.Count);
				}
				else
				{
					Assert.AreEqual(0, values.Count);
				}
			});
		}

		[TestMethod]
		public void Extension_GetDistinctStandardValues_Test()
		{
			var objs = new List<object>(new object[] { new TestClass1(), new TestClass2() });
			var values = objs.GetDistinctStandardValues("ListValue");
			Assert.AreEqual(3, values.Count);
		}

		[TestMethod]
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
				Assert.IsNull(commonValue);
			});

			sut1.ListValue = sut2.ListValue;

			distinctProperties.ForEach(propertyName =>
			{
				var distinctStandardValues = objs.GetDistinctStandardValues(propertyName);
				var commonValue = objs.GetCommonValue(propertyName);
				Assert.AreEqual(sut1.ListValue, commonValue);
			});
		}

		[TestMethod]
		public void Extension_Invalid_Property_Test()
		{
			var sut = new TestClass1();
			Assert.IsNull(sut.GetPropertyValue<string>("foo"));
		}
	}
}