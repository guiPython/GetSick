using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    private const float yearsPerTurn = 1.0f;
    private TextWithNumber turn;
    private const string template = "Turn: {0}";

    private Queue<PlayerData> playerOrderQueue = new Queue<PlayerData>();

    private PlayerData currentPlayer;

    public PlayerData player1Data;
    public PlayerData player2Data;

    public PlayerData winner;

    private List<ScriptableObject> cards = new List<ScriptableObject>();

    void Start()
    {
        InitializeCards();

        Text turnTextComponent = GameObject.Find("Turn").GetComponent<Text>();
        this.turn = new TextWithNumber(turnTextComponent, template, 0);

        player1Data.timeRemaining = 100.0f;
        player1Data.status = PlayerStatus.Alive;

        player2Data.timeRemaining = 100.0f;
        player2Data.status = PlayerStatus.Alive;

        this.playerOrderQueue.Enqueue(player1Data);
        this.playerOrderQueue.Enqueue(player2Data);

        this.currentPlayer = this.playerOrderQueue.Peek();
        PlayerUIManager enemyUIManager = this.GetPlayerUI(this.currentPlayer.enemy);
        enemyUIManager.FlipCards();
        this.UpdateNames();
    }

    private void InitializeCards()
    {
        List<NegativeEffectData> negativeEffectCards = Resources.LoadAll<NegativeEffectData>("Cards/NegativeEffects").ToList();
        List<PositiveEffectData> positiveEffectCards = Resources.LoadAll<PositiveEffectData>("Cards/PositiveEffects").ToList();

        foreach (var card in negativeEffectCards)
        {
            for (int i = 0; i < card.numberOfCards; i++)
            {
                this.cards.Add(card);
            }
        }

        foreach (var card in positiveEffectCards)
        {
            for (int i = 0; i < card.numberOfCards; i++)
            {
                this.cards.Add(card);
            }
        }

        this.cards.Shuffle();
    }

    private void ReinitializeCards()
    {
        this.InitializeCards();
    }

    public void OnCardClicked(Card card)
    {
        if (!this.currentPlayer.OwnsCard(card))
            return;

        PlayerData cardTarget = card.effect is DamagingEffect ? this.currentPlayer.enemy : this.currentPlayer;
        this.RemoveCard(card, this.currentPlayer);

        if (card.effect.lifetime is ImmediateLifetime)
        {
            cardTarget.AddEffect(card.effect);
            card.effect.Affect();
            cardTarget.RemoveEffect(card.effect);
            this.HandleCurrentTurn();
        }
        else
        {
            this.HandleCurrentTurn();
            cardTarget.AddEffect(card.effect);
        }

        bool gameHasEnded = this.CheckWhetherGameHasEnded();
        if (gameHasEnded)
            return;

        this.MoveToTheNextTurn();
    }

    private void FlipBothPlayersCards()
    {
        PlayerUIManager playerUIManager = this.GetPlayerUI(this.currentPlayer);
        playerUIManager.FlipCards();
        
        PlayerUIManager enemyUIManager = this.GetPlayerUI(this.currentPlayer.enemy);
        enemyUIManager.FlipCards();
    }

    private void HandleCurrentTurn()
    {
        foreach (PlayerData player in this.playerOrderQueue)
        {
            foreach (Effect effect in player.activeEffects)
            {
                effect.Affect();

                if (effect.lifetime is TemporaryLifetime lifetime)
                {
                    lifetime.duration--;
                }
            }

            player.RemoveExpiredEffects();

            if (player.ShouldBeDead())
            {
                player.status = PlayerStatus.Dead;
            }
        }
    }

    public void RemoveCard(Card card, PlayerData player)
    {
        player.cards.Remove(card);
        card.gameObject.SetActive(false);
        Destroy(card);
    }

    public bool CheckWhetherGameHasEnded()
    {
        return false;
    }

    private void MoveToTheNextTurn()
    {
        this.playerOrderQueue.Dequeue();
        this.playerOrderQueue.Enqueue(this.currentPlayer);
        this.currentPlayer = this.playerOrderQueue.Peek();

        foreach (PlayerData player in this.playerOrderQueue)
        {
            player.timeRemaining -= yearsPerTurn;
        }

        PlayerUIManager currentPlayerUI = this.GetPlayerUI(this.currentPlayer);
        currentPlayerUI.UpdateNameColor(Color.white);

        this.UpdateNames();
        this.FlipBothPlayersCards();

        this.turn++;
    }

    private void UpdateNames()
    {
        PlayerData currentPlayer = this.playerOrderQueue.Peek();
        
        foreach (PlayerData player in this.playerOrderQueue)
        {
            PlayerUIManager playerUI = this.GetPlayerUI(player);

            playerUI.UpdateNameColor(player == currentPlayer ? Color.green : Color.white);
        }
    }

    private PlayerUIManager GetPlayerUI(PlayerData player)
    {
        return GameObject.Find($"Player{player.Number}UI").GetComponent<PlayerUIManager>();
    }

    public void BuyCard()
    {
        PlayerUIManager playerUIManager = this.GetPlayerUI(this.currentPlayer);

        if (playerUIManager.SlotsAreFull())
            return;

        if (this.cards.Count == 0)
        {
            this.ReinitializeCards();
        }

        if (this.cards.Count == 0)
            return;

        var effectData = this.cards.First();

        Effect effect = this.ParseEffect(effectData);
        
        playerUIManager.AddCard(name: effect.name, effect: effect);
        this.cards.Remove(effectData);

        this.HandleCurrentTurn();

        bool gameHasEnded = this.CheckWhetherGameHasEnded();
        if (gameHasEnded)
            return;

        this.MoveToTheNextTurn();
    }

    private Effect ParseEffect(ScriptableObject effectData)
    {
        if (effectData is PositiveEffectData positiveEffect)
        {
            IEffectLifeTime lifetime = positiveEffect.lifetime switch
            {
                Lifetime.Immediate => new ImmediateLifetime(),
                Lifetime.Temporary => TemporaryLifetime.From(positiveEffect.duration),
                Lifetime.Permanent => new PermanentLifetime(),
                _ => throw new NotImplementedException("Invalid lifetime")
            };

            Effect effect = positiveEffect.type switch
            {
                PositiveEffectType.Cure => new CureEffect(positiveEffect.name, positiveEffect.description, lifetime),
                PositiveEffectType.Heal => new HealingEffect(positiveEffect.name, positiveEffect.description, positiveEffect.healing, lifetime),
                _ => throw new NotImplementedException("Invalid effect type")
            };

            return effect;
        }
        
        if (effectData is NegativeEffectData negativeEffect)
        {
            IEffectLifeTime lifetime = negativeEffect.lifetime switch
            {
                Lifetime.Immediate => new ImmediateLifetime(),
                Lifetime.Temporary => TemporaryLifetime.From(negativeEffect.duration),
                Lifetime.Permanent => new PermanentLifetime(),
                _ => throw new NotImplementedException("Invalid lifetime")
            };

            Effect effect = negativeEffect.type switch
            {
                NegativeEffectType.Damage => new DamagingEffect(negativeEffect.name, negativeEffect.description, negativeEffect.damage, lifetime, negativeEffect.curedBy.ToList()),
                _ => throw new NotImplementedException("Invalid effect type")
            };

            return effect;
        }

        throw new Exception("Invalid program state");
    }

    public void PassTurn()
    {
        this.currentPlayer = this.playerOrderQueue.Peek();
        this.HandleCurrentTurn();
        this.MoveToTheNextTurn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.PassTurn();
        }
    }
}
