/*
 * File: EventBus.cs
 * Created: 25/05/2026, 11:44:29 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class EventBus : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Events
    // ------------------------------------------------------------------------
    public delegate void DialogueNodeDelegate(DialogueNode node);
    public event DialogueNodeDelegate VisitDialogueNode;

    public delegate void EmptyDelegate();
    public event EmptyDelegate RoachHit;

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public static EventBus Instance { get; private set; }

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
    public void InvokeVisitDialogue(DialogueNode node)
    {
        VisitDialogueNode?.Invoke(node);
    }

    // ------------------------------------------------------------------------
    public void InvokeRoachHit ()
    {
        RoachHit?.Invoke();
    }
}