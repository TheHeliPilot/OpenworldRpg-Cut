using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/New Speaker")]
public class Speaker : ScriptableObject
{
    public int index;
    [SerializeField] private string _name;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite spriteOutline;

    public string GetName()
    {
        return _name;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
    public Sprite GetSpriteOutline()
    {
        return spriteOutline;
    }
}