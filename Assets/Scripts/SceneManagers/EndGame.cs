using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Essa classe gerencia toda a cena do final do jogo.
/// Carrega as cenas de Menu, Creditos ou Jogo dependendo do input do usuario.
/// </summary>
public class EndGame : MonoBehaviour
{
    public PlayerData player1Data;
    public PlayerData player2Data;

    /// <summary>
    /// Encontra o GameObject status.
    /// 
    /// /// </summary>
    private void SetStatusForPlayerWinner(PlayerData player)
    {
        var statusObject = GameObject.Find("PlayerStatus");
        statusObject.GetComponent<Text>().color = Color.green;
        statusObject.GetComponent<Text>().text = "Player " + player.name + " venceu!";
    }

    
    /// <summary>
    /// Encontra o GameObject status.
    /// 
    /// </summary>
    private void SetStatusForTie()
    {
        var statusObject = GameObject.Find("PlayerStatus");
        statusObject.GetComponent<Text>().color = Color.red;
        statusObject.GetComponent<Text>().text = "Empate";
    }


    // Start is called before the first frame update
    void Start()
    {
        if (player1Data.ShouldBeDead() && player2Data.ShouldBeDead())
        {
            SetStatusForTie();
        } else if (player2Data.ShouldBeDead())
        {
            SetStatusForPlayerWinner(player1Data);
        } else if (player1Data.ShouldBeDead())
        {
            SetStatusForPlayerWinner(player2Data);
        }
    }


    /// <summary>
    /// Carrega a cena do Jogo novamente.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Carrega a cena do Menu novamente.
    /// </summary>
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Carrega a cena de Cr�ditos.
    /// </summary>
    public void ExitGame()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
