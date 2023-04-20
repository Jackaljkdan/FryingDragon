using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.Scriptables
{
    [CreateAssetMenu(fileName = "CompletedScriptable", menuName = "JK/Scriptables/CompletedScriptable")]
    public class CompletedScriptable : ScriptableObject
    {
        #region Inspector fields

        public bool completed;

        #endregion
    }
}