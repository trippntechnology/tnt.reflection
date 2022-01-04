using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TNT.Reflection
{
	/// <summary>
	/// Base type converter that can be extended to provide a list of standard values (see <see cref="GetStandardValues(ITypeDescriptorContext)"/>)
	/// </summary>
	public abstract class BaseTypeConverter : TypeConverter
	{
		/// <summary>
		/// Member object to hold the <see cref="PropertyDescriptor"/> passed into <see cref="GetPropertyValues"/> so that it can be used
		/// by <see cref="List"/>.
		/// </summary>
		protected PropertyDescriptor m_PropertyDescriptor = null;

		/// <summary>
		/// Member object to hold the object passed into <see cref="GetPropertyValues"/> so that it can be used
		/// by <see cref="List"/>.
		/// </summary>
		protected object m_Object = null;

		/// <summary>
		/// <see cref="List{String}"/> that contains the string values associated with the converter. Must be initialized
		/// by subclasses.
		/// </summary>
		protected abstract List<string> List { get; }

		/// <summary>
		/// Returns the string value located at the index within <see cref="List"/> if it exists, string.empty otherwise.
		/// </summary>
		/// <param name="index">Index in the array were value resides</param>
		/// <returns>String value located at the index within <see cref="List"/> if it exists, string.empty otherwise</returns>
		virtual public string this[int index]
		{
			get
			{
				if (List != null && index < List.Count)
				{
					return List[index];
				}

				return string.Empty;
			}
		}

		#region TypeConverter Overrides

		/// <summary>
		/// Specifies that this object supports a standard set of values that can be picked from a list
		/// using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context, specifically a <see cref="object"/>
		/// and <see cref="PropertyDescriptor"/>.</param>
		/// <returns>True if TypeConverter.GetStandardValues() should be called to find a common set of 
		/// values the object supports; otherwise, false.</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

		/// <summary>
		/// Returns a <see cref="TypeConverter.StandardValuesCollection"/> of values that can be chosen for the instance in this context. Instance may be an array in
		/// which case only the intersection of common values are returned. 
		/// </summary>
		/// <param name="context">Indicates the context for the request</param>
		/// <returns><see cref="TypeConverter.StandardValuesCollection"/> of common values that can be chosen for this instance/instances</returns>
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] standardValues = new string[0];

			if (context.Instance.GetType().IsArray)
			{
				var objs = context.Instance as object[];
				List<string> intersection = null;

				foreach (object obj in objs)
				{
					// Get the common descriptions across all instances
					if (intersection == null)
					{
						intersection = GetPropertyValues(context.PropertyDescriptor, obj);
					}
					else
					{
						intersection = intersection.Intersect(GetPropertyValues(context.PropertyDescriptor, obj)).ToList();
					}
				}

				standardValues = intersection.ToArray();
			}
			else
			{
				standardValues = GetPropertyValues(context.PropertyDescriptor, context.Instance).ToArray();
			}

			return new StandardValuesCollection(standardValues);
		}

		#endregion

		/// <summary>
		/// Initializes <see cref="m_PropertyDescriptor"/> and <see cref="m_Object"/> with <paramref name="propertyDescriptor"/> and 
		/// <paramref name="obj"/> respectively.
		/// </summary>
		/// <param name="propertyDescriptor">Property descriptor being queried</param>
		/// <param name="obj">Object that the property belongs to</param>
		/// <returns>Returns <see cref="List"/> associated with the Property Descriptor being queried</returns>
		virtual protected List<string> GetPropertyValues(PropertyDescriptor propertyDescriptor, object obj)
		{
			m_PropertyDescriptor = propertyDescriptor;
			m_Object = obj;

			return List;
		}

		/// <summary>
		/// Returns the index of the value in <see cref="List"/> if exists
		/// </summary>
		/// <param name="value">Value who's index should be found</param>
		/// <returns>Zero based index if value exists, -1 otherwise</returns>
		virtual public int IndexOf(string value)
		{
			if (List != null)
			{
				return List.IndexOf(value);
			}

			return -1;
		}
	}
}