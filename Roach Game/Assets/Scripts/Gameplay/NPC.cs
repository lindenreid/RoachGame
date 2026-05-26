/*
 * File: NPC.cs
 * Created: 25/05/2026, 11:11:57 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class NPC : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private DialogueNode _dialogueStart;

    // ------------------------------------------------------------------------
    private void OnMouseDown()
    {
        DialogueRunner.Instance.StartDialogue(_dialogueStart);
    }
}
