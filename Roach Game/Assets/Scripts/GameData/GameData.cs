/*
 * File: GameData.cs
 * Created: 24/05/2026, 12:41:16 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class GameData : ScriptableObject
{
    [SerializeField] protected int _id;

    public int _ID => _id;
}