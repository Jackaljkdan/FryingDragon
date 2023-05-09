using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Dev.SceneSetup
{
    [DisallowMultipleComponent]
    public abstract class AbstractEditorSceneSetup : MonoBehaviour, IEditorSceneSetup
    {
        #region Inspector

        public bool disableForDebugging = false;

        [ContextMenu("Run Setup")]
        private void RunSetupInEditMode()
        {
            if (!Application.isPlaying)
                EditorSceneSetupProtected();
        }

        #endregion

        public void EditorSceneSetup()
        {
            if (!enabled)
                return;

            if (disableForDebugging)
            {
                Debug.LogWarning($"{name} editor scene setup is disabled for debugging");
                return;
            }

            EditorSceneSetupProtected();
        }

        protected abstract void EditorSceneSetupProtected();

        public virtual string GetEditorSceneSetupTitle()
        {
            return null;
        }

        public virtual string GetEditorSceneSetupDescription()
        {
            return null;
        }
    }
}