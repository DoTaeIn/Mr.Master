using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItem : MonoBehaviour
{
    [SerializeField] private BerryObj berryObj;
    [SerializeField] private Image berryImg;
    [SerializeField] private TMP_Text berryText;
    private OrderManager orderManager;

    private void Awake()
    {
        orderManager = FindFirstObjectByType<OrderManager>();
        berryImg.sprite = berryObj.sprite;
        berryText.SetText(berryObj.price.ToString());
    }

    public void OnClick()
    {
        if(orderManager == null)
            orderManager = FindFirstObjectByType<OrderManager>();

        if (orderManager.Berrycart.TryAdd(berryObj, orderManager.Berrycart[berryObj]++))
        {
            orderManager.Berrycart[berryObj] = 1;
        }

    }
}
