using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerData playerData;

    private new Text name;
    private TextWithFloat timeRemaining;
    private Text activeEffects;
    private List<GameObject> cardSlots;
    public const int numberOfCardsSlots = 3;

    private string timeRemainingTemplate = "Time remaining: {0:0.00} years";
    private string activeEffectsLabel = "Active effects: \n";

    public void Start()
    {
        this.name = this.transform.Find("Name").GetComponent<Text>();
        Text timeRemainingTextComponent = this.transform.Find("TimeRemaining").GetComponent<Text>();
        this.activeEffects = this.transform.Find("ActiveEffects").GetComponent<Text>();
        this.cardSlots = this.GetCardsGameObjects().OrderBy(gameObject => gameObject.name).ToList();

        this.name.text = this.playerData.name;
        this.timeRemaining = new TextWithFloat(timeRemainingTextComponent, this.timeRemainingTemplate, this.playerData.timeRemaining);
        this.activeEffects.text = this.activeEffectsLabel + string.Join("\n", this.playerData.activeEffects);
    }

    public void AddCard(string name, Effect effect)
    {
        if (this.SlotsAreFull())
            return;

        GameObject cardSlot = this.cardSlots.First(slot => SlotIsEmpty(slot));

        Image icon = this.GetCardSlotComponent<Image>(cardSlot, "Icon");
        icon.sprite = Resources.Load<Sprite>("ArtWork/" + name);

        Text title = this.GetCardSlotComponent<Text>(cardSlot, "Title");
        Text description = this.GetCardSlotComponent<Text>(cardSlot, "Description");
        Text valueText = this.GetCardSlotComponent<Text>(cardSlot, "Value");

        title.text = effect.name;
        description.text = effect.description;
        valueText.text = string.Empty;

        if (effect is DamagingEffect damagingEffect)
        {
            string template = "-{0:0.00} per round";
            TextWithFloat value = new TextWithFloat(valueText, template, damagingEffect.damage);
        }
        else if (effect is HealingEffect healingEffect)
        {
            string template = "+{0:0.00} per round";
            TextWithFloat value = new TextWithFloat(valueText, template, healingEffect.healing);
        }

        description.text = effect.name;

        Destroy(cardSlot.GetComponent<Card>());
        Card card = cardSlot.AddComponent<Card>();
        card.name = name;
        card.effect = effect;

        this.playerData.AddCard(card);

        card.gameObject.SetActive(true);
    }

    public void FlipCards()
    {
        foreach (var cardSlot in this.cardSlots)
        {
            Image back = this.GetCardSlotComponent<Image>(cardSlot, "Back");

            back.enabled = !back.enabled;
        }
    }

    public bool SlotIsEmpty(GameObject slot)
    {
        return slot.GetComponent<Card>() is null;
    }

    public T GetCardSlotComponent<T>(GameObject cardSlot, string componentName)
    {
        return cardSlot.transform.Find(componentName).GetComponent<T>();
    }

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

    public void UpdateNameColor(Color color)
    {
        this.name.color = color;
    }

    private void FixedUpdate()
    {
        this.timeRemaining.SetValue(this.playerData.timeRemaining);
        this.activeEffects.text = this.activeEffectsLabel + string.Join("\n", this.playerData.activeEffects);
    }

    public bool SlotsAreFull()
    {
        return this.cardSlots.All(slot => !SlotIsEmpty(slot));
    }
}
