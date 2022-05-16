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
    public int Number; // N�mero do jogador
    public new string name; // Nome do jogador
    public float timeRemaining; // Tempo de vida restante do jogador em anos
    public List<Card> cards = new List<Card>(); // Cartas do jogador
    public List<Effect> activeEffects = new List<Effect>(); // Efeitos ativos no jogador
    public PlayerData enemy; // Refer�ncia para o inimigo do jogador
    public PlayerStatus status; // Status do jogador (vivo ou morto)
    public int numberOfBuysThisTurn = 0; // N�mero de compras efetuadas por esse jogador em um determinado turno

    /*
     * M�todo que diz se um player � dono de um card
     */
    public bool OwnsCard(Card card) => card.owner == this.Number;

    /*
     * M�todo que permite a adi��o de uma carta para o jogador
     */
    public void AddCard(Card card)
    {
        card.owner = this.Number;
        this.cards.Add(card);
    }

    /*
     * M�todo que permite a adi��o de um efeito no jogador
     */
    public void AddEffect(Effect effect)
    {
        effect.target = this;
        this.activeEffects.Add(effect);
    }

    /*
     * M�todo que permite a remo��o de um efeito no jogador
     */
    public void RemoveEffect(Effect effect)
    {
        effect.target = null;
        this.activeEffects.Remove(effect);
    }

    /*
     * M�todo que remove efeitos expirados no jogador
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
     * M�todo que diz se o player deve estar morto ou n�o
     */
    public bool ShouldBeDead() => this.timeRemaining <= 0.0f + float.Epsilon;

    /*
     * M�todo que cura uma doen�a do player
     */
    public void CureDesease(string remedyName)
    {
        activeEffects.RemoveAll(effect => effect is DamagingEffect e && e.IsCuredBy(remedyName));
    }
}
