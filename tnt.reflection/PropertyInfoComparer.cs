using System.Collections.Generic;
using System.Reflection;

namespace TNT.Reflection
{
	class PropertyInfoComparer : IEqualityComparer<PropertyInfo>
	{
		public bool Equals(PropertyInfo x, PropertyInfo y) => x.Name == y.Name && x.PropertyType == y.PropertyType;

		public int GetHashCode(PropertyInfo obj) => obj.Name.GetHashCode() + obj.PropertyType.GetHashCode();
	}
}