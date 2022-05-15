using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Alive,
    Dead
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int Number; // Número do jogador
    public new string name; // Nome do jogador
    public float timeRemaining; // Tempo de vida restante do jogador em anos
    public List<Card> cards = new List<Card>(); // Cartas do jogador
    public List<Effect> activeEffects = new List<Effect>(); // Efeitos ativos no jogador
    public PlayerData enemy;
    public PlayerStatus status;

    public bool OwnsCard(Card card) => card.owner == this.Number;

    public void AddCard(Card card)
    {
        card.owner = this.Number;
        this.cards.Add(card);
    }

    public void AddEffect(Effect effect)
    {
        effect.target = this;
        this.activeEffects.Add(effect);
    }

    public void RemoveEffect(Effect effect)
    {
        effect.target = null;
        this.activeEffects.Remove(effect);
    }

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

    public bool ShouldBeDead() => this.timeRemaining <= 0.0f + float.Epsilon;
    public void CureDesease(string remedyName)
    {
        activeEffects.RemoveAll(effect => effect is DamagingEffect e && e.IsCuredBy(remedyName));
    }
}
