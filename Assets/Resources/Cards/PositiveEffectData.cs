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
    public string description; // Descri��o da carta
    public float healing; // Regenera��o da carta
    public PositiveEffectType type; // Tipo do efeito
    public Lifetime lifetime; // Tipo de dura��o do efeito
    public ushort duration; // Dura��o do efeito
    public ushort numberOfCards = 1; // N�mero de cartas dispon�veis no jogo
}
