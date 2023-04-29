using JK.Injection;
using Project.Dragon;
using Project.Flames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Characters
{
    [DisallowMultipleComponent]
    public class FarmerFear : MonoBehaviour
    {
        #region Inspector

        public FarmerAnimator animator;

        [Injected]
        public DragonStress dragonStress;

        [Injected]
        private SignalBus signalBus;

        private void Reset()
        {
            animator = GetComponent<FarmerAnimator>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonStress = context.Get<DragonStress>(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            signalBus.AddListener<FirefighterExitSignal>(OnFirefighterExit);
            dragonStress.onFrenzy.AddListener(OnFrenzy);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<FirefighterExitSignal>(OnFirefighterExit);
            dragonStress.onFrenzy.RemoveListener(OnFrenzy);
        }

        private void OnFrenzy()
        {
            animator.PlayHorrorLoop();
        }

        private void OnFirefighterExit(FirefighterExitSignal arg)
        {
            animator.PlayIdle();
        }
    }
}