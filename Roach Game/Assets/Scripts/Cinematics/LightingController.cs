/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics/LightingController.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics
 * Created Date: Wednesday, July 1st 2026, 9:31:18 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class LightingController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Light _mainDirLight;
    [SerializeField] private float _ambientLightIntensityLightsOff = 0.2f;
    [SerializeField] private float _ambientLightIntensityLightsOn = 1.0f;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void TurnLightsOff ()
    {
        RenderSettings.ambientIntensity = _ambientLightIntensityLightsOff;
        _mainDirLight.enabled = false;
    }

    // ------------------------------------------------------------------------
    public void TurnLightsOn ()
    {
        RenderSettings.ambientIntensity = _ambientLightIntensityLightsOn;
        _mainDirLight.enabled = true;
    }
}