/*
 * File: Roach.cs
 * Created: 26/05/2026, 5:19:10 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class Roach : NPC
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    private enum RoachState
    {
        Idle, Running
    }

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private float _runSpeed = 0.01f;
    [SerializeField] private Vector2 _idleTimeMinMax;
    [SerializeField] private SplineAnimate _splineAnimator;
    [SerializeField] private Transform _roachSplines;

    private RoachState _currentState;
    private float _stateTime;

    // idle state
    private float _maxStateTime;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        _roachSplines.SetParent(null);
        EnterRunningState();
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        switch(_currentState)
        {
            case RoachState.Idle: DoIdleState(); break;
            case RoachState.Running: DoRunningState(); break;
        }
    }

    // ------------------------------------------------------------------------
    private void EnterIdleState ()
    {
        _currentState = RoachState.Idle;

        _stateTime = 0;
        _maxStateTime = Random.Range(_idleTimeMinMax.x, _idleTimeMinMax.y);
    }

    // ------------------------------------------------------------------------
    private void EnterRunningState ()
    {
        _currentState = RoachState.Running;

        transform.Rotate(0, Random.Range(0, 350), 0);
        _roachSplines.Rotate(0, Random.Range(0, 350), 0);
        _roachSplines.position = transform.position;

        _splineAnimator.Restart(true);
    }

    // ------------------------------------------------------------------------
    private void DoIdleState ()
    {
        _stateTime += Time.deltaTime;
        if(_stateTime >= _maxStateTime)
        {
            EnterRunningState();
        }
    }

    // ------------------------------------------------------------------------
    private void DoRunningState ()
    {
        if(!_splineAnimator.IsPlaying)
        {
            EnterIdleState();
        }
    }
}