using System.Collections.Generic;
using System.ComponentModel;

namespace TNT.Reflection
{
	class PropertyDescriptorComparer : IEqualityComparer<PropertyDescriptor>
	{
		public bool Equals(PropertyDescriptor x, PropertyDescriptor y)
		{
			return x.Name == y.Name &&
			x.PropertyType == y.PropertyType &&
			x.DisplayName == y.DisplayName;
		}

		public int GetHashCode(PropertyDescriptor obj) => obj.Name.GetHashCode() + obj.PropertyType.GetHashCode();
	}
}
