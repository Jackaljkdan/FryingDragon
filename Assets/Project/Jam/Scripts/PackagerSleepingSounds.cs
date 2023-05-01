using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class PackagerSleepingSounds : MonoBehaviour
    {
        #region Inspector

        public Packager packager;

        public AudioSource sleepAudioSource;

        private void Reset()
        {
            packager = GetComponent<Packager>();
        }

        #endregion

        private void OnEnable()
        {
            packager.isSleeping.onChange.AddListener(OnSleepingChange);
            OnSleepingChange(new ObservableProperty<bool>.Changed() { updated = packager.isSleeping.Value });
        }

        private void OnDisable()
        {
            packager.isSleeping.onChange.RemoveListener(OnSleepingChange);
        }

        private void OnSleepingChange(ObservableProperty<bool>.Changed arg)
        {
            sleepAudioSource.loop = true;

            if (arg.updated)
                sleepAudioSource.Play();
            else
                sleepAudioSource.Stop();
        }
    }
}