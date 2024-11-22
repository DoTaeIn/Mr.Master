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

    public void setTxt(string name, float proof, List<string> taste, float price, float left)
    {
        this.name = name;
        this.proof = proof;
        this.taste = taste;
        this.price = price;
        this.left = left;
        
        
        drinkNameTxt.text = name;
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
        leftTxt.text = left.ToString() + "/ 1000mL";
    }

    public void onClick()
    {
        uIManager.isMeasureOpen = true;
        uIManager.selectedDrink = currDrink;
        uIManager.setMeausrePanel(name, left.ToString());
    }
}