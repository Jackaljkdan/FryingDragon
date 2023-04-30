using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class TimeLimit : MonoBehaviour
    {
        #region Inspector

        public UnityEvent onTimeUp = new UnityEvent();

        [RuntimeField]
        public float elapsedSeconds;

        [Injected]
        public LevelSettings levelSettings;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            levelSettings = context.Get<LevelSettings>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Update()
        {
            elapsedSeconds += Time.deltaTime;

            if (elapsedSeconds >= levelSettings.minutes * 60)
            {
                enabled = false;
                onTimeUp.Invoke();
            }
        }
    }
}