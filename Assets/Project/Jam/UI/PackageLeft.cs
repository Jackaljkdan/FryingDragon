using DG.Tweening;
using JK.Injection;
using JK.Observables;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class PackageLeft : MonoBehaviour
    {
        #region Inspector

        public TextMeshProUGUI text;

        public float animationDuration = 1f;
        public float scaleMultiplier = 1.5f;
        public Color targetColor = Color.green;

        public int wiggleCount = 4;
        public float wiggleAngle = 5f;

        public RawImage boxImage;

        [Injected]
        public LevelSettings levelSettings;

        [Injected]
        public Truck truck;

        [DebugField]
        public int boxesToShip = 0;

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        private Color initialTextColor;
        private Vector3 initialTextScale;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            levelSettings = context.Get<LevelSettings>(this);
            truck = context.Get<Truck>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            boxesToShip = levelSettings.boxesTodo;
            text.text = boxesToShip.ToString();
            truck.boxDone.onChange.AddListener(OnStateChanged);
            initialTextColor = text.color;
            initialTextScale = text.transform.localScale;
        }



        private void OnStateChanged(ObservableProperty<int>.Changed arg)
        {
            text.text = (boxesToShip - arg.updated).ToString();

            text.DOColor(targetColor, animationDuration / 2)
                .OnComplete(() => text.DOColor(initialTextColor, animationDuration / 2));

            text.transform.DOScale(initialTextScale * scaleMultiplier, animationDuration / 2)
                .OnComplete(() => text.transform.DOScale(initialTextScale, animationDuration / 2));

            Sequence wiggleSequence = DOTween.Sequence();
            for (int i = 0; i < wiggleCount; i++)
            {
                float angle = i % 2 == 0 ? wiggleAngle : -wiggleAngle;
                wiggleSequence.Append(boxImage.transform.DOLocalRotate(new Vector3(0, 0, angle), animationDuration / (wiggleCount * 2)));
            }
            wiggleSequence.Append(boxImage.transform.DOLocalRotate(Vector3.zero, animationDuration / (wiggleCount * 2)));
            wiggleSequence.Play();
        }

    }
}