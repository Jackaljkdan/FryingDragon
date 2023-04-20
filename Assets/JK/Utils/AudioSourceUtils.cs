using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class AudioSourceUtils
    {
        public static void PlaySafelyAs(this AudioSource self, AudioClip clip, bool oneShot, float oneShotVolumeScale = 1)
        {
            if (self != null && clip != null)
                self.PlayAs(clip, oneShot, oneShotVolumeScale);
        }

        public static void PlayAs(this AudioSource self, AudioClip clip, bool oneShot, float oneShotVolumeScale = 1)
        {
            if (oneShot)
            {
                self.PlayOneShot(clip, oneShotVolumeScale);
            }
            else
            {
                self.clip = clip;
                self.Play();
            }
        }

        public static AudioClip PlayRandomClip(this AudioSource self, List<AudioClip> clips, bool oneShot, float oneShotVolumeScale = 1)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Count);
            AudioClip randomClip = clips[randomIndex];

            self.PlayAs(randomClip, oneShot, oneShotVolumeScale);

            return randomClip;
        }

        public static IEnumerator FadeCoroutine(this AudioSource self, float to, float seconds)
        {
            float elapsedSeconds = 0;
            float initialVolume = self.volume;

            while (elapsedSeconds < seconds)
            {
                elapsedSeconds += Time.deltaTime;
                self.volume = Mathf.Lerp(initialVolume, to, elapsedSeconds / seconds);
                yield return null;
            }

            self.volume = to;
        }
    }
}