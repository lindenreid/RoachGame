/*
 * File: GameDataWindow.cs
 * Created: 24/05/2026, 6:02:38 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;
using UnityEditor;

public class GameDataWindow : EditorWindow
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private static ChatParserSettings _settings;
    private static readonly string c_settingsFilePath = "Assets/Editor/ChatParserSettings.asset";
    private bool _gameDataImportAttempted;
    private bool _importSuccess;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    [MenuItem("Dialogue/Game Data", false, 100)]
    static void Init() {
        LoadSettings();

        GameDataWindow window =
            (GameDataWindow)EditorWindow.GetWindow(typeof(GameDataWindow));
        window.Show();
    }

    // ------------------------------------------------------------------------
    private static void LoadSettings ()
    {
        _settings =
            AssetDatabase.LoadAssetAtPath<ChatParserSettings>(c_settingsFilePath);
        if(_settings == null)
        {
            Debug.LogError("Game Data Window settings asset not found.");
        }
    }

    // ------------------------------------------------------------------------
    void OnGUI()
    {
        if(_settings == null)
        {
            EditorGUILayout.LabelField("Please refresh window.");
            return;
        }

        EditorGUILayout.LabelField("Game Data", EditorStyles.largeLabel);

        EditorGUILayout.LabelField("Game Data List Container");
        _settings.GameData = (GameDataList)EditorGUILayout.ObjectField(
            obj:_settings.GameData, 
            objType:typeof(GameDataList),
            allowSceneObjects:false
        );

        GUILayout.Space(20);

        if(GUILayout.Button("Import game data")) {
            _gameDataImportAttempted = true;
            _importSuccess = ImportGameData();
        }

        if(_gameDataImportAttempted) {
            GUILayout.Space(20);
            EditorGUILayout.LabelField(
                _importSuccess ?
                    "Data imported." :
                    "Data import failure. Check console for details."
            );
        }
    }

    // ------------------------------------------------------------------------
    public static bool ImportGameData()
    {
        if(_settings == null)
        {
            LoadSettings();
            if(_settings == null)
            {
                return false;
            }
        }

        GameDataList gameDataList = _settings.GameData;
        gameDataList._Friends = EditorWindowUtils.FindScriptableObjectData<FriendData>();
        gameDataList._Clues = EditorWindowUtils.FindScriptableObjectData<ClueData>();

        gameDataList._FriendsByName = new System.Collections.Generic.Dictionary<string, FriendData>();
        foreach(FriendData friend in gameDataList._Friends)
        {
            gameDataList._FriendsByName.Add(friend._Name, friend);
            //Debug.LogFormat("added kvp {0}, {1}", friend._Name, friend);
        }

        gameDataList._CluesByName = new System.Collections.Generic.Dictionary<string, ClueData>();
        foreach(ClueData clue in gameDataList._Clues)
        {
            gameDataList._CluesByName.Add(clue._Name, clue);
            //Debug.LogFormat("added kvp {0}, {1}", clue._Name, clue);
        }

        return true;
    }
}