using DG.Tweening;
using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class EndLevel : MonoBehaviour
    {
        #region Inspector

        public Image bg;
        public TextMeshProUGUI title;
        public TextMeshProUGUI positive;
        public TextMeshProUGUI negative;

        public GameObject restartObject;

        public GameObject win;

        [Injected]
        public GameOver gameOver;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            gameOver = context.Get<GameOver>(this);
        }

        [ContextMenu("LoadNextScene")]
        private void LoadNextSceneFromInspector()
        {
            LoadNextScene();
        }

        #endregion

        private void Awake()
        {
            Inject();
            bg.color = new Color(0, 0, 0, 0);
            title.transform.localScale = Vector3.zero;
            positive.transform.localScale = Vector3.zero;
            negative.transform.localScale = Vector3.zero;
            restartObject.transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            gameOver.onTimeUp.AddListener(NegativeLevelEnding);
            gameOver.onLevelWin.AddListener(PositiveLevelEnding);
        }

        private void OnDestroy()
        {
            gameOver.onTimeUp.RemoveListener(NegativeLevelEnding);
            gameOver.onLevelWin.RemoveListener(PositiveLevelEnding);
        }

        private Tween DOEnter()
        {
            Sequence seq = DOTween.Sequence();

            seq.Insert(0f, bg.DOFade(0.9f, 0.5f));
            seq.Insert(0.4f, title.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

            return seq;
        }

        public void PositiveLevelEnding()
        {
            DOEnter().OnComplete(() =>
            {
                positive.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    Invoke(nameof(LoadNextScene), 2f);
                });
            });
        }

        public void NegativeLevelEnding()
        {
            DOEnter().OnComplete(() =>
            {
                negative.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    restartObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
                });
            });
        }
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HideAllText()
        {
            restartObject.transform.localScale = Vector3.zero;
            win.transform.localScale = Vector3.zero;
            negative.transform.DOScale(0f, 0.5f);
            positive.transform.DOScale(0f, 0.5f);
            title.transform.DOScale(0f, 0.5f);
        }

        private void StartOver()
        {
            HideAllText();
            win.SetActive(true);
            bg.DOFade(0.9f, 0.5f).OnComplete(() => { win.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack); });

            Invoke(nameof(LoadFirstScene), 12f);
        }

        private void LoadFirstScene()
        {
            SceneManager.LoadSceneAsync(0);
        }

        private void LoadNextScene()
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            else
                StartOver();
        }
    }
}