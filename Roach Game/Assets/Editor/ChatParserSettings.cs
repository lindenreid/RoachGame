/*
 * File: ChatParserSettings.cs
 * Created: 02/08/2024, 3:42:36 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatParserSettings", menuName = "Dialogue/Chat Parser Settings", order = 1)]
public class ChatParserSettings : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public ChatListData ChatList;
    public GameDataList GameData;
    public string InputFolder = "";
}