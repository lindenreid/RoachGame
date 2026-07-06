/*
 * File: DialogueUI.cs
 * Created: 25/05/2026, 11:42:23 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TypewriterText _dialogueText;
    [SerializeField] private OptionButton _optionButtonPrefab;
    [SerializeField] private Transform _optionsParent;
    [SerializeField] private Button _continueButton;

    private DialogueNode _currentNode;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        EventBus._Instance.VisitDialogueNode += HandleVisitDialogueNode;
        EventBus._Instance.TyperwriterFinished += HandleTypewriterFinished;
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
        _currentNode = node;

        _dialogueWindow.SetActive(true);
        foreach(Transform t in _optionsParent)
        {
            Destroy(t.gameObject);
        }

        _speakerText.text = node._Speaker._Name;

        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < node._Lines.Length; i++)
        {
            sb.Append(node._Lines[i]);
            if(i < node._Lines.Length - 1)
            {
                sb.Append("<br><br>");   
            }
        }
        _dialogueText.SetText(sb.ToString());
    }

    // ------------------------------------------------------------------------
    private void HandleTypewriterFinished ()
    {
        Assert.IsNotNull(_currentNode);

        if(_currentNode._Options == null || _currentNode._Options.Length == 0)
        {
            Button continueButton = Instantiate(_continueButton, _optionsParent);
            continueButton.onClick.AddListener(delegate{ExitDialogue();});
        }
        else
        {
            foreach(DialogueOption option in _currentNode._Options)
            {
                OptionButton optionButton = Instantiate(_optionButtonPrefab, _optionsParent);
                optionButton.SetupButton(option);
            }
        }
    }

    // ------------------------------------------------------------------------
    private void ExitDialogue ()
    {
        // MUST end sequence before unlocking clues
        // since clue unlocks usually lead to a new sequence
        SequenceController._Instance.EndCurrentSequence();

        foreach(ClueData clue in _currentNode._CluesGiven)
        {
            EventBus._Instance.InvokeClueUnlocked(clue);
        }

        _dialogueWindow.SetActive(false);
    }
}
