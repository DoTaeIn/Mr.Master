using System.Collections.Generic;
using UnityEngine;

public class Cocktail : Drink
{
    // Amount of Drink & Type of Drink
    private Dictionary<float, Drink> drinks;

    // Constructor
    public Cocktail(int id, string name, float price, float proof, float amount, List<string> tastes, Color color, Dictionary<float, Drink> drinks)
        : base(id, name, price, proof, amount, color, tastes)
    {
        this.drinks = drinks ?? new Dictionary<float, Drink>();
    }

    public Dictionary<float, Drink> Drinks
    {
        get => drinks;
        set => drinks = value;
    }
}