using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor
{
    public static class AudioSourceContextMenus
    {
        [MenuItem("CONTEXT/AudioSource/Make 3D")]
        public static void Make3D(MenuCommand command)
        {
            AudioSource audioSource = (AudioSource)command.context;
            audioSource.spatialBlend = 1;
            UndoUtils.SetDirty(audioSource);
        }
    }
}