using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items.Ingredients
{
    [DisallowMultipleComponent]
    public class IngredientDispenser : ItemDispenser
    {
        #region Inspector



        #endregion

        protected override void InteractProtected(RaycastHit hit)
        {
            if (!holder.heldItem.Value)
                return;

            if (holder.heldItem.Value.TryGetComponent<Bowl>(out Bowl bowl))
                bowl.TryAddIngredient(grabbableItem);
        }
    }
}