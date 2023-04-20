using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.StateMachine
{
    [Serializable]
    public enum UpdateStateResult
    {
        Stay,
        Exit,
    }
}