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

            hitPointsProperty.intValue = EditorGUILayout.IntField("Hit Points", hitPointsProperty.intValue);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
