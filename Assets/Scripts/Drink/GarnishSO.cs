using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Garnish", menuName = "Drink/GarnishData")]
public class GarnishSO : ScriptableObject
{
    //Garnish ID
    public int id;
    //Garnish Name
    public string name;
    //Garnish PRICE PER Thing
    public float price;
    //Garnish Image
    public Sprite Image;
    //Garnish TASTES
    public List<string> tastes;
}
