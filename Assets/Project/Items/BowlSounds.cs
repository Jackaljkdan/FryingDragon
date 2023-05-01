using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    public class BowlSounds : MonoBehaviour
    {
        #region Inspector

        public AudioSource takeAudioSource;
        public AudioSource putAudioSource;

        #endregion

        public void PlayTake()
        {
            takeAudioSource.Play();
        }

        public void PlayPut()
        {
            putAudioSource.Play();
        }
    }
}