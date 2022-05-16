using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

/// <summary>
/// Classe que gerencia a UI de um player no jogo
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    public PlayerData playerData; // dados do jogador

    private new Text name; // nome do jogador
    private TextWithFloat timeRemaining; // dado que o jogador ainda pode levar
    private Text activeEffects; // efeitos ativos nesse jogador
    private List<GameObject> cardSlots; // slots de cartas da mesa
    public const int numberOfCardsSlots = 3; // numero máximo de slots para cartas na mesa

    private string timeRemainingTemplate = "Tempo restante: {0:0.00} anos"; // texto do dado restante do jogador
    private string activeEffectsLabel = "Efeitos ativos: \n"; // texto de efeitos ativos do jogador

    // inicia o jogo
    public void Start()
    {
        this.Init();
    }

    /*
        Método que inicia o componente com os atributos iniciais
    */
    public void Init()
    {
        this.name = this.transform.Find("Name").GetComponent<Text>();
        Text timeRemainingTextComponent = this.transform.Find("TimeRemaining").GetComponent<Text>();
        this.activeEffects = this.transform.Find("ActiveEffects").GetComponent<Text>();
        this.cardSlots = this.GetCardsGameObjects().OrderBy(gameObject => gameObject.name).ToList();

        this.name.text = this.playerData.name;
        this.timeRemaining = new TextWithFloat(timeRemainingTextComponent, this.timeRemainingTemplate, this.playerData.timeRemaining);
        this.activeEffects.text = this.activeEffectsLabel + string.Join("\n", this.playerData.activeEffects);
    }

    /*
        Método que adiciona carta caso os slots do jogador não estejam vazios
        Carrega cada pedaço da carta dinamicamente a partir de um template
    */
    public void AddCard(string name, Effect effect)
    {
        if (this.SlotsAreFull())
            return;

        GameObject cardSlot = this.cardSlots.First(slot => SlotIsEmpty(slot));

        Image icon = this.GetCardSlotComponent<Image>(cardSlot, "Icon");
        icon.sprite = Resources.Load<Sprite>("ArtWork/" + name.Replace(" ", ""));

        Text title = this.GetCardSlotComponent<Text>(cardSlot, "Title");
        Text description = this.GetCardSlotComponent<Text>(cardSlot, "Description");
        Text valueText = this.GetCardSlotComponent<Text>(cardSlot, "Value");

        title.text = effect.name;
        description.text = effect.description;
        valueText.text = string.Empty;

        if (effect is DamagingEffect damagingEffect)
        {
            string template = "-{0:0.00} Tempo de vida do inimigo";

            if (damagingEffect.lifetime is TemporaryLifetime
             || damagingEffect.lifetime is PermanentLifetime)
            {
                template += " por rodada";
            }
            else
            {
                template += " imediatamente";
            }

            TextWithFloat value = new TextWithFloat(valueText, template, damagingEffect.damage);
        }
        else if (effect is HealingEffect healingEffect)
        {
            string template = "+{0:0.00} Tempo de vida";

            if (healingEffect.lifetime is TemporaryLifetime
             || healingEffect.lifetime is PermanentLifetime)
            {
                template += "\npor rodada";
            }
            else
            {
                template += "\nimediatamente";
            }

            TextWithFloat value = new TextWithFloat(valueText, template, healingEffect.healing);
        }

        Destroy(cardSlot.GetComponent<Card>());
        Card card = cardSlot.AddComponent<Card>();
        card.name = name;
        card.effect = effect;

        this.playerData.AddCard(card);

        card.gameObject.SetActive(true);
    }

    /*
        Método que vira todas cartas do jogador 
    */
    public void FlipCards()
    {
        foreach (var cardSlot in this.cardSlots)
        {
            Image back = this.GetCardSlotComponent<Image>(cardSlot, "Back");

            back.enabled = !back.enabled;
        }
    }

    /*
        Método que verifica um slot do jogador está vazio
    */
    public bool SlotIsEmpty(GameObject slot)
    {
        return slot.GetComponent<Card>() is null;
    }

    /*
        Método auxiliar que procura e retorna um componente genérico
    */
    public T GetCardSlotComponent<T>(GameObject cardSlot, string componentName)
    {
        return cardSlot.transform.Find(componentName).GetComponent<T>();
    }

    /*
        Método que pega as cartas do canvas e adiciona no deck do jogador 
    */
    private List<GameObject> GetCardsGameObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (Transform transform in this.transform)
        {
            if (transform.name.StartsWith("CardSlotPrefab"))
            {
                gameObjects.Add(transform.Find("Canvas").gameObject);
            }
        }

        return gameObjects;
    }

    /*
        Método que setta a cor do nome do jogador
    */
    public void UpdateNameColor(Color color)
    {
        this.name.color = color;
    }

    /*
        Método que atualiza o jogo setando o dano restante do jogador e os efeitos ativos nele
    */
    private void FixedUpdate()
    {
        this.timeRemaining.SetValue(this.playerData.timeRemaining);
        this.activeEffects.text = this.activeEffectsLabel + string.Join("\n", this.playerData.activeEffects);
    }

    /*
        Método que retorna se todos os slots de cartas estão vazios
    */
    public bool SlotsAreFull()
    {
        return this.cardSlots.All(slot => !SlotIsEmpty(slot));
    }
}
