/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering/ReflectionProbeBouncer.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering
 * Created Date: Tuesday, August 25th 2026, 7:43:53 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ReflectionProbeBouncer : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ReflectionProbe[] _probes;
    [SerializeField] private Renderer[] _metalRenderers; 
    [SerializeField] private int _matReplaceIndex = 2;
    [SerializeField] private float _originalSmoothness = 1.0f;
    [SerializeField] private float _replaceSmoothness = 0.5f;
    [SerializeField] private string _smoothnessPropertyName = "_Smoothness";

    private int[] _probeIDs;
    private bool _running;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
        BakeProbes();
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if(!_running) return;

        bool done = true;
        int i = 0;
        foreach(int probeID in _probeIDs)
        {
            if(!_probes[i].IsFinishedRendering(probeID))
            {
                done = false;
                break;
            }
            i++;
        }

        if(done)
        {
            ResetRenderers();
        }
    } 

    // ------------------------------------------------------------------------
    public void BakeProbes ()
    {
        StartCoroutine(BakeProbesRoutine());
    }

    // ------------------------------------------------------------------------
    public IEnumerator BakeProbesRoutine ()
    {
        Debug.Log("Beginning bake");

        foreach(Renderer renderer in _metalRenderers)
        {
            if(Application.isPlaying)
            {
                renderer.materials[_matReplaceIndex].SetFloat(_smoothnessPropertyName, _replaceSmoothness);
            }
            else
            {
                renderer.sharedMaterials[_matReplaceIndex].SetFloat(_smoothnessPropertyName, _replaceSmoothness);
            }
        }

        yield return new WaitForEndOfFrame();

        _probeIDs = new int[_probes.Length];
        int i = 0;
        foreach(ReflectionProbe probe in _probes)
        {
            _probeIDs[i] = probe.RenderProbe();
            i++;
        }

        _running = true;
    }

    // ------------------------------------------------------------------------
    private void ResetRenderers ()
    {
        _running = false;

        foreach(Renderer renderer in _metalRenderers)
        {
            if(Application.isPlaying)
            {
                renderer.materials[_matReplaceIndex].SetFloat(_smoothnessPropertyName, _originalSmoothness);
            }
            else
            {
                renderer.sharedMaterials[_matReplaceIndex].SetFloat(_smoothnessPropertyName, _originalSmoothness);
            }
        }

        Debug.LogFormat("Finished bake. updated renderers: {0}", _metalRenderers.Length);
    }
}
