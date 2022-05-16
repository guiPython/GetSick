using UnityEngine;

/// <summary>
/// Enum para os tipos de efeito positivo
/// </summary>
public enum PositiveEffectType
{
    Cure,
    Heal
};

/// <summary>
/// ScriptableObject para cartas com efeitos negativos
/// </summary>
[CreateAssetMenu(fileName = "PositiveEffectData", menuName = "PositiveEffectData")]
public class PositiveEffectData : ScriptableObject
{
    public new string name; // Nome da carta
    public string description; // Descrição da carta
    public float healing; // Regeneração da carta
    public PositiveEffectType type; // Tipo do efeito
    public Lifetime lifetime; // Tipo de duração do efeito
    public ushort duration; // Duração do efeito
    public ushort numberOfCards = 1; // Número de cartas disponíveis no jogo
}
