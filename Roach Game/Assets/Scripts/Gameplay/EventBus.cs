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
    public event EmptyDelegate PlayerDamaged;

    public delegate void RoachDelegate(Roach roach);
    public event RoachDelegate RoachCollected;

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public static EventBus _Instance { get; private set; }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
            return;
        }

        _Instance = this;
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

    // ------------------------------------------------------------------------
    public void InvokeRoachCollected (Roach roach)
    {
        RoachCollected?.Invoke(roach);
    }

    // ------------------------------------------------------------------------
    public void InvokePlayerDamaged ()
    {
        PlayerDamaged?.Invoke();
    }
}