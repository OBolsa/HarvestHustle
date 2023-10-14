using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public List<AudioClip> footStepsAudio;
    public AudioSource source;

    private void OnEnable()
    {
        source.clip = SortedClip;
        source.Play();
    }

    public AudioClip SortedClip { get => footStepsAudio[Random.Range(0, footStepsAudio.Count - 1)]; }
}