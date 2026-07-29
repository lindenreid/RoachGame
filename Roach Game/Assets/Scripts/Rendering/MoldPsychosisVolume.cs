/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering/MoldPsychosis.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering
 * Created Date: Tuesday, July 28th 2026, 6:25:22 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/MoldPsychosisVolume", typeof(UniversalRenderPipeline))]
public class MoldPsychosis : VolumeComponent, IPostProcessComponent
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public BoolParameter showEffect = new BoolParameter(false);
	public BoolParameter useVideo1 = new BoolParameter(false);
    public BoolParameter useVideo2 = new BoolParameter(false);
    public BoolParameter useVideo3 = new BoolParameter(false);
    public ClampedFloatParameter chromaticAbberation = new ClampedFloatParameter(0, 0, 1);
    
    public bool IsActive() => showEffect.value;
   
    public bool IsTileCompatible() => true;
}