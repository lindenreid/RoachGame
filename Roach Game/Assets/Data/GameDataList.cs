/*
 * File: GameDataList.cs
 * Created: 24/05/2026, 5:59:30 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Collections.Generic;
using UnityEngine;

// Literally just an SO to contain all game data SOs so that an editor tool
// can put them all here instead of humans doing it by hand
[CreateAssetMenu(fileName = "GameDataList", menuName = "Dialogue/Game Data List", order = 1)]
public class GameDataList : ScriptableObject
{
    public FriendData[] _Friends;
    public ClueData [] _Clues;

    public Dictionary<string, FriendData> _FriendsByName;
    public Dictionary<string, ClueData> _CluesByName;
}