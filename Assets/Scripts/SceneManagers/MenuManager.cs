using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
// Essa classe gerencia a cena de menu que so pode carregar a cena do jogo;
/// </summary>
public class MenuManager : MonoBehaviour
{
    /*
        Carrega a cena
    */
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    /*
        Carrega a cena de Creditos
    */
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
