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

        if (instance != null){
            Destroy(this);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayAudio(SoundId id){
        if (soundDict.ContainsKey(id)){
            var audio = soundDict[id];
            audioSource.PlayOneShot(audio);
        }
    }
}
