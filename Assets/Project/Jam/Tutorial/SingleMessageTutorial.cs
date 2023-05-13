using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Tutorial
{
    [DisallowMultipleComponent]
    public class SingleMessageTutorial : MonoBehaviour
    {
        #region Inspector

        public float initialDelaySeconds = 2.5f;

        public string message;

        public bool showInEditor = true;

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
            if (PlatformUtils.IsEditor && !showInEditor)
                return;

            Invoke(nameof(Welcome), initialDelaySeconds);
        }

        private void Welcome()
        {
            popup.Show(message, autoHide: true);
        }
    }
}