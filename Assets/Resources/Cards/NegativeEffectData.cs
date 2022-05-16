using UnityEngine;

/// <summary>
/// Enum para os tipos de efeito negativo
/// </summary>
public enum NegativeEffectType
{
    Damage
};

/// <summary>
/// ScriptableObject para cartas com efeitos negativos
/// </summary>
[CreateAssetMenu(fileName = "NegativeEffectData", menuName = "NegativeEffectData")]
public class NegativeEffectData : ScriptableObject
{
    public new string name; // Nome da carta
    public string description; // Descrição da carta
    public float damage; // Dano da carta
    public string[] curedBy; // Remédios que curam esse efeito
    public NegativeEffectType type; // Tipo do efeito
    public Lifetime lifetime; // Tipo de duração do efeito
    public ushort duration; // Duração do efeito
    public ushort numberOfCards = 1; // Número de cartas disponíveis no jogo
}
