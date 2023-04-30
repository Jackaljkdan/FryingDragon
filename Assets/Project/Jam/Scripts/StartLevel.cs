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
    public class StartLevel : MonoBehaviour
    {
        #region Inspector

        public Image image;
        public TextMeshProUGUI text;


        [Injected]
        public LevelSettings levelSetting;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            levelSetting = context.Get<LevelSettings>(this);
        }

        private void Reset()
        {
            image = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion
        private void Awake()
        {
            Inject();
            text.text = $"Day {levelSetting.level}";
        }

        private void Start()
        {
            Invoke(nameof(DoFade), 2f);
        }

        private void DoFade()
        {
            text.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
            image.DOFade(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));
        }
    }
}