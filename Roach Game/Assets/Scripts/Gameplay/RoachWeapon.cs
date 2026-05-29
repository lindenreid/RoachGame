/*
 * File: RoachWeapon.cs
 * Created: 28/05/2026, 12:52:38 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class RoachWeapon : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    [SerializeField] private Transform _pivot;
    [SerializeField] private GameObject _bangText;
    [SerializeField] private float _textAppearTime;

    private float _textTime;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Update ()
    {
        if(_textTime <= _textAppearTime)
        {
            _textTime += Time.deltaTime;
            if(_textTime >= _textAppearTime)
            {
                _bangText.SetActive(false);
            }
        }
    }

    // ------------------------------------------------------------------------
    public void PointAtPlayer ()
    {
        _pivot.LookAt(Player._Instance._Position);
    }

    // ------------------------------------------------------------------------
    public void Use ()
    {
        _textTime = 0.0f;
        _bangText.SetActive(true);
        
        Player._Instance.Damage();
    }
}