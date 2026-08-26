/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering/ReflectionProbeBouncer.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering
 * Created Date: Tuesday, August 25th 2026, 7:43:53 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class ReflectionProbeBouncer : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ReflectionProbe[] _probes;
    [SerializeField] private Renderer[] _metalRenderers; 
    [SerializeField] private int _matReplaceIndex = 2;
    [SerializeField] private Material _replacementMat;

    private Material[] _originalMaterials;
    private int[] _probeIDs;
    private bool _running;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void Start()
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
        _originalMaterials = new Material[_metalRenderers.Length];
        int i = 0;
        foreach(Renderer renderer in _metalRenderers)
        {
            if(Application.isPlaying)
            {
                _originalMaterials[i] = renderer.materials[_matReplaceIndex];
                renderer.materials[_matReplaceIndex] = _replacementMat;
            }
            else
            {
                _originalMaterials[i] = renderer.sharedMaterials[_matReplaceIndex];
                renderer.sharedMaterials[_matReplaceIndex] = _replacementMat;
            }
            i++;
        }

        _probeIDs = new int[_probes.Length];
        i = 0;
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

        int i = 0;
        foreach(Renderer renderer in _metalRenderers)
        {
            if(Application.isPlaying)
            {
                renderer.materials[_matReplaceIndex] = _originalMaterials[i];
            }
            else
            {
                renderer.sharedMaterials[_matReplaceIndex] = _originalMaterials[i];
            }
            i++;
        }
    }
}
