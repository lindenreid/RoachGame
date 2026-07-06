/*
 * File: DialogueNodeParseData.cs
 * Created: 24/05/2026, 12:40:01 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

public struct DialogueOptionParseData
{
    public string _line;
    public string _nextNodeId;
}

public struct DialogueNodeParseData
{
    public string _nodeId; // id within twine file
    public string[] _lines;
    public string[] _clues;
    public string[] _cluesGiven;
    public string _friend;
    public DialogueOptionParseData[] _options;
}