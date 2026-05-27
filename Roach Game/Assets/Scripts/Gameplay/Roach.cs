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
    [SerializeField] private Transform _leftAntennae;
    [SerializeField] private Transform _rightAntennae;
    [SerializeField] private Vector3 _antennaeAnimMin;
    [SerializeField] private Vector3 _antennaeAnimMax;
    [SerializeField] private float _antennaeFlipTime;

    private RoachState _currentState;
    private float _stateTime;

    // idle state
    private float _maxStateTime;
    private float _antennaeAnimTime;
    private Vector3 _leftRot;
    private Vector3 _rightRot;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        _roachSplines.SetParent(null);
        EnterIdleState();
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
        _leftRot = Vector3.Lerp(_antennaeAnimMin, _antennaeAnimMax, Random.Range(0.0f, 1.0f));
        _rightRot = Vector3.Lerp(_antennaeAnimMin, _antennaeAnimMax, Random.Range(0.0f, 1.0f));
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
        _antennaeAnimTime += Time.deltaTime;
        if(_antennaeAnimTime >= _antennaeFlipTime)
        {
            _antennaeAnimTime = 0;
            _leftRot = new Vector3(-_leftRot.x, _leftRot.y, _leftRot.z);
            _rightRot = new Vector3(-_rightRot.x, _rightRot.y, _rightRot.z);
        }
        _leftAntennae.Rotate(_leftRot * Time.deltaTime);
        _rightAntennae.Rotate(_rightRot * Time.deltaTime);

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