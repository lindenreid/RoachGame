/*
 * File: Friend.cs
 * Created: 24/05/2026, 12:47:14 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

[CreateAssetMenu(fileName = "FriendData", menuName = "Dialogue/Friend", order = 1)]
public class FriendData : GameData
{
    [SerializeField] private string _name;

    public string _Name => _name;
}