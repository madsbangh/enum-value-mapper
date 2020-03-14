using System;
using System.Collections.Generic;
using UnityEngine;

namespace Madsbangh.EnumValueMapping
{
	public abstract class EnumValueMappingBase
	{
		[SerializeField]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Deserialized and used by drawer")]
		protected string enumTypeAssemblyQualifiedName;
	}

	/// <summary>
	/// Derive from this class and add a System.SerializableAttribute to the derived class.
	/// This will create a nice mapping in the inspector.
	/// </summary>
	/// <typeparam name="TKey">Enum to use as key</typeparam>
	/// <typeparam name="TValue">The value type to map too. Must also be serializeable</typeparam>
	public abstract class EnumValueMapping<TKey, TValue> : EnumValueMappingBase, ISerializationCallbackReceiver where TKey : Enum
	{
		[SerializeField]
		private List<TKey> keys;
		[SerializeField]
		private List<TValue> values;
		private Dictionary<TKey, TValue> mapping;

		public TValue GetValue(TKey key) => mapping[key];
		public IEnumerable<KeyValuePair<TKey, TValue>> Items => mapping;

		public void OnAfterDeserialize()
		{
			mapping = new Dictionary<TKey, TValue>(keys.Count);

			for (int i = 0; i < keys.Count; i++)
			{
				mapping.Add(keys[i], values[i]);
			}
		}

		public void OnBeforeSerialize()
		{
			enumTypeAssemblyQualifiedName = typeof(TKey).AssemblyQualifiedName;

			if (mapping == null)
			{
				mapping = new Dictionary<TKey, TValue>();
			}

			foreach (TKey key in Enum.GetValues(typeof(TKey)))
			{
				if (!mapping.ContainsKey(key))
				{
					mapping.Add(key, default);
				}
			}

			keys = new List<TKey>(mapping.Count);
			values = new List<TValue>(mapping.Count);

			foreach (var kvp in mapping)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}
	}
}
