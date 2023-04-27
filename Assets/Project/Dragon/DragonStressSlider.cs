using DG.Tweening;
using JK.Injection;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonStressSlider : MonoBehaviour
    {
        #region Inspector

        public float tweenSeconds = 0.2f;

        public Slider slider;

        [Injected]
        public DragonStress dragonStress;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonStress = context.Get<DragonStress>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void OnEnable()
        {
            slider.value = dragonStress.stress.Value;
            dragonStress.stress.onChange.AddListener(OnStressChange);
        }

        private void OnDisable()
        {
            dragonStress.stress.onChange.RemoveListener(OnStressChange);
        }

        private void OnStressChange(ObservableProperty<float>.Changed arg)
        {
            slider.DOValue(arg.updated, tweenSeconds);
        }
    }
}