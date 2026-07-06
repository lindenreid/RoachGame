/*
 * File: Roach.cs
 * Created: 26/05/2026, 5:19:10 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Linq;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Splines;

public partial class Roach : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    private enum RoachStateType
    {
        Idle, Running, Dead, Collected, Attacking, Cinematic
    }

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] private Vector2 _idleTimeMinMax;
    [SerializeField] private float _pathKnotDistance = 0.5f;
    [SerializeField] private NavMeshAgent _agent;
    [Header("Splines")]
    [SerializeField] private SplineContainer _movementSplineContainer;
    [SerializeField] private SplineAnimate _movementSplineAnimator;
    [SerializeField] private SplineAnimate _deathSplineAnimator;
    [SerializeField] private Transform _roachSplines;
    [Header("Antennae")]
    [SerializeField] private Transform _leftAntennae;
    [SerializeField] private Transform _rightAntennae;
    [SerializeField] private Vector3 _antennaeAnimMin;
    [SerializeField] private Vector3 _antennaeAnimMax;
    [SerializeField] private float _antennaeFlipTime;
    [Header("Legs")]
    [SerializeField] private Transform[] _legs;
    [SerializeField] private Vector3 _legAnim;
    [SerializeField] private float _legFlipTime;
    [Header("Health")]
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private Collider _collider;
    [Header("Weapons")]
    [SerializeField] private RoachWeapon _gun;
    [SerializeField] private float _weaponUseInterval;
    [Header("Collection")]
    [SerializeField] private GameObject _collectUI;
    [Header("Cinematics")]
    [SerializeField] private PlayableDirector _firstGunTimeline;

    // shared state variables
    private int _health;
    private RoachState _currentState;

    private MeshRenderer[] _renderers;

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

        _collectUI.SetActive(false);

        _renderers = GetComponentsInChildren<MeshRenderer>().ToArray();

        EnterState(RoachStateType.Idle);
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        if(SequenceController._Instance._ActiveStateType != GameStateType.Action)
        {
            return;
        }

        _currentState.RunState(Time.deltaTime);
    }

    // ------------------------------------------------------------------------
    private void OnMouseOver ()
    {
        if(SequenceController._Instance._ActiveStateType != GameStateType.Action)
        {
            return;
        }

        _currentState.OnMouseOver();
    }

    // ------------------------------------------------------------------------
    private void OnMouseExit ()
    {
        _currentState.OnMouseExit();
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void ZoomInToRoach ()
    {
        GameController._Instance.SetTargetRoach(this);
        CameraCinematics._Instance.AnimateRoachZoomIn();
    }

    // ------------------------------------------------------------------------
    public void Hit ()
    {
        if(SequenceController._Instance._ActiveStateType != GameStateType.Action)
        {
            return;
        }
        if(_health <= 0) return;

        EventBus._Instance.InvokeRoachHit(this);

        _health--;

        if(GameController._Instance._TargetRoach == this)
        {
            EnterState(RoachStateType.Cinematic);
        }
        else
        {
            if(_health <= 0)
            {
                EnterState(RoachStateType.Dead);
            }
            else
            {
                EnterState(RoachStateType.Attacking);
            }
        }
    }

    // ------------------------------------------------------------------------
    // also a Timeline signal callback- do not rename
    public void ShowGun ()
    {
        _gun.gameObject.SetActive(true);
        _gun.PointAtPlayer();
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void FinishFirstGunCinematic ()
    {
        EnterState(RoachStateType.Attacking);
    }

    // ------------------------------------------------------------------------
    public void KilledPlayer ()
    {
        EnterState(RoachStateType.Running);
    }

    // ------------------------------------------------------------------------
    public void ResetRoach(Vector3 originalPos)
    {
        _health = _maxHealth;

        transform.SetParent(null);
        transform.position = originalPos;

        EnterState(RoachStateType.Idle);
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
            case RoachStateType.Cinematic: _currentState = new RoachCinematicState(); break;
            default: Debug.LogError("unhandled roach state: " + newState); break;
        }
        //Debug.LogFormat("{0} new state: {1}", gameObject.name, _currentState);
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

    // ------------------------------------------------------------------------
    private void OnDrawGizmos ()
    {
        if(_currentState == null) return;

        _currentState.OnDrawGizmos();
    }
}