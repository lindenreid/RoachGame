/*
 * File: Roach.cs
 * Created: 26/05/2026, 5:19:10 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public partial class Roach : NPC
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    private enum RoachStateType
    {
        Idle, Running, Dead, Collected, Attacking
    }

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private float _runSpeed = 0.01f;
    [SerializeField] private Vector2 _idleTimeMinMax;
    [SerializeField] private SplineAnimate _movementSplineAnimator;
    [SerializeField] private SplineAnimate _deathSplineAnimator;
    [SerializeField] private Transform _roachSplines;
    [SerializeField] private Transform _leftAntennae;
    [SerializeField] private Transform _rightAntennae;
    [SerializeField] private Vector3 _antennaeAnimMin;
    [SerializeField] private Vector3 _antennaeAnimMax;
    [SerializeField] private float _antennaeFlipTime;
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private Collider _collider;

    // shared state variables
    private int _health;
    private RoachState _currentState;
    private float _stateTime;

    // antennae rotation
    private Vector3 _leftAntennaeNeutralPos;
    private Quaternion _leftAntennaeNeutralRot;
    private Vector3 _rightAntennaeNeutralPos;
    private Quaternion _rightAntennaeNeutralRot;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        _health = _maxHealth;

        _roachSplines.SetParent(null);

        _leftAntennaeNeutralPos = _leftAntennae.localPosition;
        _leftAntennaeNeutralRot = _leftAntennae.localRotation;

        _rightAntennaeNeutralPos = _rightAntennae.localPosition;
        _rightAntennaeNeutralRot = _rightAntennae.localRotation;

        EnterState(RoachStateType.Idle);
    }

    // ------------------------------------------------------------------------
    protected override void OnMouseDown()
    {
        if(_currentState is RoachDeadState)
        {
            EnterState(RoachStateType.Collected);
        }
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        _currentState.RunState(Time.deltaTime);
    }

    // ------------------------------------------------------------------------
    public void Hit ()
    {
        EventBus.Instance.InvokeRoachHit();

        _health--;
        if(_health <= 0)
        {
            EnterState(RoachStateType.Dead);
        }
    }

    // ------------------------------------------------------------------------
    private void EnterState(RoachStateType newState)
    {
        _currentState?.ExitState();

        switch(newState)
        {
            case RoachStateType.Idle: _currentState = new RoachIdleState(); break;
            case RoachStateType.Running: _currentState = new RoachRunningState(); break;
            case RoachStateType.Attacking: _currentState = new RoachAttackingState(); break;
            case RoachStateType.Dead: _currentState = new RoachDeadState(); break;
            case RoachStateType.Collected: _currentState = new RoachCollectedState(); break;
            default: Debug.LogError("unhandled roach state: " + newState); break;
        }
        _currentState.EnterState(this);
    }

    // ------------------------------------------------------------------------
    private void ResetAntennae ()
    {
        _leftAntennae.localRotation = _leftAntennaeNeutralRot;
        _leftAntennae.localPosition = _leftAntennaeNeutralPos;

        _rightAntennae.localRotation = _rightAntennaeNeutralRot;
        _rightAntennae.localPosition = _rightAntennaeNeutralPos;
    }
}