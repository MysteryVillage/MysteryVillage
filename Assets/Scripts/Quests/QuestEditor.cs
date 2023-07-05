#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using Quests.Goals;
using UnityEditor;
using UnityEngine;

namespace Quests
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor
    {
        private SerializedProperty m_QuestInfoProperty;
        private SerializedProperty m_QuestFollowUpProperty;

        private List<string> m_QuestGoalType;
        private SerializedProperty m_QuestGoalListProperty;

        [MenuItem("Assets/Quest", priority = 0)]
        public static void CreateQuest()
        {
            var newQuest = CreateInstance<Quest>();
            
            ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
        }

        void OnEnable()
        {
            m_QuestInfoProperty = serializedObject.FindProperty(nameof(Quest.Information));

            m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Quest.goals));

            m_QuestFollowUpProperty = serializedObject.FindProperty(nameof(Quest.followUpQuest));

            var lookup = typeof(QuestGoal);
            m_QuestGoalType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
                .Select(type => type.Name)
                .ToList();
        }

        public override void OnInspectorGUI()
        {
            var child = m_QuestInfoProperty.Copy();
            var depth = child.depth;
            child.NextVisible(true);
            
            EditorGUILayout.LabelField("Quest Info", EditorStyles.boldLabel);
            while (child.depth > depth)
            {
                EditorGUILayout.PropertyField(child, true);
                child.NextVisible(false);
            }
            
            EditorGUILayout.PropertyField(m_QuestFollowUpProperty.Copy(), true);

            int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, m_QuestGoalType.ToArray());

            if (choice != -1)
            {
                var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);
                
                AssetDatabase.AddObjectToAsset(newInstance, target);
                
                m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
                m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize - 1)
                    .objectReferenceValue = newInstance;
            }

            Editor ed = null;
            int toDelete = -1;
            for (int i = 0; i < m_QuestGoalListProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                var item = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
                SerializedObject obj = new SerializedObject(item.objectReferenceValue);
                
                CreateCachedEditor(item.objectReferenceValue, null, ref ed);
                
                ed.OnInspectorGUI();
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("-", GUILayout.Width(32)))
                {
                    toDelete = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (toDelete != -1)
            {
                var item = m_QuestGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
                DestroyImmediate(item, true);
                
                // need to do it twice, first time just nullify the entry, second time actually remove it
                m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
                m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif