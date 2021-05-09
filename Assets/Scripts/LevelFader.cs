using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelFader : MonoBehaviour
{
    public float audioFadeInRate;
    public float sceneFadeInRate;
    public float audioFadeOutRate;
    public float sceneFadeOutRate;

    public void Update()
    {
        if (fadeOut)
        {
            foreach (var audioSource in audioSources)
            {
                audioSource.volume -= Math.Max(audioFadeOutRate * Time.deltaTime, 0);
            }

            var r = colorGrading.colorFilter.value.r;
            var b = colorGrading.colorFilter.value.b;
            var g = colorGrading.colorFilter.value.g;

            colorGrading.colorFilter.value.r = Math.Max(r - (sceneFadeOutRate * Time.deltaTime), 0f);
            colorGrading.colorFilter.value.b = Math.Max(b - (sceneFadeOutRate * Time.deltaTime), 0f);
            colorGrading.colorFilter.value.g = Math.Max(g - (sceneFadeOutRate * Time.deltaTime), 0f);

            if (r + b + g == 0)
            {
                onFadeComplete();
                fadeOut = false;
            }
        }

        if (fadeIn)
        {
            foreach (var audioSource in audioSources)
            {
                audioSource.volume += Math.Min(audioFadeInRate * Time.deltaTime, 1);
            }

            var r = colorGrading.colorFilter.value.r;
            var b = colorGrading.colorFilter.value.b;
            var g = colorGrading.colorFilter.value.g;

            colorGrading.colorFilter.value.r = Math.Min(r + (sceneFadeInRate * Time.deltaTime), 1f);
            colorGrading.colorFilter.value.b = Math.Min(b + (sceneFadeInRate * Time.deltaTime), 1f);
            colorGrading.colorFilter.value.g = Math.Min(g + (sceneFadeInRate * Time.deltaTime), 1f);

            if (r + b + g == 3)
            {
                onFadeComplete();
                fadeIn = false;
            }
        }
    }

    public void FadeIn(IEnumerable<AudioSource> audioSources, Action onFadeComplete)
    {
        colorGrading = GetComponent<PostProcessVolume>().profile.GetSetting<ColorGrading>();
        colorGrading.colorFilter.value.r = 0;
        colorGrading.colorFilter.value.b = 0;
        colorGrading.colorFilter.value.g = 0;
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0;
        }

        this.audioSources = audioSources;
        this.onFadeComplete = onFadeComplete;

        fadeIn = true;
    }

    public void FadeOut(IEnumerable<AudioSource> audioSources, Action onFadeComplete)
    {
        this.audioSources = audioSources;
        this.onFadeComplete = onFadeComplete;

        fadeOut = true;
    }

    private IEnumerable<AudioSource> audioSources;
    private ColorGrading colorGrading;
    private Action onFadeComplete;

    private bool fadeOut = false;
    private bool fadeIn = false;
}
