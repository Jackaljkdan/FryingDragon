using JK.Injection;
using JK.Interaction;
using JK.Observables;
using JK.Utils;
using Project.Dragon;
using Project.Items;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class Truck : AbstractInteractable
    {
        #region Inspector

        public int boxTodo = 12;

        [RuntimeField]
        public ObservableProperty<int> boxDone = new ObservableProperty<int>();

        [Injected]
        public DragonItemHolder dragonItemHolder;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
        }

        private void Awake()
        {
            Inject();
        }

        protected override void Start()
        {
            base.Start();
            boxDone.SetSilently(0);
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (dragonItemHolder.holdedItem == null)
                return;

            if (!dragonItemHolder.holdedItem.TryGetComponent(out Bowl bowl))
                return;

            Ingredient box = bowl.ingredients.FirstOrDefault(ingredient => ingredient.ingredientTypeValue == IngredientTypeValue.box);

            if (box == null)
                return;

            bowl.RemoveIngredient(box);
            Destroy(box.gameObject);
            boxDone.Value++;
        }
    }
}