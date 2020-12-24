using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBorn))]
public class EnemyEditor : Editor
{
    EnemyBorn m_Target;

    public override void OnInspectorGUI()
    {
        m_Target = (EnemyBorn)target;

        DrawDefaultInspector();
        DrawStatesInspector();
    }

    //Draw a beautiful and useful custom inspector for our states array
    void DrawStatesInspector()
    {
        GUILayout.Space(5);
        GUILayout.Label("groups", EditorStyles.boldLabel);

        for (int i = 0; i < m_Target.groups.Count; ++i)
        {
            DrawState(i);
        }

        DrawAddStateButton();
    }

    void DrawState(int index)
    {
        if (index < 0 || index >= m_Target.groups.Count)
        {
            return;
        }

        // 在我们的serializedObject中找到States变量
        // serializedObject允许我们方便地访问和修改参数，Unity会提供一系列帮助函数。例如，我们可以通过serializedObject来修改组件值，而不是直接修改，Unity会自动创建Undo和Redo功能
        SerializedProperty listIterator = serializedObject.FindProperty("groups");

        GUILayout.BeginHorizontal();
        {
            // 如果是在实例化的prefab上修改参数，我们可以模仿Unity默认的途径来让修改过的而且未被Apply的值显示成粗体
            if (listIterator.isInstantiatedPrefab == true)
            {
                //The SetBoldDefaultFont functionality is usually hidden from us but we can use some tricks to
                //access the method anyways. See the implementation of our own EditorGUIHelper.SetBoldDefaultFont
                //for more info

                //EditorGUIHelper.SetBoldDefaultFont(listIterator.GetArrayElementAtIndex(index).prefabOverride);
            }

            // BeginChangeCheck()和EndChangeCheck()会检测它们之间的GUI有没有被修改
            EditorGUI.BeginChangeCheck();


            GUILayout.Label("Id", EditorStyles.label, GUILayout.Width(20));
            int id = EditorGUILayout.IntField(m_Target.groups[index].id, GUILayout.Width(40));
            GUILayout.Label("num", EditorStyles.label, GUILayout.Width(30));
            int num = EditorGUILayout.IntField(m_Target.groups[index].num, GUILayout.Width(40));
            GUILayout.Label("refreshTime", EditorStyles.label, GUILayout.Width(80));
            float refreshTime = EditorGUILayout.FloatField(m_Target.groups[index].refreshTime, GUILayout.Width(40));
            


            //Vector3 newPosition = EditorGUILayout.Vector3Field("", m_Target.States[index].Position);

            // 如果修改了的话EndChangeCheck()就会返回true，此时我们就可以进行一些操作例如存储变化的数值
            if (EditorGUI.EndChangeCheck())
            {
                //Create an Undo/Redo step for this modification
                Undo.RecordObject(m_Target, "Modify groups");

                m_Target.groups[index].id = id;
                m_Target.groups[index].num = num;
                m_Target.groups[index].refreshTime = refreshTime;
                //m_Target.groups[index].Position = newPosition;

                // 如果我们直接修改属性，而没有通过serializedObject，那么Unity并不会保存这些数据，Unity只会保存那些标识为dirty的属性
                EditorUtility.SetDirty(m_Target);
            }

            //EditorGUIHelper.SetBoldDefaultFont(false);

            if (GUILayout.Button("Remove"))
            {
                EditorApplication.Beep();

                // 可以很方便的显示一个包含特定按钮的对话框，例如是否同意删除
                if (EditorUtility.DisplayDialog("Really?", "Do you really want to remove the enemy ID : " + m_Target.groups[index].id + "?", "Yes", "No") == true)
                {
                    Undo.RecordObject(m_Target, "Delete groups");
                    m_Target.groups.RemoveAt(index);
                    EditorUtility.SetDirty(m_Target);
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    void DrawAddStateButton()
    {
        var myScript = (EnemyBorn)target;
        if (GUILayout.Button("New"))
        {
            myScript.Add();
        }
    }
}