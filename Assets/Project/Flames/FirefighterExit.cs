using JK.Injection;
using JK.Observables;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterExit : MonoBehaviour
    {
        #region Inspector

        public FirefighterInput input;

        public FirefighterMovement movement;

        [Injected]
        public FlammableList flammableList;

        [Injected]
        public DragonStress dragonStress;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            flammableList = context.Get<FlammableList>(this);
            dragonStress = context.Get<DragonStress>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            flammableList.fires.onChange.AddListener(OnFiresChange);
        }

        private void OnDestroy()
        {
            flammableList.fires.onChange.RemoveListener(OnFiresChange);
        }

        private void OnFiresChange(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 0 && !dragonStress.isInFrenzy)
            {
                flammableList.fires.onChange.RemoveListener(OnFiresChange);
                input.enabled = false;
                Destroy(gameObject);
                //StartCoroutine(ExitCoroutine());
            }
        }

        private IEnumerator ExitCoroutine()
        {
            while (true)
            {
                movement.Move(Vector2.up);
                yield return null;
            }
        }
    }
}