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

            public TypeCount WithIncreasedCount(int value, bool asMine)
            {
                TypeCount copy = new TypeCount()
                {
                    type = type,
                    myCount = myCount,
                    theirCount = theirCount,
                };

                if (asMine)
                    copy.myCount += value;
                else
                    copy.theirCount += value;

                return copy;
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

        public override int GetHashCode()
        {
            // TODO: hash sensato
            return 0;
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

                count[index] = count[index].WithIncreasedCount(1, asMine);
            }
        }

        public static bool TryFindBestMatch(List<IngredientTypeValue> ingredients, List<Recipe> list, out Recipe bestMatch)
        {
            return TryFindBestMatch(new Recipe(ingredients), list, out bestMatch);
        }

        public static bool TryFindBestMatch(Recipe recipe, List<Recipe> list, out Recipe bestMatch)
        {
            var index = list.IndexOf(recipe);

            if (index >= 0)
            {
                bestMatch = list[index];
                return true;
            }

            foreach (var el in list)
            {
                if (el.CanMakeWith(recipe.ingredients))
                {
                    bestMatch = el;
                    return true;
                }
            }

            bestMatch = null;
            return false;
        }
    }
}