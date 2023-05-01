using JK.Injection;
using JK.Interaction;
using JK.Observables;
using JK.Utils;
using Project.Dragon;
using Project.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Tutorial
{
    [DisallowMultipleComponent]
    public class Level1Tutorial : MonoBehaviour
    {
        #region Inspector

        public float initialDelaySeconds = 5;

        [Injected]
        public TutorialPopup popup;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [Injected]
        public Truck truck;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            popup = context.Get<TutorialPopup>(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
            truck = context.Get<Truck>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            Invoke(nameof(Welcome), initialDelaySeconds);
            dragonItemHolder.heldItem.onChange.AddListener(OnHeldItemChange);

            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.onMakeBox.AddListener(OnMakeBox);

            truck.boxDone.onChange.AddListener(OnBoxDone);
        }

        private void OnDestroy()
        {
            dragonItemHolder.heldItem.onChange.RemoveListener(OnHeldItemChange);

            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.onMakeBox.RemoveListener(OnMakeBox);

            truck.boxDone.onChange.RemoveListener(OnBoxDone);
        }

        List<UnityAction> listeners = new List<UnityAction>();

        private void Welcome()
        {
            popup.Show("Welcome to the Dragon Egg Delivery Service warehouse!\nThis is where you will ship the eggs requested by our clients.\n\nClick to continue...", autoHide: false);
            listeners.Add(popup.onHidden.AddListenerOnce(MovementTutorial));
        }

        private void MovementTutorial()
        {
            popup.Show("Move with WASD and look around with the mouse. Why do you like the cursor so much that you always look at it? Anyway... try now!", autoHide: false);
            listeners.Add(popup.onHidden.AddListenerOnce(BowlTutorial));
        }

        private void BowlTutorial()
        {
            popup.Show("Good! In order to gather the eggs you need a bowl, take one from the table below this popup. Just get close and look at it then left click or press E.", autoHide: true);
        }

        private void OnHeldItemChange(ObservableProperty<GameObject>.Changed arg)
        {
            CancelInvoke(nameof(Welcome));
            foreach (var listener in listeners)
                popup.onHidden.RemoveListener(listener);

            dragonItemHolder.heldItem.onChange.RemoveListener(OnHeldItemChange);

            popup.Show("Great! The egg orders appear on the top right. Take the corresponding eggs from the dispensers on the left. Each order on the top right will show if you are carrying the correct eggs for it.", autoHide: true);
            listeners.Add(popup.onHidden.AddListenerOnce(BoxTutorial));
        }

        private void BoxTutorial()
        {
            popup.Show("When you have the eggs for an order take them to one of the workers on the right. They will make a box so we can ship. You can run by holding Shift, but be careful not to drop anything! We're not in the omelette business.", autoHide: true);
        }

        private void OnMakeBox()
        {
            foreach (var listener in listeners)
                popup.onHidden.RemoveListener(listener);

            dragonItemHolder.heldItem.onChange.RemoveListener(OnHeldItemChange);

            foreach (var packager in transform.root.GetComponentsInChildren<Packager>())
                packager.onMakeBox.RemoveListener(OnMakeBox);

            popup.Show("Ok now take the box to the the truck on the top left. And be careful! We know your temper, we don't want you running around like a mad dragon lighting everything on fire like the last time.", autoHide: true);
        }

        private void OnBoxDone(ObservableProperty<int>.Changed arg)
        {
            popup.Show("Wonderful! You can take more eggs now on the same bowl, but if you want you can drop it by pressing G. Like gone. Get it? So intuitive.", autoHide: true);
        }
    }
}