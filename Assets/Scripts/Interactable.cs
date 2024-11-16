using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    Light,
    Door,
    NPC,
    Sign,
    Drink
}

public class Interactable
{
    private int id;
    private string name;
    private string nickName;
    private bool isInteractable;
    private InteractableType type;
    private List<string> interactLines;
    private Drink drink;
    private Cocktail cocktail;

    public Interactable(int id, string name, string nickName, bool isInteractable, InteractableType type)
    {
        this.id = id;
        this.name = name;
        this.nickName = nickName;
        this.isInteractable = isInteractable;
        this.type = type;
    }
    
    public Interactable(int id, string name, string nickName, bool isInteractable, InteractableType type, List<string> interactLines)
    {
        this.id = id;
        this.name = name;
        this.nickName = nickName;
        this.isInteractable = isInteractable;
        this.type = type;
        this.interactLines = interactLines;
    }
    
    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string NickName { get => nickName; set => nickName = value; }
    public bool IsInteractable { get => isInteractable; set => isInteractable = value; }
    public InteractableType Type { get => type; set => type = value; }
    public List<string> InteractLines { get => interactLines; set => interactLines = value; }
    

}

