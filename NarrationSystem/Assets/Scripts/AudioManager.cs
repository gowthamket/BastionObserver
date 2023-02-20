using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _oof;
    [SerializeField] AudioClip _splat;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitEffect() {
        _audioSource.PlayOneShot(_splat);
        _audioSource.PlayOneShot(_oof);
    }
}
