using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.Scriptables
{
    [CreateAssetMenu(fileName = "BoolScriptable", menuName = "JK/Scriptables/BoolScriptable")]
    public class BoolScriptable : ScriptableObject
    {
        #region Inspector fields

        public bool value;

        #endregion
    }
}