using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))]
[CanEditMultipleObjects]
public class AbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {

        /*base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        */
        
        GUILayout.Label("Base Unit Stats");
        Unit unit = (Unit)target;
        SerializedProperty Health = serializedObject.FindProperty("health");
        SerializedProperty Damage = serializedObject.FindProperty("Damage");
        SerializedProperty AttackRange = serializedObject.FindProperty("AttackRange");
        SerializedProperty MaxMovement = serializedObject.FindProperty("MaxMovement");

        Health.intValue = EditorGUILayout.IntField("Health", unit.health);
        Damage.intValue = EditorGUILayout.IntField("Damage", unit.Damage);
        AttackRange.intValue = EditorGUILayout.IntField("AttackRange", unit.AttackRange);
        MaxMovement.intValue = EditorGUILayout.IntField("Movement", unit.MaxMovement);
        EditorGUI.EndChangeCheck();

        serializedObject.ApplyModifiedProperties();
        
    }
}
