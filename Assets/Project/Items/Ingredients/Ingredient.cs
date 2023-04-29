using Project.Items.Ingredients;
using Project.Jam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    #region Inspector

    public IngredientTypeValue ingredientTypeValue;
    public GameObject brokenIngredient;

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponentInParent<Ground>())
            return;

        if (!brokenIngredient)
            return;
        Instantiate(brokenIngredient, transform.position, brokenIngredient.transform.rotation, transform.root);
        Destroy(gameObject);
    }

}
