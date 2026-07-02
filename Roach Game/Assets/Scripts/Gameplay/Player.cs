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
    // Variables
    // ------------------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 7.0f;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private CharacterController _cc;
    [Header("Shoe animation")]
    [SerializeField] private Collider _shoeCollider;
    [SerializeField] private MeshRenderer _shoeRenderer;
    [SerializeField] private Transform _shoeDefaultPos;
    [SerializeField] private SplineContainer _shoeSpline;
    [SerializeField] private SplineAnimate _splineAnimator;
    [Header("Aiming reticle")]
    [SerializeField] private Transform _reticleDefaultPosition;
    [SerializeField] private MeshRenderer _reticleRenderer;
    [SerializeField] private Transform _aimReticle;
    [SerializeField] private float _maxAimDistance;
    [Header("Roach collection")]
    [SerializeField] private LayerMask _roachLayer;
    [SerializeField] private Transform _roachHoldLoc;
    [Header("Combat")]
    [SerializeField] private int _maxHealth = 5;
    [Header("Cinematics")]
    [SerializeField] private Transform _cameraTransform;

    // movement and aiming
    private bool _inputEnabled;
    private Transform _cameraTrans;
    private float _rotationX;
    private float _rotationY;
    private bool _needsAnimRestart;
    private int _health;

    // aiming reticle
    private Vector3 _reticleOffset = new Vector3(0, 0.1f, 0);
    private Material _reticleMat;
    private float _reticleAlpha;
    private Color _reticleHit;
    private Color _reticleMiss;
    private Color _reticleInvalid;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static Player _Instance { get; private set; }

    public Vector3 _Position => transform.position;
    public int _Health => _health;
    public Vector3 _CameraPosition => _cameraTransform.position;

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

        _health = _maxHealth;
        _inputEnabled = true;
    }

    // ------------------------------------------------------------------------
    private void Start ()
    {
        _cameraTrans = Camera.main.transform;

#if UNITY_WEBGL
        _mouseSensitivity = _mouseSensitivity / 2.0f;
#endif

        EventBus._Instance.RoachCollected += HandleRoachCollected;

        _reticleMat = _reticleRenderer.material;
        _reticleAlpha = _reticleMat.color.a;

        _reticleHit = new Color(0, 1, 0, _reticleAlpha);
        _reticleMiss = new Color(1, 1, 0, _reticleAlpha);
        _reticleInvalid = new Color(1, 0, 0, _reticleAlpha);

        _splineAnimator.transform.position = _shoeDefaultPos.position;
    }

    // ------------------------------------------------------------------------
    private void Update ()
    {
        if(_inputEnabled)
        {
            LookAndMove();   
            UpdateShoe();
        }
    }

    // ------------------------------------------------------------------------
    public void SetInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;
        _reticleRenderer.enabled = enabled;
        _shoeRenderer.enabled = enabled;
        _shoeCollider.enabled = enabled;

        if(!enabled)
        {
            ResetShoeAnim();
        }
    }

    // ------------------------------------------------------------------------
    public bool DamageAndTryKill ()
    {
        _health--;
        EventBus._Instance.InvokePlayerHealthChanged();

        if(_health == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            SetInputEnabled(false);
            EventBus._Instance.InvokePlayerDied();
        }

        return _health == 0;
    }

    // ------------------------------------------------------------------------
    public void SetupForActionSequence ()
    {
        SetInputEnabled(true);
        Cursor.lockState = CursorLockMode.Locked;
        _health = _maxHealth;
        EventBus._Instance.InvokePlayerHealthChanged();
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

        Vector3 playerForward = transform.TransformDirection(Vector3.forward);
        Vector3 playerRight = transform.TransformDirection(Vector3.right);
        _cc.Move(((vertical * playerForward) + (horizontal * playerRight))
            * speed * Time.deltaTime);
    }

    // ------------------------------------------------------------------------
    private void UpdateShoe ()
    {
        SetShoeSplineStartPos();

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
                SetValidAim(raycastHit);
            }
            else if(raycastHit.normal == Vector3.up)
            {
                _reticleMat.color = _reticleMiss;
                SetValidAim(raycastHit);
            }
            else
            {
                SetInvalidAim();
            }
        }
        else
        {
            SetInvalidAim();
        }

        if(aimRaycastHit && Input.GetMouseButtonDown(0))
        {
            _splineAnimator.Play();
            _needsAnimRestart = true;
        }

        if(_needsAnimRestart && _splineAnimator.ElapsedTime >= _splineAnimator.Duration*2)
        {
            ResetShoeAnim();
        }
    }

    // ------------------------------------------------------------------------
    private void SetValidAim (RaycastHit raycastHit)
    {
        _aimReticle.position = raycastHit.point + _reticleOffset;
        SetShoeSplineDestination(raycastHit.point);
    }

    // ------------------------------------------------------------------------
    private void SetInvalidAim ()
    {
        _reticleMat.color = _reticleInvalid;
        _aimReticle.position = _reticleDefaultPosition.position;
        SetShoeSplineDestination(_reticleDefaultPosition.position);
    }

    // ------------------------------------------------------------------------
    private void ResetShoeAnim()
    {
        _needsAnimRestart = false;
        _splineAnimator.Restart(false);
    }

    // ------------------------------------------------------------------------
    private void SetShoeSplineStartPos()
    {
        var targetKnot = _shoeSpline.Spline.Knots.ToArray()[0];
        targetKnot.Position = _shoeSpline.transform.InverseTransformPoint(_shoeDefaultPos.position);
        _shoeSpline.Spline.SetKnot(0, targetKnot);
    }

    // ------------------------------------------------------------------------
    private void SetShoeSplineDestination(Vector3 targetWorldPos)
    {
        var targetKnot = _shoeSpline.Spline.Knots.ToArray()[1];
        targetKnot.Position = _shoeSpline.transform.InverseTransformPoint(targetWorldPos);
        _shoeSpline.Spline.SetKnot(1, targetKnot);
    }

    // ------------------------------------------------------------------------
    private void HandleRoachCollected(Roach roach)
    {
        roach.transform.position = _roachHoldLoc.position;
        roach.transform.SetParent(_roachHoldLoc);
    }
}
