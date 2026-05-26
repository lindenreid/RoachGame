/*
 * File: ChatData.cs
 * Created: 24/05/2026, 12:13:38 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatData", menuName = "Dialogue/Chat", order = 1)]
public class ChatData : GameData
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private DialogueNode[] _nodes;
    [SerializeField] private FriendData[] _friends;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public DialogueNode[] _Nodes => _nodes;

    public string FileName
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach(FriendData friend in _friends)
            {
                sb.Append(friend._Name);
            }
            return sb.ToString();
        }
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void InitFromParseData(
        ChatParseData parseData,
        Dictionary<int, DialogueNode> nodesById
    )
    {
        _id = parseData._id;
        _nodes = nodesById.Values.ToArray();

        // create friend list from all unique dialogue speakers 
        HashSet<FriendData> chatFriends = new HashSet<FriendData>();
        foreach(DialogueNode node in _nodes)
        {
            if(!chatFriends.Contains(node._Speaker))
            {
                chatFriends.Add(node._Speaker);
            }
        }
        _friends = chatFriends.ToArray();

        // create unique ID from list of friends
        CreateIdFromFriendList();

        Debug.LogFormat(
            "made chat {0}; num dialogue nodes: {1}",
            _id,
            _nodes.Length
        );
    }

    // ------------------------------------------------------------------------
    public void UpdateWithNewDialogue(DialogueNode[] newNodes)
    {
        List<DialogueNode> nodesToAdd = new List<DialogueNode>();

        // go through all of the new messages
        // if we find an existing message with the same node,
        //      update it with the new message data
        // otherwise, add it to the list of new messages to append to message list
        foreach(DialogueNode newNode in newNodes) {
            DialogueNode matchingNode = _nodes.FirstOrDefault(m => m._ID == newNode._ID);
            if(matchingNode == null) {
                nodesToAdd.Add(newNode);
            } else {
                matchingNode.CopyFrom(newNode);
            }
        }

        List<DialogueNode> allNodes = new List<DialogueNode>(_nodes);
        allNodes.AddRange(nodesToAdd);
        _nodes = allNodes.ToArray();
    }

    // ------------------------------------------------------------------------
    public void FinalizeDialogueNodeReferences(DialogueNode[] nodes)
    {
        _nodes = nodes;
    }

    // ------------------------------------------------------------------------
    public void CreateIdFromFriendList()
    {
        _friends = _friends.OrderBy(f => f.ToString()).ToArray();

        StringBuilder sb = new StringBuilder("");
        for(int i = 0; i < _friends.Length; i++)
        {
            sb.Append(_friends[i].ToString());
            if(i != _friends.Length - 1) {
                sb.Append(", ");
            }
        }

        int id = 0;
        string names = sb.ToString();
        for(int i = 0; i < names.Length; i++)
        {
            id += (int)names[i];
        }

        _id = id;
    }
}