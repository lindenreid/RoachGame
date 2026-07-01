/*
 * Filename: GameController.cs
 * Created: 06/30/26, 1:20:23 pm
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class GameController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ClueData _gameStartClue;
    [SerializeField] private ClueData _firstRoachHitClue;
    [SerializeField] private ClueData _postHitFirstRoach;

    private bool _hitFirstRoach;
    private bool _initialized;

    private Roach _hitRoach;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static GameController _Instance { get; private set; }
    
    public Roach _HitRoach => _hitRoach;

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
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        if(!_initialized)
        {
            Initialize();
        }
    }

    // ------------------------------------------------------------------------
    private void Initialize()
    {
        _initialized = true;
        EventBus._Instance.InvokeClueUnlocked(_gameStartClue);
    }

    // ------------------------------------------------------------------------
    private void HandleRoachHit (Roach roach)
    {
        if(!_hitFirstRoach)
        {
            _hitRoach = roach;
            EventBus._Instance.InvokeClueUnlocked(_firstRoachHitClue);
            EventBus._Instance.RoachHit -= HandleRoachHit;
        }
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void EndFirstGunCinematic ()
    {
        _hitRoach = null;
        EventBus._Instance.InvokeClueUnlocked(_postHitFirstRoach);
    }
}