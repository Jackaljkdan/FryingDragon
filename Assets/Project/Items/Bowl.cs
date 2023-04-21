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

        [RuntimeField]
        public List<GameObject> ingredients = new();

        #endregion

        public void TryAddIngredient(GameObject ingredient)
        {
            ingredients.Add(ingredient);
            Instantiate(ingredient, spawnAnchor.position, spawnAnchor.rotation);
        }
    }
}