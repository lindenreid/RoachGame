/*
 * File: AudioController.cs
 * Created: 26/05/2026, 9:20:16 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;
using UnityEngine.Assertions;

public class AudioController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioClip _roachHitClip;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static AudioController _Instance { get; private set; }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
            return;
        }

        _Instance = this;
    }

    // ------------------------------------------------------------------------
    private void Start ()
    {
        EventBus._Instance.RoachHit += HandleRoachHit;
        EventBus._Instance.SequenceStarted += HandleSequenceStarted;
    }

    // ------------------------------------------------------------------------
    private void HandleRoachHit (Roach roach)
    {
        _sfxAudioSource.PlayOneShot(_roachHitClip);
    }

    // ------------------------------------------------------------------------
    private void HandleSequenceStarted(Sequence sequence)
    {
        switch(sequence._AudioType)
        {
            case SequenceAudioType.ContinuePreviousClip:
                // do nothing (intentionally)
                break;
            case SequenceAudioType.StopClipOnly:
                _musicAudioSource.Stop();
                break;
            case SequenceAudioType.PlayNewClip:
                Assert.IsNotNull(sequence._Music);
                _musicAudioSource.clip = sequence._Music;
                _musicAudioSource.Play();   
                break;
        }
    }
}
