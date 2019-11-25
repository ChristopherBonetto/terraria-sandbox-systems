#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(UnityEngine.Tilemaps.TLinkedDestructibleTile))]
    public class TLinkedDestructibleTileEditor : TDestructibleTileEditor
    {
        protected override void TileSpecializationInspectorGUI()
        {
            EditorGUILayout.LabelField("Linked destruction", EditorStyles.boldLabel);

            SerializedProperty destructionDirectionProperty = serializedObject.FindProperty("m_DestructionDirections");
            SerializedProperty QuantityUnitsProperty = serializedObject.FindProperty("m_QuantityUnitsPerPickup");

            EditorGUILayout.PropertyField(destructionDirectionProperty, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(QuantityUnitsProperty);
        }
    }
}

#endif
