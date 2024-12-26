using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public Cocktail currCocktail;

    private void Update()
    {
        if (currCocktail != null)
        {
            Debug.Log(currCocktail.Name);

            string temp = "";

            for (int i = 0; i < currCocktail.Drinks.Count; i++)
            {
                temp += currCocktail.Drinks[i].Name + ", ";
            }
        
            Debug.Log(temp);
        }
    }
}
