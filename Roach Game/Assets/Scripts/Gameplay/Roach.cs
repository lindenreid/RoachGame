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
        Idle, Running, Dead, Collected
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

    private int _health;

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
        _health = _maxHealth;
        _roachSplines.SetParent(null);
        EnterIdleState();
    }

    // ------------------------------------------------------------------------
    protected override void OnMouseDown()
    {
        if(_currentState == RoachState.Dead)
        {
            EnterCollectedState();
        }
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
    public void Hit ()
    {
        EventBus.Instance.InvokeRoachHit();

        _health--;
        if(_health <= 0)
        {
            EnterDeadState();
        }
    }

    // ------------------------------------------------------------------------
    private void EnterDeadState ()
    {
        if(_currentState == RoachState.Dead)
        {
            return;
        }

        _currentState = RoachState.Dead;

        _roachSplines.position = transform.position;
        _deathSplineAnimator.Play();
    }

    // ------------------------------------------------------------------------
    private void EnterCollectedState ()
    {
        _currentState = RoachState.Collected;

        _movementSplineAnimator.enabled = false;
        _deathSplineAnimator.enabled = false;

        _collider.enabled = false;
        EventBus.Instance.InvokeRoachCollected(this);
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

        _movementSplineAnimator.Restart(true);
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
        if(!_movementSplineAnimator.IsPlaying)
        {
            EnterIdleState();
        }
    }
}