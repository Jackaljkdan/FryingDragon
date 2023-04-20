using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class QualitySettingText : MonoBehaviour
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
            text.text = QualitySettings.GetQualityLevel().ToString();
        }
    }
}