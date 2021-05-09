using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class MountainLevelEffects : MonoBehaviour, ILevelEffects
    {
        public List<AudioClip> thunderAudioClips;
        public AudioClip ambientThunderAudioClip;
        public AudioClip rainAudioClip;

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
            StartCoroutine(MakeLightning());
            StartCoroutine(PlayAmbientThunder());
            StartCoroutine(PlayAmbientRain());
        }

        private IEnumerator MakeLightning()
        {
            float defaultLightIntensity = directionalLight.intensity;

            while (true)
            {
                float lightningPause = UnityEngine.Random.Range(30, 50);
                yield return new WaitForSeconds(lightningPause);
                directionalLight.intensity = 3;
                yield return new WaitForSeconds(.2f);
                directionalLight.intensity = defaultLightIntensity;
                yield return new WaitForSeconds(.1f);
                directionalLight.intensity = 1;
                yield return new WaitForSeconds(.2f);
                directionalLight.intensity = defaultLightIntensity;
                yield return new WaitForSeconds(.1f);
                directionalLight.intensity = 2;
                yield return new WaitForSeconds(.1f);
                directionalLight.intensity = defaultLightIntensity;
                yield return new WaitForSeconds(.3f);
                oneShotSource.PlayOneShot(thunderAudioClips[UnityEngine.Random.Range(0, thunderAudioClips.Count())], .55f);
            }
        }

        private IEnumerator PlayAmbientThunder()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5, 15));
            while (true)
            {
                float lightningPause = UnityEngine.Random.Range(20, 35);
                yield return new WaitForSeconds(lightningPause);
                oneShotSource.PlayOneShot(ambientThunderAudioClip, .55f);
            }
        }

        private IEnumerator PlayAmbientRain()
        {
            while (true)
            {
                ambientSource.PlayOneShot(rainAudioClip, .55f);
                yield return new WaitForSeconds(rainAudioClip.length - .75f);
                ambientSource.Stop();
            }
        }

        private AudioSource ambientSource;
        private AudioSource oneShotSource;
        private Light directionalLight;
    }
}
