using JK.Injection;
using JK.Utils;
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

    [RuntimeField]
    public bool isFirstCollision;

    [Injected]
    private SignalBus signalBus;

    #endregion

    [InjectMethod]
    public void Inject()
    {
        Context context = Context.Find(this);
        signalBus = context.Get<SignalBus>(this);
    }

    private void Awake()
    {
        Inject();
    }

    private void Start()
    {
        isFirstCollision = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponentInParent<Ground>())
            return;

        if (isFirstCollision)
        {
            isFirstCollision = false;
            signalBus.Invoke(new IngredientFallenSignal() { ingredient = this });
        }

        if (!brokenIngredient)
            return;

        Instantiate(brokenIngredient, transform.position, brokenIngredient.transform.rotation, transform.root);
        Destroy(gameObject);
    }
}
