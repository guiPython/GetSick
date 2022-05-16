using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Componente de carta
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler
{
    public new string name; // Nome da carta
    public int owner; // Player dono da carta

    public Effect effect; // Efeito da carta

    /*
     * Método que adiciona um efeito na carta
     */
    public Card AddEffect(Effect effect)
    {
        this.effect = effect;

        return this;
    }

    /*
     * Método que aciona ao clicar na carta
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameSceneManager>()
            .SendMessage("OnCardClicked", this);
    }
}
