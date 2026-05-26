/*
 * File: ChatListData.cs
 * Created: 24/05/2026, 12:12:23 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Collections.Generic;
using UnityEngine;

// Literally just an SO to contain all chat SOs so that the ChatParser
// can put them all here instead of humans doing it by hand
[CreateAssetMenu(fileName = "ChatList", menuName = "Dialogue/Chat List", order = 1)]
public class ChatListData : ScriptableObject 
{
    public List<ChatData> Chats;
}