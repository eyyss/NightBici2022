using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "AudioData")]
public class AudioData : ScriptableObject
{
    public List<AudioClip> audioClips;
    public AudioMixerGroup group;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float minPitch = 0.95f;
    [Range(0.1f, 3f)] public float maxPitch = 1.05f;

    public void Play2D(MonoBehaviour mono)
    {
        if (audioClips.Count == 0) return;
        int r = Random.Range(0, audioClips.Count);
        float pitch = Random.Range(minPitch, maxPitch);
        audioClips[r].PlayClip2D(mono, group, volume, pitch);
    }

    public void Play3D(MonoBehaviour mono, Vector3 pos)
    {
        if (audioClips.Count == 0) return;
        int r = Random.Range(0, audioClips.Count);
        float pitch = Random.Range(minPitch, maxPitch);
        audioClips[r].PlayClip3D(mono, pos, group, volume, pitch);
    }
}
