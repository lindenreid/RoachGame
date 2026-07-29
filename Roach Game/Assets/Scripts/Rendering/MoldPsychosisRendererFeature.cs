/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering/MoldPsychosisRendererFeature.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering
 * Created Date: Tuesday, July 28th 2026, 6:36:30 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MoldPsychosisRendererFeature : ScriptableRendererFeature
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    class MoldPsychosisPass : ScriptableRenderPass
    {
        // --------------------------------------------------------------------
        // Variables
        // --------------------------------------------------------------------
        private Material material;
        private MoldPsychosis volumeComponent;

        // --------------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------------
        public MoldPsychosisPass(Shader shader)
        {
            if (shader != null)
                material = new Material(shader);
                
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        // --------------------------------------------------------------------
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            // Fetch the active settings from the Volume Stack
            var stack = VolumeManager.instance.stack;
            volumeComponent = stack.GetComponent<MoldPsychosis>();

            // Do not render if the volume component is missing or inactive
            if (volumeComponent == null || !volumeComponent.IsActive()) return;

            CommandBuffer cmd = CommandBufferPool.Get("Mold Psychosis");
            cmd.Clear();

            // Send volume variables directly into your shader properties
            material.SetInt("_UseVideo1", volumeComponent.useVideo1.value? 1 : 0);
            material.SetInt("_UseVideo2", volumeComponent.useVideo2.value? 1 : 0);
            material.SetInt("_UseVideo3", volumeComponent.useVideo3.value? 1 : 0);
            material.SetInt("_UseWhiteBackground", volumeComponent.useWhiteBackground.value? 1 : 0);
            material.SetFloat("_ChromaticAbberation", volumeComponent.chromaticAbberation.value);
            material.SetFloat("_VoronoiSpeed", volumeComponent.voronoiSpeed.value);
            material.SetFloat("_VoronoiDensity", volumeComponent.voronoiDensity.value);
            material.SetInt("_UseMold", volumeComponent.useMold.value? 1 : 0);
            material.SetFloat("_MoldCoverage", (1.0f - volumeComponent.moldCoverage.value)*10.0f);
            material.SetTexture("_VideoRenderTex1", volumeComponent.video1Tex.value);
            material.SetTexture("_VideoRenderTex2", volumeComponent.video2Tex.value);
            material.SetTexture("_VideoRenderTex3", volumeComponent.video3Tex.value);

            // Fetch camera target textures for the blit process
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            
            // Perform a fullscreen blit using the custom material
            Blit(cmd, source, source, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private MoldPsychosisPass moldPass;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Create()
    {
        Shader shader = Shader.Find("Shader Graphs/MoldPsychosis");
        Assert.IsNotNull(shader);
        moldPass = new MoldPsychosisPass(shader);
    }

    // ------------------------------------------------------------------------
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.postProcessEnabled)
        {
            renderer.EnqueuePass(moldPass);
        }
    }
}
