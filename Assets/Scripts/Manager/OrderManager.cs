using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    //In case of Limiting available orders -> Not using yet.
    public List<DrinkSO> availableOrders;
    public Dictionary<DrinkSO, int> Drinkcart;
    
    public List<BerryObj> berries;
    public Dictionary<BerryObj, int> Berrycart;
    
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

        if (Drinkcart.Count > 0)
        {
            foreach (DrinkSO SOS in Drinkcart.Keys)
            {
                cost += SOS.price * Drinkcart[SOS];
            }

            if(player != null)
                player = FindFirstObjectByType<PlayerCTRL>();

            if (player.money >= cost)
            {
                if(storage == null)
                    storage = FindFirstObjectByType<StorageManager>();
                player.money -= cost;


                foreach (DrinkSO SOS in Drinkcart.Keys)
                {
                    if (storage.drinks[SOS] != null)
                    {
                        for (int i = 0; i < Drinkcart[SOS]; i++)
                            storage.drinks[SOS].Add(SOS);
                    }
                    else
                    {
                        storage.drinks[SOS] = new List<DrinkSO>();
                        for (int i = 0; i < Drinkcart[SOS]; i++)
                            storage.drinks[SOS].Add(SOS);
                    }
                }
            }
            else
                Debug.Log("Don't have enough money");
        }
        else if (Berrycart.Count > 0)
        {
            foreach (BerryObj SOS in Berrycart.Keys)
            {
                cost += SOS.price * Berrycart[SOS];
            }

            if(player != null)
                player = FindFirstObjectByType<PlayerCTRL>();

            if (player.money >= cost)
            {
                if(storage == null)
                    storage = FindFirstObjectByType<StorageManager>();
                player.money -= cost;


                foreach (BerryObj SOS in Berrycart.Keys)
                {
                    if (storage.berry[SOS] != null)
                    {
                        for (int i = 0; i < Berrycart[SOS]; i++)
                            storage.berry[SOS].Add(SOS);
                    }
                    else
                    {
                        storage.berry[SOS] = new List<BerryObj>();
                        for (int i = 0; i < Berrycart[SOS]; i++)
                            storage.berry[SOS].Add(SOS);
                    }
                }
            }
            else
                Debug.Log("Don't have enough money");
        }
    }

    public void EmptyOrder()
    {
        Drinkcart.Clear();
    }

}
