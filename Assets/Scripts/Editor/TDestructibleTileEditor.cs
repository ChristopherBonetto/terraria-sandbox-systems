#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(UnityEngine.Tilemaps.TDestructibleTile))]
    public class TDestructibleTileEditor : RuleTileEditor
    {
        public override void OnInspectorGUI()
        {
            DestructibleTileBaseInspectorGUI();

            base.OnInspectorGUI();
        }

        protected void DestructibleTileBaseInspectorGUI()
        {
            SerializedProperty hitPointsProperty = serializedObject.FindProperty("m_HitPoints");
            SerializedProperty containedItemsProperty = serializedObject.FindProperty("m_ContainedItems");
            SerializedProperty groupIDProperty = serializedObject.FindProperty("m_GroupID");

            EditorGUILayout.LabelField("Destructibility", EditorStyles.boldLabel);

            hitPointsProperty.intValue = EditorGUILayout.IntField("Hit Points", hitPointsProperty.intValue);
            EditorGUILayout.PropertyField(containedItemsProperty, true);
            EditorGUILayout.PropertyField(groupIDProperty);

            EditorGUILayout.Space();

            TileSpecializationInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void TileSpecializationInspectorGUI()
        {

        }
    }
}

#endif
