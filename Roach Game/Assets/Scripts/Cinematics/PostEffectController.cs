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
    [SerializeField] private Volume _ppVolume;
    [SerializeField] private float _fadeInDurationSeconds = 2.0f;
    [SerializeField] private float _fadeOutDurationSeconds = 2.0f;

    private ColorAdjustments _colorAdjustments;
    private MoldPsychosis _moldPostEffect;
    private bool _doFadeIn;
    private bool _doFadeOut;
    private float _fadeTimeSeconds;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start()
    {
        _ppVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
        _ppVolume.profile.TryGet<MoldPsychosis>(out _moldPostEffect);
        Assert.IsNotNull(_colorAdjustments);
        Assert.IsNotNull(_moldPostEffect);

        SetFadeInNormalizedValue(1.0f);
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if(_doFadeIn)
        {
            _fadeTimeSeconds += Time.deltaTime;

            SetFadeInNormalizedValue((float)_fadeTimeSeconds / (float)_fadeInDurationSeconds);

            if(_fadeTimeSeconds >= _fadeInDurationSeconds)
            {
                SetFadeInNormalizedValue(1.0f);
                _doFadeIn = false;
            }
        }
        else if(_doFadeOut)
        {
            _fadeTimeSeconds += Time.deltaTime;

            SetFadeOutNormalizedValue((float)_fadeTimeSeconds / (float)_fadeOutDurationSeconds);

            if(_fadeTimeSeconds >= _fadeOutDurationSeconds)
            {
                SetFadeOutNormalizedValue(1.0f);
                _doFadeOut = false;
            }
        }
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void SetWhiteBackgroundEnabled (bool enabled)
    {
        _moldPostEffect.useWhiteBackground.SetValue(new BoolParameter(enabled, true));
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void SetVideo1Enabled (bool enabled)
    {
        _moldPostEffect.useVideo1.SetValue(new BoolParameter(enabled, true));
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void SetVideo2Enabled (bool enabled)
    {
        _moldPostEffect.useVideo2.SetValue(new BoolParameter(enabled, true));
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void SetVideo3Enabled (bool enabled)
    {
        _moldPostEffect.useVideo3.SetValue(new BoolParameter(enabled, true));
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartFadeIn ()
    {
        _doFadeIn = true;
        _doFadeOut = false;
        _fadeTimeSeconds = 0;
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartFadeOut ()
    {
        _doFadeIn = false;
        _doFadeOut = true;
        _fadeTimeSeconds = 0;
    }

    // ------------------------------------------------------------------------
    private void SetFadeInNormalizedValue(float t)
    {
        _colorAdjustments.colorFilter.SetValue(new ColorParameter(
            Color.Lerp(Color.black, Color.white, t),
            true
        ));
    }

    // ------------------------------------------------------------------------
    private void SetFadeOutNormalizedValue(float t)
    {
        _colorAdjustments.colorFilter.SetValue(new ColorParameter(
            Color.Lerp(Color.white, Color.black, t),
            true
        ));
    }
}
