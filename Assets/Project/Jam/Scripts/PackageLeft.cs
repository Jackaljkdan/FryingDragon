using JK.Observables;
using JK.Utils;
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

        public LevelSettings levelSettings;
        public Truck truck;

        public TextMeshProUGUI text;

        [DebugField]
        public int boxesToShip = 0;

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        private void Start()
        {
            boxesToShip = levelSettings.boxesTodo;
            text.text = boxesToShip.ToString();
            truck.boxDone.onChange.AddListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<int>.Changed arg)
        {
            text.text = (boxesToShip - arg.updated).ToString();
        }


    }
}