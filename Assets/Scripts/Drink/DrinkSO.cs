using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDrink", menuName = "Drink/DrinkData")]
public class DrinkSO : ScriptableObject
{
    //DRINK ID
    public int id; 
    //DRINK NAMAE
    public string name;
    //DRINK PRICE PER GLASS
    public float price;
    //DRINK ALCOHOL PERCENTAGE
    public float proof;
    //DRINK AMOUNT LEFT
    public float amount;
    //DRINK COLOR
    public Color color;
    //DRINK TASTES
    public List<string> tastes;

    public bool isAllUsed;
}
