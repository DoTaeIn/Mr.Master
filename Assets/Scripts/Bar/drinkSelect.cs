using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class drinkSelect: MonoBehaviour
{
    public TMP_Text drinkNameTxt;
    public TMP_Text proofTxt;
    public TMP_Text tasteTxt;
    public TMP_Text priceTxt;
    public TMP_Text leftTxt;
    
    public DrinkSO currDrink;
    private string drinkName;
    private float proof;
    private List<string> taste;
    private float price;
    private float left;

    UIManager uIManager;


    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    public void setTxt(DrinkSO drink)
    {
        this.name = drink.name;
        this.proof = drink.proof;
        this.taste = drink.tastes;
        this.price = drink.price;
        this.left = drink.amount;
        
        
        drinkNameTxt.text = name;
        if(proof == 0)
            proofTxt.SetText("NOT ALCOHOL");
        else
            proofTxt.text = proof + "%";
        
        string temp = "";
        for (int i = 0; i < taste.Count; i++)
        {
            if (i != taste.Count - 1)
                temp += taste[i]+", ";
            else
                temp += taste[i];
        }
        tasteTxt.text = temp;
        
        priceTxt.text = "$" + price.ToString();
        leftTxt.text = left.ToString() + " / " + drink.MAXamount.ToString() + "ml";
    }

    public void onClick()
    {
        uIManager.isMeasureOpen = true;
        uIManager.selectedDrink = currDrink;
        uIManager.setMeausrePanel(name, left.ToString());
    }
}