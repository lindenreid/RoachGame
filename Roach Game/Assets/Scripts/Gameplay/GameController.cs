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

    private bool _hitFirstRoach;
    private bool _initialized;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static GameController _Instance { get; private set; }

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
    private void HandleRoachHit ()
    {
        if(!_hitFirstRoach)
        {
            EventBus._Instance.InvokeClueUnlocked(_firstRoachHitClue);
            EventBus._Instance.RoachHit -= HandleRoachHit;
        }
    }
}