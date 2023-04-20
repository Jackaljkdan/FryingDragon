using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class ActionQueue : AbstractAction
    {
        #region Inspector

        public List<AbstractAction> actions = new List<AbstractAction>();

        protected virtual void Reset()
        {
            ResetActions();
        }

        [ContextMenu("Reset Actions")]
        protected virtual void ResetActions()
        {
            GetComponentsInChildren(actions);
            actions.Remove(this);
            UndoUtils.SetDirty(this);
        }

        #endregion

        public bool TryAction(ActionArgs args, out AbstractAction executedAction)
        {
            foreach (var act in actions)
            {
                if (!act.enabled)
                    continue;

                if (!act.TryAction(args))
                    continue;

                executedAction = act;
                return true;
            }

            executedAction = null;
            return false;
        }

        public override bool TryAction(ActionArgs args)
        {
            return TryAction(args, out _);
        }
    }
}