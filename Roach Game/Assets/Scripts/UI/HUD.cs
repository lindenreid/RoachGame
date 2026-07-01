/*
 * File: UiController.cs
 * Created: 28/05/2026, 2:02:19 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject _hud;
    [SerializeField] private TMP_Text _healthText;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        EventBus._Instance.VisitDialogueNode += HandleVisitDialogueNode;
        EventBus._Instance.PlayerDamaged += HandlePlayerDamaged;

        HandlePlayerDamaged();
    }

    // ------------------------------------------------------------------------
    private void HandlePlayerDamaged ()
    {
        _healthText.text = Player._Instance._Health.ToString();
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
        _hud.SetActive(false);
    }
}
