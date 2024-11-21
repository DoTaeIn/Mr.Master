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

    UIManager uIManager;


    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    public void setTxt(string name, float proof, List<string> taste, float price, float left)
    {
        drinkNameTxt.text = name;
        proofTxt.text = proof.ToString() + "%";
        
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
        uIManager.showDrinksPanel = false;
    }
}