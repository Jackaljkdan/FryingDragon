using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace JK.Editor.Shortcuts
{
    [InitializeOnLoad]
    public static class PauseGameShortcut
    {
        public static EditorKey pauseKey = new(KeyCode.P);

        static PauseGameShortcut()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            // TODO: rimettere in play non funziona
            if ((EditorApplication.isPlaying || EditorApplication.isPaused) && ShouldTogglePause())
                EditorApplication.isPaused = !EditorApplication.isPaused;
        }

        private static bool ShouldTogglePause()
        {
            //Debug.Log($"{pauseKey.IsDown() && InputUtils.GetAnyShift() && InputUtils.GetAnyControl()}");
            return pauseKey.IsDown() && InputUtils.GetAnyShift() && InputUtils.GetAnyControl();
        }
    }
}