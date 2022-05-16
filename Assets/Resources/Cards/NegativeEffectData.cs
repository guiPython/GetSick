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
    public string description; // Descri��o da carta
    public float damage; // Dano da carta
    public string[] curedBy; // Rem�dios que curam esse efeito
    public NegativeEffectType type; // Tipo do efeito
    public Lifetime lifetime; // Tipo de dura��o do efeito
    public ushort duration; // Dura��o do efeito
    public ushort numberOfCards = 1; // N�mero de cartas dispon�veis no jogo
}
