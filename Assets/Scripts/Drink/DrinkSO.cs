using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrinkType
{
    Alcohol,
    Syrup
}

[CreateAssetMenu(fileName = "New Drink", menuName = "Drink/DrinkData")]
public class DrinkSO : ScriptableObject
{
    //DRINK ID
    public int id; 
    //DRINK NAMAE
    public string name;
    //DRINK TYPE
    public DrinkType type;
    //DRINK PRICE PER GLASS
    public float price;
    //DRINK ALCOHOL PERCENTAGE
    public float proof;
    //DRINK AMOUNT LEFT
    public float amount;
    //DRUNK's MAX AMOUNT
    public float MAXamount;
    //DRINK COLOR
    public Color color;
    //DRINK TASTES
    public List<string> tastes;
}
