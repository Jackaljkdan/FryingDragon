using JK.Injection;
using JK.Observables;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterExit : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        public FirefighterInput input;

        public FirefighterMovement movement;

        public NavMeshAgent agent;

        [Injected]
        public FlammableList flammableList;

        [Injected]
        public DragonStress dragonStress;

        [Injected]
        public Transform exitAnchor;

        [ContextMenu("Exit")]
        private void ExitInEditMode()
        {
            if (Application.isPlaying)
                Exit();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            flammableList = context.Get<FlammableList>(this);
            dragonStress = context.Get<DragonStress>(this);
            exitAnchor = context.Get<Transform>(this, "firefighter.exit");
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
                Exit();
        }

        public void Exit()
        {
            flammableList.fires.onChange.RemoveListener(OnFiresChange);
            input.enabled = false;
            input.movement.enabled = false;
            agent.enabled = true;
            agent.destination = exitAnchor.position;
            animator.CrossFade("Move", 0.5f);
            animator.SetFloat("X", 0);
            animator.SetFloat("Z", 1);
            StartCoroutine(ExitCoroutine());
        }

        private IEnumerator ExitCoroutine()
        {
            yield return null;
            yield return null;

            while (agent.remainingDistance > agent.stoppingDistance)
                yield return null;

            Destroy(gameObject);
        }
    }
}