/*
 * Filename: GameController.cs
 * Created: 06/30/26, 1:20:23 pm
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Handles hard-coded clues for very specific sequence triggers
public class GameController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ClueData _gameStartClue;
    [SerializeField] private ClueData _firstRoachHitClue;
    [SerializeField] private ClueData _postHitFirstRoach;
    [SerializeField] private Sequence _firstRoachGunSequence;

    private bool _hitFirstRoach;
    private Roach _targetRoach;
    private List<Roach> _activeRoaches;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static GameController _Instance { get; private set; }
    
    public Roach _TargetRoach => _targetRoach;
    public int _LivingRoaches => _activeRoaches == null ? 0 : _activeRoaches.Count(r => !r._IsDead);
    public bool _ReadyForHealthDisplay => _hitFirstRoach;

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

        _activeRoaches = new List<Roach>();
    }

    // ------------------------------------------------------------------------
    private void Start ()
    {
        EventBus._Instance.RoachHit += HandleRoachHit;
        EventBus._Instance.ClueUnlocked += HandleClueUnlocked;
        EventBus._Instance.SequenceStarted += HandleSequenceStarted;
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
    private void HandleSequenceStarted(Sequence sequence)
    {
        _activeRoaches.Clear();
        _activeRoaches.AddRange(sequence._Roaches);
    }

    // ------------------------------------------------------------------------
    private void HandleRoachHit (Roach roach)
    {
        if(SequenceController._Instance._ActiveSequence == _firstRoachGunSequence && !_hitFirstRoach)
        {
            //Debug.Log("set target roach: " + roach);
            _hitFirstRoach = true;
            SetTargetRoach(roach);
            EventBus._Instance.InvokeClueUnlocked(_firstRoachHitClue);
        }
        else // don't bother counting dead roaches if we're checking for first roach gun
        {
            int deadRoaches = _activeRoaches.Count(r => r._IsDead);
            //Debug.LogFormat("dead roaches: {0}; total roaches: {1}", deadRoaches, _activeRoaches.Count());
            if(deadRoaches == _activeRoaches.Count())
            {
                SequenceController._Instance.EndCurrentSequence();
            } 
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