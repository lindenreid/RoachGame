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
    [SerializeField] public int _fadeInDurationFrames = 120;

    private ColorAdjustments _colorAdjustments;
    private bool _doFadeIn;
    private int _fadeTimeFrames;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start()
    {
        _ppVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
        Assert.IsNotNull(_colorAdjustments);

        SetFadeInValue(1.0f);
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if(_doFadeIn)
        {
            _fadeTimeFrames += 1;

            SetFadeInValue((float)_fadeTimeFrames / (float)_fadeInDurationFrames);

            if(_fadeTimeFrames >= _fadeInDurationFrames)
            {
                SetFadeInValue(1.0f);
                _doFadeIn = false;
            }
        }
    } 

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartFadeIn ()
    {
        _doFadeIn = true;
        _fadeTimeFrames = 0;
    }

    // ------------------------------------------------------------------------
    private void SetFadeInValue(float t)
    {
        _colorAdjustments.colorFilter.SetValue(new ColorParameter(
            Color.Lerp(Color.black, Color.white, t),
            true
        ));
    }
}
