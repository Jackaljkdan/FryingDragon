using DG.Tweening;
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

        public int leaveEveryN = 2;

        public float leaveDistance = 20f;

        public float leaveSeconds = 5f;

        [RuntimeField]
        public ObservableProperty<int> boxDone = new ObservableProperty<int>();

        [RuntimeField]
        public int boxesSinceLeft;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [ContextMenu("Leave")]
        private void LeaveInEditMode()
        {
            if (Application.isPlaying)
                DOLeave();
        }

        #endregion

        private Vector3 initialPosition;

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
            boxesSinceLeft = 0;
            initialPosition = transform.position;
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (dragonItemHolder.heldItem.Value == null)
                return;

            if (!dragonItemHolder.heldItem.Value.TryGetComponent(out Bowl bowl))
                return;

            Ingredient box = bowl.ingredients.FirstOrDefault(ingredient => ingredient.ingredientTypeValue == IngredientTypeValue.box);

            if (box == null)
                return;

            bowl.RemoveIngredient(box);
            Destroy(box.gameObject);
            boxDone.Value++;

            boxesSinceLeft++;

            if (boxesSinceLeft >= leaveEveryN)
            {
                boxesSinceLeft = 0;
                DOLeave();
            }
        }

        public Tween DOLeave()
        {
            Transform myTransform = transform;

            var seq = DOTween.Sequence();

            seq.Append(myTransform.DOMove(myTransform.TransformPoint(0, 0, leaveDistance), leaveSeconds / 2));
            seq.Append(myTransform.DOMove(initialPosition, leaveSeconds / 2));

            seq.SetEase(Ease.InOutQuad);

            return seq;
        }
    }
}