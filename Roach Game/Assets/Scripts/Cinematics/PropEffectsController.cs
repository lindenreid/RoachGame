/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics/PropEffectsController.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics
 * Created Date: Tuesday, August 4th 2026, 5:53:42 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class PropEffectsController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Renderer _safeRenderer;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline callback
    public void StartSafeMold ()
    {
        Material safeMat = _safeRenderer.material;
        safeMat.SetFloat("_MoldStartTime", Time.time);
    }
}