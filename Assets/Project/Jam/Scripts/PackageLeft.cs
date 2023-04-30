using JK.Injection;
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

        public TextMeshProUGUI text;

        [Injected]
        public LevelSettings levelSettings;

        [Injected]
        public Truck truck;

        [DebugField]
        public int boxesToShip = 0;

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            levelSettings = context.Get<LevelSettings>(this);
            truck = context.Get<Truck>(this);
        }

        private void Awake()
        {
            Inject();
        }

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