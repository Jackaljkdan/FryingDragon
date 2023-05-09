using JK.Dev.SceneSetup;
using JK.Injection;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FlammableList : MonoBehaviour, IEditorSceneSetup
    {
        #region Inspector

        public List<Flammable> list;

        public ObservableProperty<int> fires = new ObservableProperty<int>();

        [Injected]
        private SignalBus signalBus;

        private void Reset()
        {
            transform.root.GetComponentsInChildren(list);
        }

        #endregion

        public void EditorSceneSetup()
        {
            Reset();
        }

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            fires.SetSilently(0);
            signalBus.AddListener<FireStartSignal>(OnFireStart);
            signalBus.AddListener<FireStopSignal>(OnFireStop);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<FireStartSignal>(OnFireStart);
            signalBus.RemoveListener<FireStopSignal>(OnFireStop);
        }

        private void OnFireStart(FireStartSignal signal)
        {
            fires.Value += 1;
        }

        private void OnFireStop(FireStopSignal signal)
        {
            fires.Value -= 1;
        }
    }
}