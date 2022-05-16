using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum para o status dos jogadores
/// </summary>
public enum PlayerStatus
{
    Alive,
    Dead
}

/// <summary>
/// ScriptableObject para os jogadores
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int Number; // Número do jogador
    public new string name; // Nome do jogador
    public float timeRemaining; // Tempo de vida restante do jogador em anos
    public List<Card> cards = new List<Card>(); // Cartas do jogador
    public List<Effect> activeEffects = new List<Effect>(); // Efeitos ativos no jogador
    public PlayerData enemy; // Referência para o inimigo do jogador
    public PlayerStatus status; // Status do jogador (vivo ou morto)
    public int numberOfBuysThisTurn = 0; // Número de compras efetuadas por esse jogador em um determinado turno

    /*
     * Método que diz se um player é dono de um card
     */
    public bool OwnsCard(Card card) => card.owner == this.Number;

    /*
     * Método que permite a adição de uma carta para o jogador
     */
    public void AddCard(Card card)
    {
        card.owner = this.Number;
        this.cards.Add(card);
    }

    /*
     * Método que permite a adição de um efeito no jogador
     */
    public void AddEffect(Effect effect)
    {
        effect.target = this;
        this.activeEffects.Add(effect);
    }

    /*
     * Método que permite a remoção de um efeito no jogador
     */
    public void RemoveEffect(Effect effect)
    {
        effect.target = null;
        this.activeEffects.Remove(effect);
    }

    /*
     * Método que remove efeitos expirados no jogador
     */
    public void RemoveExpiredEffects()
    {
        List<Effect> effectsToRemove = new List<Effect>();

        foreach (Effect effect in this.activeEffects)
        {
            bool expired = effect.lifetime is TemporaryLifetime lifetime && lifetime.duration == 0;
            if (expired)
            {
                effectsToRemove.Add(effect);
            }
        }

        activeEffects.RemoveAll(effectsToRemove.Contains);
    }

    /*
     * Método que diz se o player deve estar morto ou não
     */
    public bool ShouldBeDead() => this.timeRemaining <= 0.0f + float.Epsilon;

    /*
     * Método que cura uma doença do player
     */
    public void CureDesease(string remedyName)
    {
        activeEffects.RemoveAll(effect => effect is DamagingEffect e && e.IsCuredBy(remedyName));
    }
}
