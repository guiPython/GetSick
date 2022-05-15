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

    public bool ShouldBeDead() => this.timeRemaining <= 0.0f + float.Epsilon;
    public void CureDesease(string remedyName)
    {
        activeEffects.RemoveAll(effect => effect is DamagingEffect e && e.IsCuredBy(remedyName));
    }
}
