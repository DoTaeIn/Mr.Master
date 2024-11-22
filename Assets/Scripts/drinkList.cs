using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class drinkList : MonoBehaviour
{
    public DrinkSO drink;
    public int amount_fl;
    public TMP_Text drinkName;
    public TMP_Text amount;

    public void Initiate()
    {
        drinkName.text = drink.name;
        amount.text = amount_fl.ToString() + "ml";
    }
}
