using JK.Injection;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class GameOver : MonoBehaviour
    {
        #region Inspector

        public UnityEvent onTimeUp = new UnityEvent();
        public UnityEvent onLevelWin = new UnityEvent();

        [Injected]
        public LevelSettings levelSettings;

        [Injected]
        public TimeLimit timeLimit;

        [Injected]
        public Truck truck;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            levelSettings = context.Get<LevelSettings>(this);
            timeLimit = context.Get<TimeLimit>(this);
            truck = context.Get<Truck>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            timeLimit.onTimeUp.AddListener(OnTimeUp);
            truck.boxDone.onChange.AddListener(OnBoxDoneChange);
        }

        private void OnDestroy()
        {
            timeLimit.onTimeUp.RemoveListener(OnTimeUp);
            truck.boxDone.onChange.RemoveListener(OnBoxDoneChange);
        }

        private void OnTimeUp()
        {
            onTimeUp.Invoke();
        }

        private void OnBoxDoneChange(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == levelSettings.boxesTodo)
                onLevelWin.Invoke();
        }
    }
}