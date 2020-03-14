using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Madsbangh.EnumValueMapping.Editor
{
	[CustomPropertyDrawer(typeof(EnumValueMappingBase), true)]
	public class EnumValueMappingDrawer : PropertyDrawer
	{
		private bool foldout;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (foldout)
			{
				var enumValues = Enum.GetValues(GetEnumType(property));
				return (enumValues.Length + 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
			}
			else
			{
				return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect headerPosition = new Rect
			{
				min = position.min,
				xMax = position.xMax,
				yMax = position.yMin + EditorGUIUtility.singleLineHeight
			};
			foldout = EditorGUI.BeginFoldoutHeaderGroup(headerPosition, foldout, new GUIContent(property.name));

			if (foldout)
			{
				EditorGUI.indentLevel++;

				var enumType = GetEnumType(property);
				var names = Enum.GetNames(enumType);

				for (int i = 0; i < names.Length; i++)
				{
					float yMin = position.yMin + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (i + 1);
					var labelPosition = new Rect()
					{
						xMin = position.xMin,
						yMin = yMin,
						xMax = position.xMin + EditorGUIUtility.labelWidth,
						yMax = yMin + EditorGUIUtility.singleLineHeight
					};

					var valuePosition = new Rect()
					{
						xMin = position.xMin + EditorGUIUtility.labelWidth,
						yMin = yMin,
						xMax = position.xMax,
						yMax = yMin + EditorGUIUtility.singleLineHeight
					};

					EditorGUI.PrefixLabel(labelPosition, new GUIContent(names[i]));
					EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("values").GetArrayElementAtIndex(i), new GUIContent());
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.EndFoldoutHeaderGroup();
		}

		private static Type GetEnumType(SerializedProperty property)
		{
			var typeName = property.FindPropertyRelative("enumTypeAssemblyQualifiedName").stringValue;
			return Type.GetType(typeName);
		}
	}
}
