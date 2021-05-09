
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClearCutLevelEffects : MonoBehaviour, ILevelEffects
    {
        public AudioClip cicadaSoundClip;
        public AudioClip ambientBirdsSoundClip;
        public List<AudioClip> blackbirdSoundClips;

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
            StartCoroutine(DoCicada());
            StartCoroutine(DoBirdsAmbient());
            StartCoroutine(DoBlackbird());
        }

        private IEnumerator DoCicada()
        {
            while (true)
            {
                float cicadaPause = Random.Range(12, 45);
                yield return new WaitForSeconds(cicadaPause);
                oneShotSource.PlayOneShot(cicadaSoundClip, .06f);
            }
        }

        private IEnumerator DoBirdsAmbient()
        {
            while (true)
            {
                oneShotSource.PlayOneShot(ambientBirdsSoundClip, 3f);
                yield return new WaitForSeconds(ambientBirdsSoundClip.length);
            }
        }

        private IEnumerator DoBlackbird()
        {
            while (true)
            {
                float blackbirdPause = Random.Range(20, 40);
                yield return new WaitForSeconds(blackbirdPause);
                AudioClip clip = blackbirdSoundClips[Random.Range(0, blackbirdSoundClips.Count())];
                oneShotSource.PlayOneShot(clip, .2f);
            }
        }

        private AudioSource ambientSource;
        private AudioSource oneShotSource;
        private Light directionalLight;
    }
}
