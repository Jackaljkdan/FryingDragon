using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class TimeLimitSlider : MonoBehaviour
    {
        #region Inspector

        public Slider slider;

        [Injected]
        public TimeLimit timeLimit;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            timeLimit = context.Get<TimeLimit>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void LateUpdate()
        {
            slider.value = 1 - timeLimit.elapsedSeconds / (timeLimit.levelSettings.minutes * 60);
        }
    }
}