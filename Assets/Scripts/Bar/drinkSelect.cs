using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class drinkSelect: MonoBehaviour
{
    [Header("Drink Selection")]
    [SerializeField] private TMP_Text drinkNameTxt;
    [SerializeField] private TMP_Text proofTxt;
    [SerializeField] private TMP_Text tasteTxt;
    [SerializeField] private TMP_Text priceTxt;
    [SerializeField] private TMP_Text leftTxt;
    public DrinkSO currDrink;
    
    
    [Header("Ordering")]
    private bool isShop;
    
    
    OrderManager orderManager;
    UIManager uIManager;
    

    private void Awake()
    {
        uIManager = FindFirstObjectByType<UIManager>();
        orderManager = FindFirstObjectByType<OrderManager>();
    }

    public void setTxt(bool isShop)
    {
        drinkNameTxt.text = name;
        if(currDrink.proof == 0)
            proofTxt.SetText("NOT ALCOHOL");
        else
            proofTxt.text = currDrink.proof + "%";
        
        string temp = "";
        for (int i = 0; i < currDrink.tastes.Count; i++)
        {
            if (i != currDrink.tastes.Count - 1)
                temp += currDrink.tastes[i]+", ";
            else
                temp += currDrink.tastes[i];
        }
        tasteTxt.text = temp;
        
        priceTxt.text = "$" + currDrink.price.ToString();
        
        if(isShop)
            leftTxt.text = currDrink.MAXamount.ToString() + "ml";
        else
            leftTxt.text = currDrink.amount.ToString() + " / " + currDrink.MAXamount.ToString() + "ml";
    }

    public void onClick()
    {
        if (isShop)
        {
            if(orderManager == null)
                orderManager = FindFirstObjectByType<OrderManager>();
        
            if(!orderManager.cart.TryAdd(currDrink, 1))
                orderManager.cart[currDrink] += 1;
        }
        else
        {
            uIManager.isMeasureOpen = true;
            uIManager.selectedDrink = currDrink;
            uIManager.setMeausrePanel(name, currDrink.amount.ToString());
        }
    }
}