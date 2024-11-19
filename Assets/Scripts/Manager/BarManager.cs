using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D barbehind;
    [SerializeField] private PolygonCollider2D barfont;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private CinemachineVirtualCamera player;



    public void toBehind()
    {
        player.Follow = null;
        confiner.m_BoundingShape2D = barbehind;
    }

    public void toFont()
    {
        player.Follow = FindObjectOfType<PlayerCTRL>().gameObject.transform;
        confiner.m_BoundingShape2D = barfont;
    }
}
