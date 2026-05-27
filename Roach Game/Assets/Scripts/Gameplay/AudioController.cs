/*
 * File: AudioController.cs
 * Created: 26/05/2026, 9:20:16 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class AudioController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioClip _roachHitClip;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Start ()
    {
         EventBus.Instance.RoachHit += HandleRoachHit;
    }

    // ------------------------------------------------------------------------
    private void HandleRoachHit ()
    {
        _sfxAudioSource.PlayOneShot(_roachHitClip);
    }
}
