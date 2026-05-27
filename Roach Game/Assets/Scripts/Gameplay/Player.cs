/*
 * File: Player.cs
 * Created: 25/05/2026, 11:14:18 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

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
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 7.0f;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private SplineContainer _shoeSpline;
    [SerializeField] private SplineAnimate _splineAnimator;
    [SerializeField] private Transform _reticleDefaultPosition;
    [SerializeField] private MeshRenderer _reticleRenderer;
    [SerializeField] private Transform _aimReticle;
    [SerializeField] private float _maxAimDistance;
    [SerializeField] private LayerMask _roachLayer;
    [SerializeField] private Transform _roachHoldLoc;

    private Transform _cameraTrans;
    private float _rotationX;
    private float _rotationY;

    private PlayerState _state;
    private bool _needsAnimRestart;
    private Vector3 _reticleOffset = new Vector3(0, 0.1f, 0);
    private Material _reticleMat;
    private float _reticleAlpha;
    private Color _reticleHit;
    private Color _reticleMiss;
    private Color _reticleInvalid;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTrans = Camera.main.transform;

#if UNITY_WEBGL
        _mouseSensitivity = _mouseSensitivity / 2.0f;
#endif

        _state = PlayerState.Explore;

        EventBus.Instance.VisitDialogueNode += HandleVisitDialogueNode;
        EventBus.Instance.RoachCollected += HandleRoachCollected;

        _reticleMat = _reticleRenderer.material;
        _reticleAlpha = _reticleMat.color.a;

        _reticleHit = new Color(0, 1, 0, _reticleAlpha);
        _reticleMiss = new Color(1, 1, 0, _reticleAlpha);
        _reticleInvalid = new Color(1, 0, 0, _reticleAlpha);
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        switch(_state)
        {
            case PlayerState.Explore: 
                LookAndMove();
                UpdateShoe();
                break;
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

        float speed = _walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = _runSpeed;
        }

        transform.Translate(
            ((vertical * Vector3.forward) + (horizontal * Vector3.right))
            * speed * Time.deltaTime
        );
    }

    // ------------------------------------------------------------------------
    private void UpdateShoe ()
    {
        RaycastHit raycastHit;
        bool aimRaycastHit = Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out raycastHit,
                _maxAimDistance
        );
        if(aimRaycastHit)
        {
            if(((1<<raycastHit.collider.gameObject.layer) & _roachLayer) != 0)
            {
                _reticleMat.color = _reticleHit;
            }
            else
            {
                _reticleMat.color = _reticleMiss;
            }

            _aimReticle.position = raycastHit.point + _reticleOffset;
            SetShoeSplineDestination(raycastHit.point);
        }
        else
        {
            _reticleMat.color = _reticleInvalid;
            _aimReticle.position = _reticleDefaultPosition.position;
            SetShoeSplineDestination(_reticleDefaultPosition.position);
        }

        if(aimRaycastHit && Input.GetMouseButtonDown(0))
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
    private void SetShoeSplineDestination(Vector3 targetWorldPos)
    {
        var targetKnot = _shoeSpline.Spline.Knots.ToArray()[1];
        targetKnot.Position = _shoeSpline.transform.InverseTransformPoint(targetWorldPos);
        _shoeSpline.Spline.SetKnot(1, targetKnot);
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
        _state = PlayerState.Dialogue;
        Cursor.lockState = CursorLockMode.None;
    }

    // ------------------------------------------------------------------------
    private void HandleRoachCollected(Roach roach)
    {
        roach.transform.position = _roachHoldLoc.position;
        roach.transform.SetParent(_roachHoldLoc);
    }
}
