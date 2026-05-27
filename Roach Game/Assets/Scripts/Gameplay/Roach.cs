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
    [SerializeField] private SplineContainer _runningSplines;

    private RoachState _currentState;
    private float _stateTime;

    // idle state
    private float _maxStateTime;

    // running state
    private Spline _currentSpline;
    private Vector3 _startPos;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
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

        _currentSpline = _runningSplines.Splines[
            Random.Range(0, _runningSplines.Splines.Count-1)
        ];
        _stateTime = 0;
        _startPos = transform.position;
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
        _stateTime += Time.deltaTime * _runSpeed;

        transform.position = _startPos + (Vector3)SplineUtility.EvaluatePosition(_currentSpline, _stateTime);

        if(_stateTime >= 1)
        {
            EnterIdleState();
        }
    }
}