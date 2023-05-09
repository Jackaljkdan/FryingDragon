using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Dev.SceneSetup
{
    public interface IEditorSceneSetup
    {
        void EditorSceneSetup();

        string GetEditorSceneSetupTitle()
        {
            return null;
        }

        string GetEditorSceneSetupDescription()
        {
            return null;
        }
    }
}