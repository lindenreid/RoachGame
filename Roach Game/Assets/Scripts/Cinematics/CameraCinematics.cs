/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics/CameraCinematics.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics
 * Created Date: Tuesday, June 30th 2026, 8:36:48 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class CameraCinematics : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Vector3 _roachZoomOffset = Vector3.zero;
    [SerializeField] private SplineContainer _roachZoomSpline;
    [SerializeField] private SplineAnimate _roachZoomSplineAnimator;
    [SerializeField] private SplineContainer _roachZoomOutSpline;
    [SerializeField] private SplineAnimate _roachZoomOutSplineAnimator;

    private Transform _roachTransform;
    private bool _zooming;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static CameraCinematics _Instance { get; private set; }

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
    // timeline signal callback
    public void AnimateRoachZoomIn ()
    {
        Roach targetRoach = GameController._Instance._TargetRoach;

        _roachTransform = targetRoach.transform;
        _zooming = true;

        Vector3 targetRoachPosLocal = _roachZoomSpline.transform.InverseTransformPoint(targetRoach.gameObject.transform.position) + _roachZoomOffset;

        var targetKnot = _roachZoomSpline.Spline.Knots.ToArray()[1];
        targetKnot.Position = targetRoachPosLocal;
        _roachZoomSpline.Spline.SetKnot(1, targetKnot);

        targetKnot = _roachZoomOutSpline.Spline.Knots.ToArray()[0];
        targetKnot.Position = targetRoachPosLocal;
        _roachZoomOutSpline.Spline.SetKnot(0, targetKnot);

        targetKnot = _roachZoomOutSpline.Spline.Knots.ToArray()[1];
        targetKnot.Position = _roachZoomOutSpline.transform.InverseTransformPoint(_cameraTransform.position);
        _roachZoomOutSpline.Spline.SetKnot(1, targetKnot);

        _roachZoomSplineAnimator.Restart(autoplay:true);
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if(_zooming)
        {
            _cameraTransform.LookAt(_roachTransform);
        }
    } 

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void AnimateRoachZoomOut ()
    {
        _roachZoomOutSplineAnimator.Restart(autoplay:true);
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void SequenceEnded ()
    {
        _zooming = false;
        _roachTransform = null;
    }
}
