using DG.Tweening;
using JK.Injection;
using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class Music : MonoBehaviour
    {
        #region Inspector

        public List<AssetReferenceT<AudioClip>> winClipAssets;
        public List<AssetReferenceT<AudioClip>> loseClipAssets;

        public AudioSource musicAudioSource;
        public AudioSource effectAudioSource;

        [Injected]
        public GameOver gameOver;

        private void Reset()
        {
            musicAudioSource = GetComponents<AudioSource>()[0];
            effectAudioSource = GetComponents<AudioSource>()[1];
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            gameOver = context.Get<GameOver>(this);
        }

        private void Awake()
        {
            Inject();
        }


        private void Start()
        {
            gameOver.onLevelWin.AddListener(OnLevelWin);
            gameOver.onTimeUp.AddListener(OnTimeUp);
        }

        private void OnDestroy()
        {
            gameOver.onLevelWin.RemoveListener(OnLevelWin);
            gameOver.onTimeUp.RemoveListener(OnTimeUp);
        }

        private void OnLevelWin()
        {
            musicAudioSource.DOFade(0, 0.5f);
            effectAudioSource.PlayAssetSafelyAsyncAs(RandomUtils.Choose(winClipAssets), oneShot: true);
        }

        private void OnTimeUp()
        {
            musicAudioSource.DOFade(0, 0.5f);
            effectAudioSource.PlayAssetSafelyAsyncAs(RandomUtils.Choose(loseClipAssets), oneShot: true);
        }
    }
}