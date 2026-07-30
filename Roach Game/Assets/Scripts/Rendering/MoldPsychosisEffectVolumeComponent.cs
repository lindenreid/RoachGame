using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Defines a custom Volume Override component that controls the intensity of the URP Post-processing effect on a Scriptable Renderer Feature.
// For more information about the VolumeComponent API, refer to https://docs.unity3d.com/Packages/com.unity.render-pipelines.core@17.2/api/UnityEngine.Rendering.VolumeComponent.html

// Add the Volume Override to the list of available Volume Override components in the Volume Profile.
[VolumeComponentMenu("Post-processing Custom/Mold Psychosis")]

// If the related Scriptable Renderer Feature doesn't exist, display a warning about adding it to the renderer.
[VolumeRequiresRendererFeatures(typeof(MoldPsychosisEffectRendererFeature))]

// Make the Volume Override active in the Universal Render Pipeline.
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]

// Set the name of the volume component in the list in the Volume Profile.
[DisplayInfo(name = "Mold Psychosis")]

// Create the Volume Override by inheriting from VolumeComponent
public sealed class MoldPsychosisEffectVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public BoolParameter useWhiteBackground = new BoolParameter(false);
	public BoolParameter useVideo1 = new BoolParameter(false);
    public BoolParameter useVideo2 = new BoolParameter(false);
    public BoolParameter useVideo3 = new BoolParameter(false);
    public ClampedFloatParameter chromaticAbberation = new ClampedFloatParameter(0, 0, 1);
    public BoolParameter useMold = new BoolParameter(false);
    public ClampedFloatParameter moldCoverage = new ClampedFloatParameter(0.4f, 0, 1);
    public FloatParameter voronoiSpeed = new FloatParameter(2);
    public FloatParameter voronoiDensity = new FloatParameter(5);
    public TextureParameter video1Tex = new TextureParameter(null);
    public TextureParameter video2Tex = new TextureParameter(null);
    public TextureParameter video3Tex = new TextureParameter(null);
    
    public bool IsActive() => useWhiteBackground.value || useVideo1.value || useVideo2.value || useVideo3.value || useMold.value;
}
