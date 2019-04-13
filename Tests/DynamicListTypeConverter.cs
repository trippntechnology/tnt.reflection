using System.Collections.Generic;
using System.Linq;
using TNT.Reflection;

namespace Tests
{
	class DynamicListTypeConverter : BaseTypeConverter
	{
		protected override List<string> List
		{
			get
			{
				var obj = base.m_Object as TestClass1;
				return obj?.StringArrayProperty.ToList();
			}
		}
	}
}