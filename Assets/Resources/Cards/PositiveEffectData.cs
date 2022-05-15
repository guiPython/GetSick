using UnityEngine;

public enum PositiveEffectType
{
    Cure,
    Heal
};

[CreateAssetMenu(fileName = "PositiveEffectData", menuName = "PositiveEffectData")]
public class PositiveEffectData : ScriptableObject
{
    public new string name;
    public string description;
    public float healing;
    public PositiveEffectType type;
    public Lifetime lifetime;
    public ushort duration;
    public ushort numberOfCards = 1;
}
