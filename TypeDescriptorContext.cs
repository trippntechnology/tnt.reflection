using System;
using System.ComponentModel;

namespace TNT.Reflection
{
	/// <summary>
	/// Class used to pass an <see cref="object"/> and <see cref="PropertyDescriptor"/> to <see cref="TypeConverter.GetStandardValues(ITypeDescriptorContext)"/>
	/// </summary>
	public class TypeDescriptorContext : ITypeDescriptorContext
	{
		/// <summary>
		/// Not implemented
		/// </summary>
		public IContainer Container => throw new NotImplementedException();

		/// <summary>
		/// The <see cref="object"/> that is of interest
		/// </summary>
		public object Instance { get; private set; }

		/// <summary>
		/// The <see cref="PropertyDescriptor"/> that is of interest
		/// </summary>
		public PropertyDescriptor PropertyDescriptor { get; private set; }

		/// <summary>
		/// Initializing constructor
		/// </summary>
		/// <param name="instance"><see cref="object"/> of interest</param>
		/// <param name="propertyDescriptor"><see cref="PropertyDescriptor"/> of interest</param>
		public TypeDescriptorContext(object instance, PropertyDescriptor propertyDescriptor)
		{
			this.Instance = instance;
			this.PropertyDescriptor = propertyDescriptor;
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		public object GetService(Type serviceType) => throw new NotImplementedException();

		/// <summary>
		/// Not implemented
		/// </summary>
		public void OnComponentChanged() => throw new NotImplementedException();

		/// <summary>
		/// Not implemented
		/// </summary>
		public bool OnComponentChanging() => throw new NotImplementedException();
	}
}
