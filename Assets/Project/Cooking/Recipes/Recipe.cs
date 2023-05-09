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

            return MatchIngredients(other.ingredients);
        }

        public bool MatchIngredients(List<IngredientTypeValue> ingredients)
        {
            if (this.ingredients.Count != ingredients.Count)
                return false;

            return this.ingredients.OrderBy(x => x).SequenceEqual(ingredients.OrderBy(x => x));
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
            foreach (Recipe item in list)
            {
                if (item.MatchIngredients(ingredients))
                {
                    bestMatch = item;
                    return true;
                }
            }

            foreach (Recipe item in list)
            {
                if (item.CanMakeWith(ingredients))
                {
                    bestMatch = item;
                    return true;
                }
            }

            bestMatch = null;
            return false;
        }

        public static bool TryFindBestMatch(Recipe recipe, List<Recipe> list, out Recipe bestMatch)
        {
            return TryFindBestMatch(recipe.ingredients, list, out bestMatch);
        }
    }
}