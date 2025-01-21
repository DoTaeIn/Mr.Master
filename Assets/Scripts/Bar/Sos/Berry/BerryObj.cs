using UnityEngine;

[CreateAssetMenu(fileName = "New Berry", menuName = "Shopping/Berry")]
public class BerryObj : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public Color color;
    public float price;
}
