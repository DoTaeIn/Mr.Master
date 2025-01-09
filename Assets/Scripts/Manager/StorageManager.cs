using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public Dictionary<DrinkSO, List<DrinkSO>> drinks;


    private void Update()
    {
        
    }

    public void AddDrink(DrinkSO drink)
    {
        drinks[drink].Add(drink);
    }


    public void disturbIntegrity(DrinkSO obj, int index, float amt)
    {
        drinks[obj][index].integrity -= amt;
    }

    public void distrubAllIntegrity(float amt)
    {
        foreach (DrinkSO obj in drinks.Keys)
        {
            disturbIntegrity(obj, 0, amt);
        }
    }

    public List<DrinkSO> checkSpoiled()
    {
        List<DrinkSO> result = new List<DrinkSO>();
        
        foreach (DrinkSO obj in drinks.Keys)
        {
            if (drinks[obj][0].integrity <= 0)
            {
                result.Add(obj);
            }
        }
        
        return result;
    }
}
