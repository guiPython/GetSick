using UnityEngine;
using UnityEngine.SceneManagement;


/*
* Essa classe gerencia a cena de menu que so pode carregar a cena do jogo;
*/
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Carrega a cena
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    /// <summary>
    /// Carrega a cena de Creditos
    /// </summary>
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
