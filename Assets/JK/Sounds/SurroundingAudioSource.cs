using JK.Injection;
using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace JK.Sounds
{
    [DisallowMultipleComponent]
    public class SurroundingAudioSource : MonoBehaviour
    {
        #region Inspector

        public Transform sourceTransform;

        public float minSecondsBetweenSounds = 0;
        public float maxSecondsBetweenSounds = 1;

        public List<AssetReferenceT<AudioClip>> clipAssets;

        public AudioSource audioSource;

        public UnityEvent onPlay;

        [SerializeField]
        private List<Path> paths;

        [Injected]
        private Transform playerTransform;

        [Injected]
        public Context context;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        [InjectMethod]
        public virtual void Inject()
        {
            context = Context.Find(this);
            playerTransform = context.Get<Transform>(this, "player");

        }

        private void Awake()
        {
            Inject();
        }

        [Serializable]
        struct Path
        {
            public Transform start;
            public Transform end;
        }

        private Coroutine coroutine;

        private void OnEnable()
        {
            coroutine = StartCoroutine(PlaySoundsCoroutine());
        }

        private void OnDisable()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        private void SelectStartAndEnd(out Transform start, out Transform end)
        {
            int randomPathIndex = UnityEngine.Random.Range(0, paths.Count);
            var path = paths[randomPathIndex];

            int random = UnityEngine.Random.Range(0, 2);
            start = random == 0 ? path.start : path.end;
            end = random == 0 ? path.end : path.start;
        }

        protected virtual float RandomizeWaitSeconds()
        {
            return UnityEngine.Random.Range(minSecondsBetweenSounds, maxSecondsBetweenSounds);
        }

        protected virtual IEnumerator PlaySoundsCoroutine()
        {

            while (true)
            {
                yield return new WaitForSeconds(RandomizeWaitSeconds());

                SelectStartAndEnd(out Transform start, out Transform end);

                var operation = RandomUtils.Choose(clipAssets).LoadAssetAsyncIfNecessary();

                yield return operation.WaitUntilDone();

                transform.position = playerTransform.position;

                audioSource.clip = (AudioClip)operation.Result;
                audioSource.time = 0;
                audioSource.Play();
                onPlay.Invoke();

                while (audioSource.isPlaying)
                {
                    sourceTransform.position = Vector3.Lerp(start.position, end.position, audioSource.time / audioSource.clip.length);
                    yield return null;
                }
            }
        }
    }
}