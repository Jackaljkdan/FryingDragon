using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace JK.StateMachine
{
    [DisallowMultipleComponent]
    public abstract class AbstractStateMachineBehaviour : MonoBehaviour
    {
        #region Inspector

        [RuntimeHeader]

        [ReadOnly]
        public AbstractState state;

        [ContextMenu("Update state")]
        private void UpdateStateInEditMode()
        {
            if (state == null)
                Start();

            UpdateState();
            Debug.Log("current state: " + state);
        }

        #endregion

        protected virtual void Start()
        {
            SetupTransitions();
            state = GetInitialState();
            state.EnterState();
        }

        public abstract void SetupTransitions();

        public abstract AbstractState GetInitialState();

        private void Update()
        {
            UpdateState();
        }

        public void UpdateState()
        {
            UpdateStateResult result = state.UpdateState();

            if (result == UpdateStateResult.Exit)
            {
                state.ExitState();

                Assert.IsNotNull(state.GetNextState, $"{nameof(state.GetNextState)} of {state} has not been assigned".Contextualized(this, includeHierarchy: true, includeClass: true));

                state = state.GetNextState();

                state.EnterState();
            }
        }
    }
}