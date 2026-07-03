/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI/TypewriterText.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI
 * Created Date: Thursday, July 2nd 2026, 11:07:32 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterText : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _characterRevealSpeedSeconds = 0.01f;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetText(string text)
    {
        StartCoroutine(IncreaseMaxVisibleChar(text));
    }

    // ------------------------------------------------------------------------
    IEnumerator IncreaseMaxVisibleChar(string message)
    {
        _text.text = message;
        _text.maxVisibleCharacters = 0;

        int maxChars = message.Length;
        while (_text.maxVisibleCharacters < maxChars)
        {
            _text.maxVisibleCharacters++;
            yield return new WaitForSeconds(_characterRevealSpeedSeconds);
        }
    }
}
