using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    
    [SerializeField]
    private List<AudioClip> audioClipList;

    [SerializeField] private float lowVolumeValue = 0.005f;
    [SerializeField] private float normalVolumeValue = 1f;
    
    private Dictionary<string, AudioClip> audioClips;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        audioClips = new Dictionary<string, AudioClip>();
        foreach (var audioClip in audioClipList)
        {
            audioClips.Add(audioClip.name, audioClip);
        }
    }

    public void PlaySound(string audioName, bool lowVolume = false)
    {
        if (audioClips.ContainsKey(audioName))
        {
            //audioSource.volume = normalVolumeValue;
            audioSource.PlayOneShot(audioClips[audioName]);
        }
    }
}

