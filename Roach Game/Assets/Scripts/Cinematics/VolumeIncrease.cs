/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics/VolumeIncrease.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Cinematics
 * Created Date: Thursday, July 2nd 2026, 3:27:00 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class VolumeIncrease: MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private float _startVolume;
    [SerializeField] private float _endVolume;
    [SerializeField] private float _durationSeconds;

    private bool _animate;
    private float _timePassed;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Update()
    {
        if(_animate)
        {
            _timePassed += Time.deltaTime;

            float t = _timePassed / _durationSeconds;
            float volume = Mathf.Lerp(_startVolume, _endVolume, t);
            foreach(AudioSource audioSource in _audioSources)
            {
                audioSource.volume = volume;
            }

            if(_timePassed >= _durationSeconds)
            {
                EndAudioIncrease();
            }
        }
    } 

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartAudioIncrease()
    {
        _animate = true;
        _timePassed = 0;

        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.volume = _startVolume;
        }
    }

    // ------------------------------------------------------------------------
    private void EndAudioIncrease ()
    {
        _animate = false;

        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.volume = _endVolume;
        }
    }
}