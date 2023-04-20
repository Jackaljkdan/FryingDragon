using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor
{
    [Serializable]
    public class EditorKey
    {
        public KeyCode code;

        public float downIntervalSeconds = 1;
        public float lastDownTime;

        public EditorKey(KeyCode code)
        {
            this.code = code;
            Init();
        }

        public EditorKey(KeyCode code, float downIntervalSeconds)
        {
            this.code = code;
            this.downIntervalSeconds = downIntervalSeconds;
            Init();
        }

        private void Init()
        {
            lastDownTime = -downIntervalSeconds;
        }

        public bool IsDown()
        {
            if (!UnityEngine.Input.GetKey(code))
            {
                Init();
                return false;
            }

            if (Time.time - lastDownTime < downIntervalSeconds)
                return false;

            lastDownTime = Time.time;

            return true;
        }
    }
}