/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Editor/ReflectionProbeBouncerEditor.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Editor
 * Created Date: Tuesday, August 25th 2026, 8:08:58 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEditor;
using UnityEngine;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(ReflectionProbeBouncer))]
[CanEditMultipleObjects]
public class ReflectionProbeBouncerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update ();

        ReflectionProbeBouncer bouncer = target as ReflectionProbeBouncer;
        EditorGUI.BeginDisabledGroup(bouncer == null);
        if(GUILayout.Button("Bake"))
        {
            if(bouncer != null)
            {
                bouncer.BakeProbes();
            }
        }
        EditorGUI.EndDisabledGroup();

        base.OnInspectorGUI();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();
    }
}