using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Jam.Tutorial
{
    [DisallowMultipleComponent]
    public class TutorialPopup : MonoBehaviour
    {
        #region Inspector

        public float autoHideSeconds = 8;

        public TMP_Text text;

        public CanvasGroup canvasGroup;

        public Button button;

        public UnityEvent onHide = new UnityEvent();
        public UnityEvent onHidden = new UnityEvent();

        private void Reset()
        {
            text = GetComponentInChildren<TMP_Text>();
            canvasGroup = GetComponent<CanvasGroup>();
            button = GetComponent<Button>();
        }

        #endregion

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public void Show(string text, bool autoHide)
        {
            gameObject.SetActive(true);

            transform.DOScale(1, 0.2f).ChangeStartValue(Vector3Utils.Create(0.98f));
            canvasGroup.DOFade(1, 0.2f).ChangeStartValue(0);

            this.text.text = text;

            CancelInvoke(nameof(Hide));

            if (autoHide)
                Invoke(nameof(Hide), autoHideSeconds);
        }

        private void OnClick()
        {
            if (!IsInvoking(nameof(Hide)))
                Hide();
        }

        public void Hide()
        {
            transform.DOScale(0.98f, 0.2f);
            canvasGroup.DOFade(0, 0.2f).OnComplete(onHidden.Invoke);

            onHide.Invoke();
        }
    }
}