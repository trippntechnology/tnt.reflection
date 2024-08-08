using System.ComponentModel;
using System.Reflection;

namespace TNT.Reflection;

/// <summary>
/// Extension methods to get properties and property values of classes
/// </summary>
public static class Extensions
{
  /// <summary>
  /// Gets a <see cref="List{PropertyDescriptor}"/> for a given <see cref="object"/>
  /// </summary>
  /// <param name="obj"><see cref="object"/> whose <see cref="PropertyDescriptor"/>s are of interest</param>
  /// <param name="filter">Optional filter that can be applied to a <see cref="PropertyDescriptor"/></param>
  /// <returns><see cref="List{PropertyDescriptor}"/> for a given <see cref="object"/></returns>
  public static List<PropertyDescriptor> GetPropertyDescriptors(this object obj, Func<PropertyDescriptor, bool> filter = null)
  {
    if (filter == null) filter = (_) => { return true; };
    var pdc = TypeDescriptor.GetProperties(obj);
    return (from PropertyDescriptor pd in pdc where filter(pd) select pd).ToList();
  }

  /// <summary>
  /// Gets a common <see cref="List{PropertyDescriptor}"/> for a <see cref="List{Object}"/>
  /// </summary>
  /// <param name="objs"><see cref="List{Object}"/> whose common <see cref="PropertyDescriptor"/>s are of interest</param>
  /// <param name="filter">Optional filter that can be applied to a <see cref="PropertyDescriptor"/></param>
  /// <returns>Common <see cref="List{PropertyDescriptor}"/> for a given <see cref="List{Object}"/></returns>
  public static List<PropertyDescriptor> GetCommonPropertyDescriptors(this List<object> objs, Func<PropertyDescriptor, bool> filter = null)
  {
    List<PropertyDescriptor> pds = null;

    objs.ForEach(o =>
    {
      if (pds == null) { pds = o.GetPropertyDescriptors(filter); }
      else { pds = pds.Intersect(o.GetPropertyDescriptors(filter), new PropertyDescriptorComparer()).ToList(); }
    });

    return pds;
  }

  /// <summary>
  /// Gets a <see cref="List{String}"/> of distinct property names for a give <see cref="List{Object}"/>
  /// </summary>
  /// <param name="objs"><see cref="List{Object}"/> whose properties are of interest</param>
  /// <param name="filter">Optional filter that can be applied to a <see cref="PropertyDescriptor"/></param>
  /// <returns>A distinct <see cref="List{String}"/> of property names associated with <see cref="List{Object}"/></returns>
  public static List<string> GetDistinctDisplayNames(this List<object> objs, Func<PropertyDescriptor, bool> filter = null)
  {
    if (filter == null) filter = (_) => { return true; };
    List<string> names = null;

    objs.ForEach(o =>
    {
      if (names == null) { names = o.GetPropertyDescriptors(filter).ConvertAll(pd => pd.DisplayName); }
      else { names = names.Intersect(o.GetPropertyDescriptors(filter).ConvertAll(pd => pd.DisplayName)).ToList(); }
    });

    return names;
  }

  /// <summary>
  /// Gets a <see cref="List{PropertyInfo}"/> for a given <see cref="object"/>
  /// </summary>
  /// <param name="obj"><see cref="Object"/> whose <see cref="PropertyInfo"/> are of interest</param>
  /// <param name="filter">Optional filter that can be applied to a <see cref="PropertyInfo"/></param>
  /// <returns>A <see cref="List{PropertyInfo}"/> for a given <see cref="object"/></returns>
  public static List<PropertyInfo> GetPropertyInfos(this object obj, Func<PropertyInfo, bool> filter = null)
  {
    if (filter == null) filter = (_) => { return true; };

    return (from pi in obj.GetType().GetProperties() where filter(pi) select pi).ToList();
  }

  /// <summary>
  /// Gets a value associated with the <paramref name="propertyName"/>
  /// </summary>
  /// <typeparam name="T">Type of property</typeparam>
  /// <param name="obj"><see cref="object"/> whose property value is of interest</param>
  /// <param name="propertyName">Name of property</param>
  /// <returns>Value of the property if it exists, null otherwise</returns>
  public static T GetPropertyValue<T>(this object obj, string propertyName)
  {
    // Find by PropertyDescriptor first so that both Name and DisplayName can be queried
    var pd = obj.GetPropertyDescriptors(p => { return p.Name == propertyName || p.DisplayName == propertyName; }).FirstOrDefault();
    if (pd == null)
    {
      return default(T);
    }
    else
    {
      return (T)obj.GetType().GetProperty(pd.Name)?.GetValue(obj);
    }
  }

  /// <summary>
  /// Gets a <see cref="List{String}"/> of values that can be selected on the given <paramref name="propertyName"/>
  /// </summary>
  /// <param name="obj"><see cref="object"/> whose values are of interest</param>
  /// <param name="propertyName">Name of property</param>
  /// <returns><see cref="List{String}"/> of values that can be selected on the property</returns>
  public static List<string> GetStandardValues(this object obj, string propertyName)
  {
    var propDesc = obj.GetPropertyDescriptors(pd => { return pd.DisplayName == propertyName; }).FirstOrDefault();
    var collection = propDesc.Converter.GetStandardValues(new TypeDescriptorContext(obj, propDesc));
    List<string> result = null;
    if (collection != null)
    {
      result = (from string c in collection select c).ToList();
    }
    else
    {
      result = new List<string>();
    }

    return result;
  }

  /// <summary>
  /// Gets a <see cref="List{String}"/> of distinct values that can be selected on the given <paramref name="propertyName"/>
  /// </summary>
  /// <param name="objs"><see cref="List{Object}"/> whose property is of interest</param>
  /// <param name="propertyName">Name of property to query</param>
  /// <returns><see cref="List{String}"/> of distinct values that can be selected on the property across all <paramref name="objs"/></returns>
  public static List<string> GetDistinctStandardValues(this List<object> objs, string propertyName)
  {
    List<string> values = null;

    objs.ForEach(o =>
    {
      if (values == null)
      {
        values = o.GetStandardValues(propertyName);
      }
      else
      {
        values = values.Intersect(o.GetStandardValues(propertyName)).ToList();
      }
    });

    return values;
  }

  /// <summary>
  /// Gets a common value across all <paramref name="objs"/> if it exists
  /// </summary>
  /// <param name="objs"><see cref="List{Object}"/>s whose property is of interest</param>
  /// <param name="propertyName">Name of property</param>
  /// <returns>The common value on the property across all <paramref name="objs"/> if exists, otherwise null</returns>
  public static string GetCommonValue(this List<object> objs, string propertyName)
  {
    string commonValue = null;

    foreach (var o in objs)
    {
      if (commonValue == null)
      {
        commonValue = o.GetPropertyValue<string>(propertyName);
      }
      else
      {
        var value = o.GetPropertyValue<string>(propertyName);
        if (value != commonValue)
        {
          return null;
        }
      }
    }

    return commonValue;
  }
}