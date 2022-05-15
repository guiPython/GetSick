using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    public new string name;
    public int owner;

    public Effect effect;

    public Card AddEffect(Effect effect)
    {
        this.effect = effect;

        return this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameSceneManager>()
            .SendMessage("OnCardClicked", this);
    }
}
