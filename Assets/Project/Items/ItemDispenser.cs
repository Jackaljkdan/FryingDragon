using JK.Injection;
using JK.Interaction;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    public class ItemDispenser : AbstractInteractable
    {
        #region Inspector

        public GameObject grabbableItem;

        [Injected]
        public DragonItemHolder holder;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            holder = context.Get<DragonItemHolder>();
        }

        private void Awake()
        {
            Inject();
        }

        #endregion
        protected override void InteractProtected(RaycastHit hit)
        {
            holder.TryAddItem(grabbableItem);
        }
    }
}