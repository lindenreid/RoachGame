/*
 * File: Sequence.cs
 * Created: 06/06/2026, 2:47:17 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class Sequence : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ClueData _triggerClue;
    [SerializeField] private GameObject _objects;
    [SerializeField] private GameStateType _gameStateType;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public ClueData _TriggerClue => _triggerClue;
    public GameStateType _GameStateType => _gameStateType;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void StartSequence ()
    {
        _objects.SetActive(true);
    }
}
