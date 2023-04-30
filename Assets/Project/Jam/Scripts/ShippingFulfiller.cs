using JK.Injection;
using JK.Observables;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class ShippingFulfiller : MonoBehaviour
    {
        #region Inspector

        public int packagesToShip = 10;

        public Truck truck;

        public ObservableProperty<int> packagesShipped = new ObservableProperty<int>();
        #endregion

        private void Start()
        {
            packagesShipped.Value = 0;
            truck.boxDone.onChange.AddListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<int>.Changed arg)
        {
            packagesShipped.Value = arg.updated;

            if (packagesShipped.Value == packagesToShip)
                AllPackagesShipped();
        }

        public void AllPackagesShipped()
        {
            Debug.Log("You won the game, congrats now go back to work pussy. Tu devi allenare!");
        }
    }
}