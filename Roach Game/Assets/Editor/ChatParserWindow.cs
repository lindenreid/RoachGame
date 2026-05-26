/*
 * File: ChatParserWindow.cs
 * Created: 02/08/2024, 3:42:36 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System;
using UnityEngine;
using UnityEditor;

public class ChatParserWindow : EditorWindow {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private static ChatParserSettings m_settings;
    private static readonly string c_settingsFilePath = "Assets/Editor/ChatParserSettings.asset";
    private bool m_importAttempted;
    private bool m_successful;
    [SerializeField]
    private static Vector2 m_scrollPos;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    [MenuItem("Dialogue/Chat Parser", false, 100)]
    static void Init() {
        m_settings = GetSettings();

        ChatParserWindow window =
            (ChatParserWindow)EditorWindow.GetWindow(typeof(ChatParserWindow));
        window.Show();
    }

    // ------------------------------------------------------------------------
    private static ChatParserSettings GetSettings () {
        ChatParserSettings settings =
            AssetDatabase.LoadAssetAtPath<ChatParserSettings>(c_settingsFilePath);
        if (settings == null) {
            Debug.LogError("Please create ChatParserSettings asset at " + c_settingsFilePath);
        }
        return settings;
    }

    // ------------------------------------------------------------------------
    void OnGUI() {
        // settings object is in the Editor folder, so don't attempt to run
        // when we can't access Editor assets
        if(m_settings == null) {
            m_settings = GetSettings();
            if(m_settings == null) {
                EditorGUILayout.LabelField("Please reload window.");
                return;
            }
        }

        EditorGUILayout.LabelField("Chat Parser", EditorStyles.largeLabel);

        EditorGUILayout.LabelField("Chat List Container");
        m_settings.ChatList = (ChatListData)EditorGUILayout.ObjectField(
            obj:m_settings.ChatList, 
            objType:typeof(ChatListData),
            allowSceneObjects:false
        );

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Chat Folders", EditorStyles.boldLabel);
        using(var check = new EditorGUI.ChangeCheckScope()) {
            EditorGUILayout.LabelField("This should be under repoName/Chats.");
            EditorGUILayout.LabelField("Location: " + m_settings.InputFolder);
            if(GUILayout.Button("Select Input Folder")) {
                m_settings.InputFolder = EditorUtility.OpenFolderPanel("Input Folder", Application.dataPath, "");
            }

            if(check.changed) {
                EditorUtility.SetDirty(m_settings);
                AssetDatabase.SaveAssets();
            }
        }
        
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Import", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(
            String.IsNullOrEmpty(m_settings.InputFolder) ||
            m_settings.ChatList == null
        );
        if(GUILayout.Button("Import chats")) {
            Debug.ClearDeveloperConsole();
            
            m_importAttempted = true;
            m_successful = ChatParser.ImportAllChats(
                m_settings.InputFolder,
                AssetDatabase.GetAssetPath(m_settings.ChatList),
                m_settings.GameData
            );
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(20);
        
        if(m_importAttempted) {
            if(m_successful) {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.green;
                EditorGUILayout.LabelField("Import successful!", style);
            } else {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                EditorGUILayout.LabelField("Import unsuccessful.", style);
            }
        }
    }
}