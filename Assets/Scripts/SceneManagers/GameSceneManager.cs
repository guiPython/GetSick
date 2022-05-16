using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Essa classe gerencia toda a cena do jogo de cartas
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    private const float yearsPerTurn = 1.0f; // dano padrão por turno
    private const int maxBuysPerTurn = 1; // máximo de cartas que podem ser compradas por turno
    private TextWithNumber turn; // componente turno do jogo
    private TextWithNumber buyCard; // componente turno do jogo
    private const string turnTemplate = "Turno: {0}"; // constante do texto para o turno
    private const string buyCardTemplate = "Comprar Carta ({0})"; // constante do texto para a compra de cartas

    private Queue<PlayerData> playerOrderQueue = new Queue<PlayerData>(); // ordem de jogada 

    private PlayerData currentPlayer; // player dono da rodada atual

    public PlayerData player1Data; // dados do player 1
    public PlayerData player2Data;// dados do player 2

    public PlayerData winner; // vencedor

    private List<ScriptableObject> cards = new List<ScriptableObject>(); // cartas em jogo

    /*
        Método que inicia o jogo, setta os atributos primarios da partida e de jogador
        define a ordem de jogada
    */
    void Start()
    {
        InitializeCards();

        Text turnTextComponent = GameObject.Find("Turn").GetComponent<Text>();
        this.turn = new TextWithNumber(turnTextComponent, turnTemplate, 0);
        Text buyCardTextComponent = GameObject.Find("BuyCard/Text").GetComponent<Text>();
        this.buyCard = new TextWithNumber(buyCardTextComponent, buyCardTemplate, maxBuysPerTurn);

        this.player1Data.timeRemaining = 10.0f;
        this.player1Data.numberOfBuysThisTurn = 0;
        this.player1Data.status = PlayerStatus.Alive;
        this.player1Data.activeEffects.Clear();
        this.player1Data.cards.Clear();

        this.player2Data.timeRemaining = 10.0f;
        this.player2Data.numberOfBuysThisTurn = 0;
        this.player2Data.status = PlayerStatus.Alive;
        this.player2Data.activeEffects.Clear();
        this.player2Data.cards.Clear();

        PlayerUIManager player1UIManager = this.GetPlayerUI(this.player1Data);
        player1UIManager.Init();
        this.DrawNCardsToPlayer(3, player1UIManager);

        PlayerUIManager player2UIManager = this.GetPlayerUI(this.player2Data);
        player2UIManager.Init();
        this.DrawNCardsToPlayer(3, player2UIManager);

        this.playerOrderQueue.Enqueue(player1Data);
        this.playerOrderQueue.Enqueue(player2Data);

        this.currentPlayer = this.playerOrderQueue.Peek();
        PlayerUIManager enemyUIManager = this.GetPlayerUI(this.currentPlayer.enemy);
        enemyUIManager.FlipCards();
        this.UpdateNames();
    }

    /*
        Carrega as cartas de efeito negativo e positivo dos recursos, adiciona na fila
        E aplica um shuffle nessa lista
    */
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

    /*
        Reinicializa as cartas chamando o método de inicializar as cartas
    */
    private void ReinitializeCards()
    {
        this.InitializeCards();
    }

    /*
        Método chamado quando a carta é clicada, caso seja o dono da carta, aplica os efeitos no player adversário
        e, por fim, verifica se o jogo acabou e caso tenha acabado, chama a tela de endGame
    */
    public void OnCardClicked(Card card)
    {
        if (!this.currentPlayer.OwnsCard(card))
            return;

        PlayerData cardTarget = card.effect is DamagingEffect ? this.currentPlayer.enemy : this.currentPlayer;
        this.RemoveCard(card, this.currentPlayer);

        card.effect.turnApplied = this.turn.GetValue();
        cardTarget.AddEffect(card.effect);

        if (card.effect.lifetime is ImmediateLifetime)
        {
            card.effect.Affect();
            cardTarget.RemoveEffect(card.effect);
        }

        bool gameHasEnded = this.CheckWhetherGameHasEnded();
        if (gameHasEnded)
            SceneManager.LoadScene("EndGameScene");
    }

    /*
        Vira as cartas dos dois jogadores usando um métododo playerUIManager
    */
    private void FlipBothPlayersCards()
    {
        PlayerUIManager playerUIManager = this.GetPlayerUI(this.currentPlayer);
        playerUIManager.FlipCards();
        
        PlayerUIManager enemyUIManager = this.GetPlayerUI(this.currentPlayer.enemy);
        enemyUIManager.FlipCards();
    }

    /*
        Lida com rodada, iterando sobre as rodadas e aplicando os efeitos da cartas caso não tenham expiradas
        E verifica se o jogar morreu (perdeu)
    */
    private void HandleCurrentTurn()
    {
        foreach (PlayerData player in this.playerOrderQueue)
        {
            foreach (Effect effect in player.activeEffects)
            {
                if (effect.turnApplied == this.turn.GetValue())
                    continue;
                
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

    /*
        Remove um carta de um jogador da mesa
    */
    public void RemoveCard(Card card, PlayerData player)
    {
        player.cards.Remove(card);
        card.gameObject.SetActive(false);
        Destroy(card);
    }

    /*
        Método para checar se o jogo acabou verificando se um dois jogadores morreu (perdeu) 
    */
    public bool CheckWhetherGameHasEnded()
    {
        return player1Data.ShouldBeDead() || player2Data.ShouldBeDead();
    }

    /*
        Método que move para o próximo turno verificando se o jogo acabou, dando a rodada para o próximo jogador
        dando dano padrão da rodada, adicionando o turno e resetando o número de cartas para comprar 
    */
    private void MoveToTheNextTurn()
    {
        bool gameHasEnded = this.CheckWhetherGameHasEnded();
        if (gameHasEnded)
            SceneManager.LoadScene("EndGameScene");

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

        this.currentPlayer.numberOfBuysThisTurn = 0;
        this.buyCard.SetValue(maxBuysPerTurn);
    }

    /*
        Método que alterar a cor do dono da rodada para verde e o outro jogador para branco
    */
    private void UpdateNames()
    {
        PlayerData currentPlayer = this.playerOrderQueue.Peek();
        
        foreach (PlayerData player in this.playerOrderQueue)
        {
            PlayerUIManager playerUI = this.GetPlayerUI(player);

            playerUI.UpdateNameColor(player == currentPlayer ? Color.green : Color.white);
        }
    }

    /*
        Método que retorna o playerUI
    */
    private PlayerUIManager GetPlayerUI(PlayerData player)
    {
        return GameObject.Find($"Player{player.Number}UI").GetComponent<PlayerUIManager>();
    }

    /*
        Método que compra carta do monte caso o jogador tenha cartas para comprar e não tenha todas os slots da mesa com cartas
    */
    public void BuyCard()
    {
        if (this.currentPlayer.numberOfBuysThisTurn == maxBuysPerTurn)
            return;

        PlayerUIManager playerUIManager = this.GetPlayerUI(this.currentPlayer);

        if (playerUIManager.SlotsAreFull())
            return;

        if (this.cards.Count == 0)
        {
            this.ReinitializeCards();
        }

        if (this.cards.Count == 0)
            return;

        this.DrawNCardsToPlayer(1, playerUIManager);

        this.currentPlayer.numberOfBuysThisTurn++;
        this.buyCard--;
    }

    /*
        Método que, para cada carta jogada, aplicado o efeito dela no jogador adversário e a remove do jogo
    */
    private void DrawNCardsToPlayer(uint numberOfCardsToDraw, PlayerUIManager playerUIManager)
    {
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            var effectData = this.cards.First();

            Effect effect = this.ParseEffect(effectData);

            playerUIManager.AddCard(name: effect.name, effect: effect);
            this.cards.Remove(effectData);
        }
    }

    /*
        Método que transforma os efeitos das cartas em dano ou em cura dependendo do tipo da carta
    */
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

    /*
        Método que passa o turno
    */
    public void PassTurn()
    {
        this.currentPlayer = this.playerOrderQueue.Peek();
        this.HandleCurrentTurn();
        this.MoveToTheNextTurn();
    }

    /*
        Método que atualiza o jogo a cada frame, caso seja apertado a teclaca da seta direita, passa o turno
        verifica se o jogo acabou e caso acabe, chama a cena de endGame 
    */
    void Update()
    {
        if (this.CheckWhetherGameHasEnded())
            SceneManager.LoadScene("EndGameScene");
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.PassTurn();
        } 
    }
}
