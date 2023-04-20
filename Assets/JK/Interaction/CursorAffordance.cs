using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class CursorAffordance : AbstractAffordance
    {
        #region Inspector

        public bool useCustomColor = false;

        [Conditional(nameof(useCustomColor))]
        public Color customColor = Color.black;

        [SerializeField]
        private string injectionId;

        [Injected]
        public UI.Cursor cursor;

        #endregion

        protected Context context;

        [InjectMethod]
        public virtual void Inject()
        {
            context = Context.Find(this);
            cursor = context.Get<UI.Cursor>(this, injectionId);
        }

        protected virtual void Awake()
        {
            Inject();
        }

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            if (useCustomColor)
                cursor.Show(customColor);
            else
                cursor.Show();
        }

        protected override void StopHighlightProtected()
        {
            cursor.Hide();
        }
    }
}