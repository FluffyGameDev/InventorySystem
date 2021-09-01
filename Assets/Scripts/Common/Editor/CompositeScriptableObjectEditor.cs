using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DataComponentList))]
public class DataComponentListDrawerUIE : PropertyDrawer
{
    private bool m_IsListOpen = false;
    private string[] m_ObjectTypeNames;
    private List<Type> m_ObjectTypes;
    private int m_SelectedTypeIndex = 0;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property);

        if (m_IsListOpen)
        {
            height += 24.0f;

            SerializedProperty listProperty = property.FindPropertyRelative("m_DataComponentsList");
            for (int i = 0; i < listProperty.arraySize; ++i)
            {
                height += EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i));
            }
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        m_IsListOpen = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 18.0f), m_IsListOpen, label);
        if (m_IsListOpen)
        {
            position.y += 20.0f;
            position.x += 20.0f;
            position.width -= 20.0f;

            SerializedProperty listProperty = property.FindPropertyRelative("m_DataComponentsList");

            m_ObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
                     .SelectMany(assembly => assembly.GetTypes())
                     .Where(type => type.IsSubclassOf(typeof(DataComponent))).ToList();


            m_ObjectTypeNames = new string[m_ObjectTypes.Count];
            for (int i = 0; i < m_ObjectTypes.Count; ++i)
            {
                m_ObjectTypeNames[i] = m_ObjectTypes[i].Name;
            }

            m_SelectedTypeIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width - 20.0f, 18.0f), m_SelectedTypeIndex, m_ObjectTypeNames);


            if (GUI.Button(new Rect(position.x + position.width - 20.0f, position.y, 20.0f, 18.0f), new GUIContent("+", "Add")))
            {
                CompositeScriptableObject compositeObject = property.serializedObject.targetObject as CompositeScriptableObject;
                if (!compositeObject.HasDataComponent(m_ObjectTypes[m_SelectedTypeIndex]))
                {
                    int targetIndex = listProperty.arraySize;
                    listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
                    SerializedProperty newArrayElement = listProperty.GetArrayElementAtIndex(targetIndex);

                    newArrayElement.managedReferenceValue = Activator.CreateInstance(m_ObjectTypes[m_SelectedTypeIndex]);

                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }

            position.y += 20.0f;

            int idToRemove = -1;
            for (int i = 0; i < listProperty.arraySize; ++i)
            {
                SerializedProperty listEntryProperty = listProperty.GetArrayElementAtIndex(i);

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - 20.0f, position.height), listEntryProperty, new GUIContent(listEntryProperty.managedReferenceFullTypename), true);

                if (GUI.Button(new Rect(position.x + position.width - 20.0f, position.y, 20.0f, 18.0f), new GUIContent("X", "Remove")))
                {
                    idToRemove = i;
                }

                position.y += EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i));
            }

            if (idToRemove >= 0)
            {
                listProperty.DeleteArrayElementAtIndex(idToRemove);
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }

        EditorGUI.EndProperty();
    }
}