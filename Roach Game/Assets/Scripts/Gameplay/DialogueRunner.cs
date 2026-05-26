/*
 * File: DialogueRunner.cs
 * Created: 25/05/2026, 11:43:33 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private DialogueNode _currentNode;

    public static DialogueRunner Instance { get; private set; }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // ------------------------------------------------------------------------
    public void StartDialogue(DialogueNode node)
    {
        _currentNode = node;
        Debug.Log("runner visit node: " + node);
        EventBus.Instance.InvokeVisitDialogue(node);
    }

    // ------------------------------------------------------------------------
    // button callback
    public void SelectOption(DialogueNode nextNode)
    {
        Debug.Log("selected node: " + nextNode);
        EventBus.Instance.InvokeVisitDialogue(nextNode);
    }
}