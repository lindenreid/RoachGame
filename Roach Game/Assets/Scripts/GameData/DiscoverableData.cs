/*
 * File: DiscoverableData.cs
 * Created: 25/05/2026, 6:12:40 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class DiscoverableData : GameData
{
    [SerializeField] protected ClueData[] _requiredClues;
    [SerializeField] protected ClueData[] _cluesGiven;

    public ClueData[] _RequiredClues => _requiredClues;
    public ClueData[] _CluesGiven => _cluesGiven;
}