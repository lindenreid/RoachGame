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
    [SerializeField] private Color _defaultLightColor = Color.white;
    [SerializeField] private Color _redLightColor = Color.red;
    [SerializeField] private float _ambientLightIntensityLightsOff = 0.2f;
    [SerializeField] private float _ambientLightIntensityLightsRed = 0.6f;
    [SerializeField] private float _ambientLightIntensityLightsOn = 1.0f;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline signal callback
    public void TurnLightsOff ()
    {
        RenderSettings.ambientIntensity = _ambientLightIntensityLightsOff;
        _mainDirLight.enabled = false;
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void TurnLightsOn ()
    {
        RenderSettings.ambientIntensity = _ambientLightIntensityLightsOn;
        _mainDirLight.enabled = true;
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void ActivateRedLights (bool activate)
    {
        _mainDirLight.color = activate ? _redLightColor : _defaultLightColor;
        RenderSettings.ambientIntensity = activate? _ambientLightIntensityLightsRed : _ambientLightIntensityLightsOn;
    }

    // ------------------------------------------------------------------------
    // timeline callback
    public void ActivateRoachWorldLights ()
    {
        
    }
}