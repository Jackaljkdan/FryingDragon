using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterInput : MonoBehaviour
    {
        #region Inspector

        public FirefighterMovement firefighterMovement;

        public ParticleSystem extinguisherParticleSystem;

        #endregion

        private void Update()
        {
            firefighterMovement.Move(new Vector2(
                Input.GetAxis("Horizontal") * 2,
                Input.GetAxis("Vertical") * 2
            ));
        }
    }
}