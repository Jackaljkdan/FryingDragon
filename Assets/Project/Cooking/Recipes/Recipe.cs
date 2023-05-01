using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [Serializable]
    public class Recipe
    {
        public List<IngredientTypeValue> ingredients;

        private struct TypeCount
        {
            public IngredientTypeValue type;
            public int myCount;
            public int theirCount;

            public void IncreaseCount(int value, bool asMine)
            {
                if (asMine)
                    myCount += value;
                else
                    theirCount += value;
            }
        }

        public Recipe(List<IngredientTypeValue> ingredients)
        {
            this.ingredients = new List<IngredientTypeValue>(ingredients);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (!(obj is Recipe other))
                return false;

            if (ingredients.Count != other.ingredients.Count)
                return false;

            return ingredients.OrderBy(x => x).SequenceEqual(other.ingredients.OrderBy(x => x));
        }

        public Recipe Clone()
        {
            return new Recipe(new List<IngredientTypeValue>(ingredients));
        }

        public bool CanMakeWith(List<IngredientTypeValue> ingredients)
        {
            if (this.ingredients.Count > ingredients.Count)
                return false;

            List<TypeCount> count = new(4);
            Count(this.ingredients, count, asMine: true);
            Count(ingredients, count, asMine: false);

            foreach (var c in count)
                if (c.theirCount < c.myCount)
                    return false;

            return true;
        }

        private void Count(List<IngredientTypeValue> ingredients, List<TypeCount> count, bool asMine)
        {
            foreach (var ing in ingredients)
            {
                int index = count.FindIndex(c => c.type == ing);
                if (index < 0)
                {
                    count.Add(new TypeCount() { type = ing });
                    index = count.Count - 1;
                }

                count[index].IncreaseCount(1, asMine);
            }
        }
    }
}