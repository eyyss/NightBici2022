using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioExtensions
{
    public static AudioSource PlayClip2D(this AudioClip clip, MonoBehaviour mono, AudioMixerGroup group = null, float volume = 1, float pitch = 1)
    {
        if (clip == null) return null;

        GameObject go = new GameObject();

        go.name = clip.name;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        if (group != null) audioSource.outputAudioMixerGroup = group;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.spatialBlend = 0;
        audioSource.Play();
        GameObject.Destroy(audioSource.gameObject, clip.length);
        return audioSource;
    }

    public static AudioSource PlayClip3D(this AudioClip clip, MonoBehaviour mono, Vector3 pos, AudioMixerGroup group = null, float volume = 1, float pitch = 1)
    {
        if (clip == null) return null;
        GameObject go = new GameObject();

        go.name = clip.name;
        go.transform.position = pos;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        if (group != null) audioSource.outputAudioMixerGroup = group;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1;
        audioSource.maxDistance = 15;
        audioSource.Play();
        GameObject.Destroy(audioSource.gameObject, clip.length);
        return audioSource;
    }




}
