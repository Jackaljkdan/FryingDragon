using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.StateMachine
{
    [Serializable]
    public abstract class AbstractState
    {
        public Func<AbstractState> GetNextState { get; set; }

        // servono?
        //public UnityEvent onEnter = new UnityEvent();
        //public UnityEvent onExit = new UnityEvent();

        public virtual void EnterState()
        {
            //onEnter.Invoke();
        }

        public abstract UpdateStateResult UpdateState();

        public virtual void ExitState()
        {
            //onEnter.Invoke();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}