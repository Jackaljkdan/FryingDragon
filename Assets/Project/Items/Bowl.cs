using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    public class Bowl : MonoBehaviour
    {
        #region Inspector

        public Transform spawnAnchor;
        public Rigidbody rb;

        [RuntimeField]
        public List<GameObject> ingredients = new();

        [Injected]
        public Transform anchorTransform;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            anchorTransform = context.Get<Transform>(this, "dragon.anchor");
        }

        private void Awake()
        {
            Inject();
        }

        private void FixedUpdate()
        {
            rb.MovePosition(anchorTransform.position);
        }

        public void TryAddIngredient(GameObject ingredient)
        {
            ingredients.Add(ingredient);
            Instantiate(ingredient, spawnAnchor.position, spawnAnchor.rotation.normalized, transform.root);
        }
    }
}