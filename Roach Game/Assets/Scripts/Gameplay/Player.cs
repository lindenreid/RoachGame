/*
 * File: Player.cs
 * Created: 25/05/2026, 11:14:18 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class Player : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _mouseSensitivity;

    private Transform _cameraTrans;
    private float _rotationX;
    private float _rotationY;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTrans = Camera.main.transform;
    }

    // ------------------------------------------------------------------------
    private void Update ()
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
        transform.Translate(
            ((vertical * Vector3.forward) + (horizontal * Vector3.right))
            * _moveSpeed * Time.deltaTime
        );
    }
}
