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
    [SerializeField] private GameObject _talkUI;

    // ------------------------------------------------------------------------
    private void OnMouseOver ()
    {
        _talkUI.SetActive(true);

        if(Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    // ------------------------------------------------------------------------
    private void StartDialogue()
    {
        _talkUI.SetActive(false);
        
        DialogueRunner.Instance.StartDialogue(_dialogueStart);
    }

    // ------------------------------------------------------------------------
    private void OnMouseExit ()
    {
        _talkUI.SetActive(false);
    }
}
