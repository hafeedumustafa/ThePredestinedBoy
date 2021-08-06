using UnityEngine.Audio;
using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public bool RandomSoundTime;
    public bool RandomAudio;
    public float[] RSTRange;
    public AudioClip[] RARange;
    public AudioClip AudioClip;
    public AudioSource AudioSource;
    
    void Start()
    {
        if (RandomSoundTime == true) {
            StartCoroutine(RandomSoundTimeAudio());
        }
    }

    IEnumerator RandomSoundTimeAudio()
    {
        if (RandomAudio == true) {
            int RandomAudioRange = Random.Range(0, 2);
            AudioClip SettingRandomAudio = RARange[RandomAudioRange];
            AudioClip = SettingRandomAudio;
            AudioSource.clip = SettingRandomAudio;
        }
            float RandomWaitingSeconds = Random.Range(RSTRange[0], RSTRange[1]);


        yield return new WaitForSeconds(RandomWaitingSeconds);
        AudioSource.Play();

        StartCoroutine(RandomSoundTimeAudio());
    }

}
