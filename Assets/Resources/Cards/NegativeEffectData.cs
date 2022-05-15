using UnityEngine;

public enum NegativeEffectType
{
    Damage
};

[CreateAssetMenu(fileName = "NegativeEffectData", menuName = "NegativeEffectData")]
public class NegativeEffectData : ScriptableObject
{
    public new string name;
    public string description;
    public float damage;
    public string[] curedBy;
    public NegativeEffectType type;
    public Lifetime lifetime;
    public ushort duration;
    public ushort numberOfCards = 1;
}
