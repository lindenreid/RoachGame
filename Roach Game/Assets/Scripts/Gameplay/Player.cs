/*
 * File: Player.cs
 * Created: 25/05/2026, 11:14:18 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    private enum PlayerState
    {
        Explore, Dialogue
    }

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private SplineAnimate _splineAnimator;
    [SerializeField] private Transform _aimReticle;
    [SerializeField] private float _maxAimDistance;

    private CharacterController _characterController;
    private Transform _cameraTrans;
    private float _rotationX;
    private float _rotationY;

    private PlayerState _state;
    private bool _needsAnimRestart;
    private Vector3 _reticleOffset = new Vector3(0, 0.1f, 0);

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTrans = Camera.main.transform;

        _state = PlayerState.Explore;

        EventBus.Instance.VisitDialogueNode += HandleVisitDialogueNode;
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        switch(_state)
        {
            case PlayerState.Explore: 
                LookAndMove();
                break;
        }

        RaycastHit raycastHit;
        if(Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out raycastHit,
                _maxAimDistance
        ))
        {
            _aimReticle.position = raycastHit.point + _reticleOffset;
        }

        if(Input.GetMouseButtonDown(0))
        {
            _splineAnimator.Play();
            _needsAnimRestart = true;
        }

        if(_needsAnimRestart && _splineAnimator.ElapsedTime >= _splineAnimator.Duration*2)
        {
            _needsAnimRestart = false;
            _splineAnimator.Restart(false);
        }
    }

    // ------------------------------------------------------------------------
    private void LookAndMove ()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        _rotationY += mouseY * _mouseSensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -60.0f, 60.0f);

        float mouseX = Input.GetAxis("Mouse X");
        _rotationX += mouseX * _mouseSensitivity;

        _cameraTrans.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, _rotationX, 0);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        _characterController.Move(
            ((vertical * Vector3.forward) + (horizontal * Vector3.right))
            * _moveSpeed * Time.deltaTime
        );
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
        _state = PlayerState.Dialogue;
        Cursor.lockState = CursorLockMode.None;
    }
}
