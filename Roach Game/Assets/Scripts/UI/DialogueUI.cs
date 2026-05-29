/*
 * File: DialogueUI.cs
 * Created: 25/05/2026, 11:42:23 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Text;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private OptionButton _optionButtonPrefab;
    [SerializeField] private Transform _optionsParent;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        EventBus._Instance.VisitDialogueNode += HandleVisitDialogueNode;
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
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
                sb.Append("<br>");   
            }
        }
        _dialogueText.text = sb.ToString();

        foreach(DialogueOption option in node._Options)
        {
            OptionButton button = Instantiate(_optionButtonPrefab, _optionsParent);
            button.SetupButton(option);
        }
    }
}
