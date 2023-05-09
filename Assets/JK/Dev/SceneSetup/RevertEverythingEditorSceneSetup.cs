using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Dev.SceneSetup
{
    [DisallowMultipleComponent]
    public class RevertEverythingEditorSceneSetup : AbstractEditorSceneSetup
    {
        #region Inspector

        public List<UnityEngine.Object> excluded;

        #endregion

        protected override void EditorSceneSetupProtected()
        {
            PrefabUtils.RevertEverything(gameObject, excluded);
        }

        public override string GetEditorSceneSetupTitle()
        {
            return $"Revert {name}";
        }
    }
}