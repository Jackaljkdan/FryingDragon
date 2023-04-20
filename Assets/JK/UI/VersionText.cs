using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class VersionText : MonoBehaviour
    {
        #region Inspector

        public Text text;

        private void Reset()
        {
            text = GetComponent<Text>();
        }

        #endregion

        private void Start()
        {
            text.text = "v" + Application.version;
        }
    }
}