using System.Collections.Generic;
using UnityEngine;

public class Cocktail : Drink
{
    // Amount of Drink & Type of Drink
    private Dictionary<float, Drink> drinks;
    private int shakeAmt;

    // Constructor
    public Cocktail(int id, string name, float price, float proof, float amount, List<string> tastes, Color color, Dictionary<float, Drink> drinks, int shakeAmt)
        : base(id, name, price, proof, amount, color, tastes)
    {
        this.drinks = drinks ?? new Dictionary<float, Drink>();
        this.shakeAmt = shakeAmt;
    }

    public Dictionary<float, Drink> Drinks
    {
        get => drinks;
        set => drinks = value;
    }

    public int ShakeAmt
    {
        get => shakeAmt;
        set => shakeAmt = value;
    }
}