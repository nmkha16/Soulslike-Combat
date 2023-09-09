using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Sound> soundList;
    private Dictionary<SoundId,AudioClip> soundDict = new Dictionary<SoundId, AudioClip>();
    // imma do the lazy way noone can stop me
    public static SoundManager instance;

    private void Awake(){
        foreach(var sound in soundList){
            soundDict[sound.id] = sound.audio;
        }

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }  

    private void Start(){
        soundList.Clear();
    }

    private void AdjustPitch(float pitch){
        audioSource.pitch = pitch;
    }

    public void PlayAudio(SoundId id){
        if (soundDict.ContainsKey(id)){
            var audio = soundDict[id];
            AdjustPitch(1f);
            audioSource.volume = 1f;
            audioSource.PlayOneShot(audio);
        }
    }

    public void PlayAudioWithRandomPitch(SoundId id){
        if (soundDict.ContainsKey(id)){
            var audio = soundDict[id];
            AdjustPitch(Random.Range(0.75f,1.25f));
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(audio);
        }
    }
}
