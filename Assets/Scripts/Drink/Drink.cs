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

    public int getId() { return id; }
    public string getName() { return name; }
    public float getPrice() { return price; }
    public float getProof() { return proof; }
    public float getAmount() { return amount; }
    public Color getColor() { return color; }
    public List<string> getTastes() { return tastes; }
    
    
    
    
}
