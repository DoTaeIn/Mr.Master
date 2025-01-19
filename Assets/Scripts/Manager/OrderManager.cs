using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    //In case of Limiting available orders -> Not using yet.
    public List<DrinkSO> availableOrders;
    public Dictionary<DrinkSO, int> cart;
    
    UIManager uiManager;
    PlayerCTRL player;
    StorageManager storage;


    public void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        player = FindFirstObjectByType<PlayerCTRL>();
        storage = FindFirstObjectByType<StorageManager>();
    }

    public void AcceptOrder()
    {
        float cost = 0;
        foreach (DrinkSO SOS in cart.Keys)
        {
            cost += SOS.price * cart[SOS];
        }

        if(player != null)
            player = FindFirstObjectByType<PlayerCTRL>();

        if (player.money >= cost)
        {
            if(storage == null)
                storage = FindFirstObjectByType<StorageManager>();
            player.money -= cost;


            foreach (DrinkSO SOS in cart.Keys)
            {
                if (storage.drinks[SOS] != null)
                {
                    for (int i = 0; i < cart[SOS]; i++)
                        storage.drinks[SOS].Add(SOS);
                }
                else
                {
                    storage.drinks[SOS] = new List<DrinkSO>();
                    for (int i = 0; i < cart[SOS]; i++)
                        storage.drinks[SOS].Add(SOS);
                }
            }
        }
        else
            Debug.Log("Don't have enough money");
    }

    public void EmptyOrder()
    {
        cart.Clear();
    }

}
