using JK.Injection;
using JK.Observables;
using JK.Utils;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Tutorial
{
    [DisallowMultipleComponent]
    public class Level2Tutorial : MonoBehaviour
    {
        #region Inspector

        public float initialDelaySeconds = 2.5f;

        [Injected]
        public TutorialPopup popup;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            popup = context.Get<TutorialPopup>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            Invoke(nameof(Welcome), initialDelaySeconds);

            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.isSleeping.onChange.AddListener(OnSleeping);
        }

        private void OnDestroy()
        {
            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.isSleeping.onChange.RemoveListener(OnSleeping);
        }

        private void Welcome()
        {
            popup.Show("Business is boomin'! We got more eggs and workers, let's ship 'em.", autoHide: true);
        }

        private void OnSleeping(ObservableProperty<bool>.Changed arg)
        {
            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.isSleeping.onChange.RemoveListener(OnSleeping);

            popup.Show("Our workers are napping, again! We can't afford coffee, release your temper on them to wake them up. Make you sure you're not carrying a bowl, press G to drop it if you need to.", autoHide: true);
        }
    }
}