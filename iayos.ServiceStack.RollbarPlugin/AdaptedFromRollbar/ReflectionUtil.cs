using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>A utility class aiding with .NET Reflection.</summary>
	internal static class ReflectionUtil
	{
		/// <summary>Gets all public instance properties.</summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static PropertyInfo[] GetAllPublicInstanceProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}

		/// <summary>Gets all public static fields.</summary>
		/// <param name="type">The type.</param>
		/// <returns>All discovered FieldInfos.</returns>
		public static FieldInfo[] GetAllPublicStaticFields(Type type)
		{
			return type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
		}

		/// <summary>Gets the static field value.</summary>
		/// <typeparam name="TFieldDataType">The type of the field data type.</typeparam>
		/// <param name="staticField">The static field.</param>
		/// <returns></returns>
		public static TFieldDataType GetStaticFieldValue<TFieldDataType>(FieldInfo staticField)
		{
			Assumption.AssertTrue(staticField.IsStatic, "IsStatic");
			return (TFieldDataType)staticField.GetValue((object)null);
		}

		/// <summary>Gets all public static field values.</summary>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="type">The type.</param>
		/// <returns>All the field values.</returns>
		public static TField[] GetAllPublicStaticFieldValues<TField>(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
			List<TField> fieldList = new List<TField>(fields.Length);
			foreach (FieldInfo fieldInfo in fields)
				fieldList.Add((TField)fieldInfo.GetValue((object)null));
			return fieldList.ToArray();
		}

		/// <summary>Gets the nested types.</summary>
		/// <param name="hostType">Type of the host.</param>
		/// <param name="nestedTypesBindingFlags">The nested types binding flags.</param>
		/// <returns></returns>
		public static Type[] GetNestedTypes(Type hostType, BindingFlags nestedTypesBindingFlags)
		{
			return hostType.GetNestedTypes(nestedTypesBindingFlags);
		}

		/// <summary>Gets the nested type by its name.</summary>
		/// <param name="hostType">Type of the host.</param>
		/// <param name="nestedTypeName">Name of the nested type (without its namespace).</param>
		/// <param name="nestedTypeBindingFlags">The nested type binding flags.</param>
		/// <returns></returns>
		public static Type GetNestedTypeByName(Type hostType, string nestedTypeName, BindingFlags nestedTypeBindingFlags)
		{
			return hostType.GetNestedType(nestedTypeName, nestedTypeBindingFlags);
		}

		/// <summary>
		/// Gets the sub classes of a given type
		/// from the same assembly where the base type is defined.
		/// </summary>
		/// <param name="baseType">Type of the base.</param>
		/// <returns>Array of the derived types</returns>
		public static Type[] GetSubClassesOf(Type baseType)
		{
			List<Type> list = ((IEnumerable<Type>)baseType.Assembly.GetTypes()).Where<Type>((Func<Type, bool>)(t => t.IsSubclassOf(baseType))).ToList<Type>();
			if (!list.Any<Type>())
				list.Add(baseType);
			return list.ToArray();
		}
	}
}