using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink 
{
    //DRINK ID
    private int id; 
    //DRINK NAMAE
    private string name;
    //DRINK PRICE PER GLASS
    private float price;
    //DRINK ALCOHOL PERCENTAGE
    private float proof;
    //DRINK AMOUNT LEFT
    private float amount;
    //DRINK COLOR
    private Color color;
    //DRINK TASTES
    private List<string> tastes;

    // Constructor
    public Drink(int id, string name, float price, float proof, float amount, Color color, List<string> tastes)
    {
        this.id = id;
        this.name = name;
        this.price = price;
        this.proof = proof;
        this.amount = amount;
        this.color = color;
        this.tastes = tastes;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }
    public string Name { get => name; set => name = value; }
    public float Price { get => price; set => price = value; }
    public float Proof { get => proof; set => proof = value; }

    public float Amount
    {
        get => amount; set => amount = value;
    }
    public Color Color { get => color; set => color = value; }
    public List<string> Tastes { get => tastes; set => tastes = value; }
    
    
    
    
}
