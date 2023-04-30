using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class PackageLeft : MonoBehaviour
    {
        #region Inspector

        public ShippingFulfiller fulfiller;

        public TextMeshProUGUI text;

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        #endregion

        private void Start()
        {
            text.text = fulfiller.packagesToShip.ToString();
            fulfiller.packagesShipped.onChange.AddListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<int>.Changed arg)
        {
            text.text = (fulfiller.packagesToShip - arg.updated).ToString();
        }


    }
}