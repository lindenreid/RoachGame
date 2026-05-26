/*
 * File: ClueData.cs
 * Created: 25/05/2026, 6:08:45 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class ClueData : GameData
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private string _name;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public string _Name => _name;
    public string _FileName => "clue_" + _Name;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void InitFromParseData (string name)
    {
        _name = name;
    }
}