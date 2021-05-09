using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EveningLevelEffects : MonoBehaviour, ILevelEffects
{
    public AudioClip peepersSoundClip;
    public List<AudioClip> loonSoundClips;

    public void SetAudioSources(AudioSource ambientSource, AudioSource oneShotSource)
    {
        this.ambientSource = ambientSource;
        this.oneShotSource = oneShotSource;
    }

    public void SetDirectionalLight(Light directionalLight)
    {
        this.directionalLight = directionalLight;
    }

    public void StartAmbient()
    {
        StartCoroutine(DoPeepers());
        StartCoroutine(DoLoon());
    }

    private IEnumerator DoPeepers()
    {
        while (true)
        {
            oneShotSource.PlayOneShot(peepersSoundClip, .3f);
            yield return new WaitForSeconds(peepersSoundClip.length - .5f);
        }
    }

    private IEnumerator DoLoon()
    {
        yield return new WaitForSeconds(Random.Range(5, 15));
        while (true)
        {
            float loonPause = Random.Range(20, 45);
            yield return new WaitForSeconds(loonPause);
            oneShotSource.PlayOneShot(loonSoundClips[Random.Range(0, loonSoundClips.Count)], 1);
        }
    }

    private AudioSource ambientSource;
    private AudioSource oneShotSource;
    private Light directionalLight;
}
