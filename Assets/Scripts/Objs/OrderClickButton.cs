using System;
using TMPro;
using UnityEngine;

public class OrderClickButton : MonoBehaviour
{
    [SerializeField] private DrinkSO orderObj;
    [SerializeField] private TMP_Text drinkAmtTxt;
    private int orderObjAmt;
    
    OrderManager orderManager;


    private void Awake()
    {
        orderManager = FindFirstObjectByType<OrderManager>();
    }


    public void orderClick(int orderAmt)
    {
        if(orderManager == null)
            orderManager = FindFirstObjectByType<OrderManager>();
        
        if(!orderManager.cart.TryAdd(orderObj, orderObjAmt))
            orderManager.cart[orderObj] += orderAmt;
    }
}
