/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Gameplay/Manager.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Gameplay
 * Created Date: Wednesday, August 19th 2026, 2:48:42 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class Manager : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] private Transform _cowerPos;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline callback
    public void TeleportToCowerPos ()
    {
        transform.position = _cowerPos.position;
        transform.rotation = _cowerPos.rotation;
    }
}