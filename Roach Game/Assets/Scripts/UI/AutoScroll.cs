/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI/AutoScroll.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI
 * Created Date: Thursday, September 3rd 2026, 6:41:49 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private float _wait;
    [SerializeField] private Vector3 _speed;
    [SerializeField] private Transform[] _transforms;

    private float _time;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start()
    {
        _time = _wait;
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if (_time > 0.0f)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            foreach (Transform t in _transforms)
            {
                t.position += _speed * Time.deltaTime;
            }
        }
    }

    // ------------------------------------------------------------------------
    public void Stop()
    {
        this.enabled = false;
    }
}