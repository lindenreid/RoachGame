/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics/PostEffectController.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics
 * Created Date: Wednesday, July 1st 2026, 5:54:45 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] public Volume _ppVolume;

    private ColorAdjustments _colorAdjustments;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Start()
    {
        _ppVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
        Assert.IsNotNull(_colorAdjustments);

        SetFadeInAlpha(0.0f);
    } 

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartFadeIn ()
    {
        
    }

    // ------------------------------------------------------------------------
    private void SetFadeInAlpha(float a)
    {
        _colorAdjustments.colorFilter.SetValue(new ColorParameter(
            Color.Lerp(Color.white, Color.black, a),
            true
        ));
    }
}
