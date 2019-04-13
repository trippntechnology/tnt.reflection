using System.ComponentModel;

namespace Tests
{
	class TestClass1 : BaseTestClass
	{
		[TypeConverter(typeof(ListTypeConverter1))]
		public string ListValue { get; set; } = "two";

		public int Int1 { get; set; }
		public string String { get; set; }
		public long Property1 { get; set; }

		[Browsable(false)]
		public string[] StringArrayProperty { get; set; } = new string[] { "red", "yellow", "blue", "green", "orange" };

		[TypeConverter(typeof(DynamicListTypeConverter))]
		public string DynamicProperty { get; set; } = "blue";
	}
}
