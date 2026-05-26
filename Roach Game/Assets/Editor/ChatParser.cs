/*
 * File: ChatParser.cs
 * Created: 19/08/2021, 4:13:58 PM
 * Author: Linden Reid
 * Copyright 2019 - 2025 Studio Tilia
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

// Given list of writer-made script documents,
// outputs chat scriptable objects
public static class ChatParser {
    // ------------------------------------------------------------------------
    // Constants
    // ------------------------------------------------------------------------
    private static readonly string c_chatOutputPath = "Assets/Data/Chats";
    private static readonly string c_clueOutputPath = "Assets/Data/Clues";

    // json value names
    private static readonly string c_ifid_Name = "ifid";
    private static readonly string c_dialogueNodeLine_Name = "text";
    private static readonly string c_dialogueOptionLine_Name = "name";
    private static readonly string c_dialogueNodeId_Name = "pid";

    // message line separators
    private static readonly char c_escapeChar = '\n';
    private static readonly string c_line_Label = "<line>";
    private static readonly string c_clue_Label = "<clue>";

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static bool ImportAllChats(
        string inputPath,
        string chatListPath,
        GameDataList gameData
    )
    {
        // make sure game data is up-to-date
        bool foundGameData = GameDataWindow.ImportGameData();
        if(!foundGameData) return false;

        // parse all chat input files
        string[] files = Directory.GetFiles(inputPath);
        List<ChatData> chats = new List<ChatData>();

        bool successful = true;
        foreach(string file in files)
        {
            successful = successful && ParseChat(file, chats, gameData);
        }

        if(successful)
        {
            // release files held interally by Unity
            AssetDatabase.ReleaseCachedFileHandles();

            // save all new files
            foreach(ChatData chat in chats) {
                SaveChatAssets(chat);
            }
            SaveClueAssets(gameData);
            AssetDatabase.SaveAssets();

            // find saved assets and load into chat list
            UpdateChatDataListAndSaveAssets(chatListPath);
            GameDataWindow.ImportGameData();
        }

        return successful;
    }

    // ------------------------------------------------------------------------
    // return: whether or not the importing was successful (validation passed)
    public static bool ParseChat(
        string file,
        List<ChatData> chats,
        GameDataList gameData
    )
    {
        // create default parse data
        ChatParseData chatParseData = new ChatParseData();
        chatParseData._id = UnityEngine.Random.Range(0, 1000);

        List<DialogueNodeParseData> nodeParseDatas = new List<DialogueNodeParseData>();

        // open and parse file
        StreamReader streamReader = new StreamReader(file);
        JsonTextReader jsonReader = new JsonTextReader(streamReader);
        string ifid = "missing";
        while (jsonReader.Read())
        {
            //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);

            if (jsonReader.Value != null)
            {
                // chat IFID
                if(jsonReader.Value.Equals(c_ifid_Name))
                {
                    jsonReader.Read();
                    if (jsonReader.Value != null)
                    {
                        ifid = (string)jsonReader.Value;
                        //Debug.Log("read chat ifid: " + ifid);
                    }
                }

                // new node
                if(jsonReader.Value.Equals(c_dialogueNodeLine_Name))
                {
                    // create default node parse data
                    DialogueNodeParseData nodeParseData = new DialogueNodeParseData();

                    List<DialogueOptionParseData> optionsParseData = new List<DialogueOptionParseData>();
                    
                    // parse node lines
                    jsonReader.Read();
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    nodeParseData = ParseDialogueLineContent(nodeParseData, (string)jsonReader.Value);

                    jsonReader.Read();
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    
                    // if next line is "links", read all convo options
                    if(((string)jsonReader.Value).Equals("links"))
                    {
                        // read all convo options
                        while(jsonReader.Read())
                        {
                            //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                            DialogueOptionParseData option = new DialogueOptionParseData();

                            // end convo options when we reach the end of the array
                            if(jsonReader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }

                            // read content of 1 convo option
                            while(jsonReader.Read())
                            {
                                //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);

                                if(jsonReader.Value != null)
                                {
                                    if(jsonReader.Value.Equals(c_dialogueOptionLine_Name))
                                    {
                                        jsonReader.Read();
                                        //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                                        option._line = (string)jsonReader.Value;
                                        //Debug.Log("set option line to: " + option._line);
                                    }
                                    else if(jsonReader.Value.Equals(c_dialogueNodeId_Name))
                                    {
                                        jsonReader.Read();
                                        //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                                        option._nextNodeId = (string)jsonReader.Value;
                                    }
                                }

                                if(jsonReader.TokenType == JsonToken.EndObject)
                                {
                                    optionsParseData.Add(option);
                                    //Debug.LogFormat("added option: {0}, {1}", option._line, option._nextNodeId);
                                    break;
                                }
                            }
                        }
                        jsonReader.Read();
                    }

                    jsonReader.Read(); // skip name value
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip "pid"
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);

                    // read ID
                    jsonReader.Read();
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    nodeParseData._nodeId = (string)jsonReader.Value;
                    //Debug.Log("set _nodeID to " + nodeParseData._nodeId);

                    jsonReader.Read(); // skip position
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip {
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip x
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip val
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip y
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip val
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip }
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip tags
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                    jsonReader.Read(); // skip [
                    //Debug.LogFormat("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);

                    // read first tag - used for speaker
                    jsonReader.Read();
                    nodeParseData._friend = (string)jsonReader.Value;
                    //Debug.LogFormat("set friend to {0} from {1}", nodeParseData._friend, (string)jsonReader.Value);

                    // save new dialogue node parse data to list
                    nodeParseData._options = optionsParseData.ToArray();
                    nodeParseDatas.Add(nodeParseData);
                }
            }
        }
        streamReader.Close();
        jsonReader.Close();

        // now that all dialogue nodes have been created, we can initialize
        // the nodes's conversation options to point to the actual node obj
        // (instead of just an ID)
        Dictionary<int, DialogueNode> nodesById = new Dictionary<int, DialogueNode>();
        foreach(DialogueNodeParseData nodeParseData in nodeParseDatas)
        {
            DialogueNode dialogueNode = ScriptableObject.CreateInstance<DialogueNode>();
            bool success = dialogueNode.LoadParseDataAndCreateId(nodeParseData, ifid);
            if(!success) return false;
            nodesById.Add(dialogueNode._ID, dialogueNode);
        }

        foreach(DialogueNode node in nodesById.Values)
        {
            node.InitWithGameData(ifid, nodesById, gameData._FriendsByName, gameData._CluesByName);
        }

        // create chat scriptable object & load metadata AND nodes
        ChatData chat = ScriptableObject.CreateInstance<ChatData>(); 
        chat.InitFromParseData(chatParseData, nodesById);

        // check if chat with same ID already exists-
        // if so, update that one instead of replacing
        ChatData originalChat = chats.FirstOrDefault(c => c._ID == chat._ID);
        if(originalChat != null)
        {
            originalChat.UpdateWithNewDialogue(nodesById.Values.ToArray());
            chat = originalChat;
        }
        else
        {
            chats.Add(chat);   
        }

        return true;
    }

    // ------------------------------------------------------------------------
    private static DialogueNodeParseData ParseDialogueLineContent (
        DialogueNodeParseData nodeParseData,
        string parserText
    ) {
        char[] seperators = new char[1]{c_escapeChar};
        string[] splitText = parserText.Split(
            seperators,
            StringSplitOptions.RemoveEmptyEntries
        );

        List<string> dialogueLines = new List<string>();
        List<string> clues = new List<string>();

        foreach(string text in splitText)
        {
            if(text.StartsWith(c_line_Label))
            {
                dialogueLines.Add(text.Substring(
                    text.IndexOf(c_line_Label) + c_line_Label.Length + 1
                ));
            }
            else if(text.StartsWith(c_clue_Label))
            {
                clues.Add(text.Substring(
                    text.IndexOf(c_clue_Label) + c_clue_Label.Length + 1
                ));
            }
        }

        nodeParseData._lines = dialogueLines.ToArray();
        nodeParseData._clues = clues.ToArray();

        return nodeParseData;
    }

    // ------------------------------------------------------------------------
    private static void SaveChatAssets (ChatData chat)
    {
        DialogueNode[] generatedNodeFiles =
            new DialogueNode[chat._Nodes.Length];

        string chatDir = c_chatOutputPath + "/" + chat.FileName;
        if(!System.IO.Directory.Exists(chatDir))
        {
            System.IO.Directory.CreateDirectory(chatDir);
        }

        if(!System.IO.Directory.Exists(c_clueOutputPath))
        {
            System.IO.Directory.CreateDirectory(c_clueOutputPath);
        }

        int i = 0;
        foreach(DialogueNode node in chat._Nodes)
        {
            // find OR create new permanent assets for all DialogueNodes
            string path = chatDir + "/message_" + chat._ID + "_" + node._ID + ".asset";
            DialogueNode loadedNodeFile = (DialogueNode)AssetDatabase.LoadAssetAtPath<DialogueNode>(path); 
            if(loadedNodeFile == null)
            {
                AssetDatabase.CreateAsset(node, path);
                loadedNodeFile = (DialogueNode)AssetDatabase.LoadAssetAtPath<DialogueNode>(path);    
            } 
            else
            {
                loadedNodeFile.CopyFrom(node);
            }
            generatedNodeFiles[i] = loadedNodeFile;
            i++;

            // also, for this dialogue node, find OR create new permanent assets for all clues
            int j = 0;
            ClueData[] loadedClueFiles = new ClueData[loadedNodeFile._RequiredClues.Length];
            foreach(ClueData clue in loadedNodeFile._RequiredClues)
            {
                path = GetClueFilePath(clue);
                ClueData loadedClueFile = (ClueData)AssetDatabase.LoadAssetAtPath<ClueData>(path); 
                if(loadedClueFile == null)
                {
                    AssetDatabase.CreateAsset(clue, path);
                    loadedClueFile = (ClueData)AssetDatabase.LoadAssetAtPath<ClueData>(path);    
                } 
                loadedClueFiles[j] = loadedClueFile;
                j++;
            }
            loadedNodeFile.LoadClueDataAssets(loadedClueFiles);
        }
        AssetDatabase.SaveAssets();

        string chatPath = chatDir + "/chat_" + chat.FileName + ".asset"; 
        ChatData loadedChatFile = (ChatData)AssetDatabase.LoadAssetAtPath<ChatData>(chatPath);
        if(loadedChatFile == null)
        {
            AssetDatabase.CreateAsset(chat, chatPath);
            loadedChatFile = (ChatData)AssetDatabase.LoadAssetAtPath<ChatData>(chatPath);
        }
        
        // finalize chat's list of messages in case asset references changed while loading
        loadedChatFile.FinalizeDialogueNodeReferences(generatedNodeFiles);

        EditorUtility.SetDirty(loadedChatFile);
    }

    // ------------------------------------------------------------------------
    private static string GetClueFilePath(ClueData clue)
    {
        return c_clueOutputPath + "/" + clue._FileName + ".asset";
    }

    // ------------------------------------------------------------------------
    private static void SaveClueAssets (GameDataList gameData)
    {
        foreach(ClueData clue in gameData._CluesByName.Values)
        {
            string path = GetClueFilePath(clue);
            ClueData loadedClueFile = (ClueData)AssetDatabase.LoadAssetAtPath<ClueData>(path);
            EditorUtility.SetDirty(loadedClueFile);
        }
    }

    // ------------------------------------------------------------------------
    private static void UpdateChatDataListAndSaveAssets (string chatListPath)
    {
        ChatListData chatList =
            (ChatListData)AssetDatabase.LoadAssetAtPath<ChatListData>(chatListPath);
        ChatData[] chatObjs = EditorWindowUtils.FindScriptableObjectData<ChatData>();

        foreach(ChatData chatObj in chatObjs)
        {
            foreach(DialogueNode messageObj in chatObj._Nodes)
            {
                DialogueNode loadedMessageObj =
                    (DialogueNode)AssetDatabase.LoadAssetAtPath<DialogueNode>(
                        AssetDatabase.GetAssetPath(messageObj)
                    );
                loadedMessageObj._Chat = chatObj;
                EditorUtility.SetDirty(loadedMessageObj);
            }
        }

        chatList.Chats = new List<ChatData>(chatObjs);
        
        EditorUtility.SetDirty(chatList);
        AssetDatabase.SaveAssets();
    }

}