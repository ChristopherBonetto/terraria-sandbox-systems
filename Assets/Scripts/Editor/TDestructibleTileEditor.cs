#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
    [CustomEditor(typeof(TDestructibleTile))]
    public class TDestructibleTileEditor : RuleTileEditor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty hitPointsProperty = serializedObject.FindProperty("m_HitPoints");
            SerializedProperty containedItemsProperty = serializedObject.FindProperty("m_ContainedItems");
            SerializedProperty groupID = serializedObject.FindProperty("m_GroupID");

            hitPointsProperty.intValue = EditorGUILayout.IntField("Hit Points", hitPointsProperty.intValue);
            EditorGUILayout.PropertyField(containedItemsProperty, true);
            EditorGUILayout.PropertyField(groupID);

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}

#endif
