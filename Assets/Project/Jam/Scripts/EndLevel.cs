using DG.Tweening;
using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        [Injected]
        public GameOver gameOver;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            gameOver = context.Get<GameOver>(this);
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

            seq.Insert(0f, bg.DOFade(1, 0.5f));
            seq.Insert(0.4f, title.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

            return seq;
        }

        public void PositiveLevelEnding()
        {
            DOEnter().OnComplete(() =>
            {
                positive.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}