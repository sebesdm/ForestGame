using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ILevelEffects
    {
        void SetAudioSources(AudioSource ambientSource, AudioSource oneShotSource);
        void SetDirectionalLight(Light directionalLight);
        void StartAmbient();
    }
}
