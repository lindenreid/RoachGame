/*
 * Filename: GameController.cs
 * Created: 06/30/26, 1:20:23 pm
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

// Handles hard-coded clues for very specific sequence triggers
public class GameController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ClueData _gameStartClue;
    [SerializeField] private ClueData _firstRoachHitClue;
    [SerializeField] private ClueData _postHitFirstRoach;

    private bool _hitFirstRoach;
    private Roach _targetRoach;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static GameController _Instance { get; private set; }
    
    public Roach _TargetRoach => _targetRoach;

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
    private void Start ()
    {
        EventBus._Instance.RoachHit += HandleRoachHit;
        EventBus._Instance.ClueUnlocked += HandleClueUnlocked;
    }

    // ------------------------------------------------------------------------
    // button callback
    public void StartGame ()
    {
        EventBus._Instance.InvokeClueUnlocked(_gameStartClue);
    }

    // ------------------------------------------------------------------------
    public void SetTargetRoach (Roach roach)
    {
        _targetRoach = roach;
    }

    // ------------------------------------------------------------------------
    private void HandleRoachHit (Roach roach)
    {
        if(!_hitFirstRoach)
        {
            SetTargetRoach(roach);
            EventBus._Instance.InvokeClueUnlocked(_firstRoachHitClue);
            EventBus._Instance.RoachHit -= HandleRoachHit;
        }
    }

    // ------------------------------------------------------------------------
    private void HandleClueUnlocked (ClueData clue)
    {
        if(clue == _postHitFirstRoach)
        {
            _targetRoach = null;
        }
    }
}