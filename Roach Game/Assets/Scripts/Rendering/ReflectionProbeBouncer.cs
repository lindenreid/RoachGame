/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering/ReflectionProbeBouncer.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Rendering
 * Created Date: Tuesday, August 25th 2026, 7:43:53 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;
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
    [SerializeField] private Material _replacementMat;

    private List<Material[]> _originalMaterials;
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
        Debug.Log("Beginning bake");

        _originalMaterials = new List<Material[]>();
        int i = 0;
        foreach(Renderer renderer in _metalRenderers)
        {
            if(Application.isPlaying)
            {
                _originalMaterials.Add(renderer.materials);
                
                List<Material> newMats = new List<Material>(renderer.materials);
                newMats[_matReplaceIndex] = _replacementMat;
                renderer.SetMaterials(newMats);
            }
            else
            {
                _originalMaterials.Add(renderer.sharedMaterials);

                List<Material> newMats = new List<Material>(renderer.sharedMaterials);
                newMats[_matReplaceIndex] = _replacementMat;
                renderer.SetSharedMaterials(newMats);
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
                renderer.SetMaterials(new List<Material>(_originalMaterials[i]));
            }
            else
            {
                renderer.SetSharedMaterials(new List<Material>(_originalMaterials[i]));
            }
            i++;
        }

        Debug.LogFormat("Finished bake. updated renderers: {0}", _metalRenderers.Length);
    }
}
