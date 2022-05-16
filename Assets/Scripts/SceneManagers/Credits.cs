using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Essa classe gerencia a cena de creditos que se tem a funcao de fechar o game.
/// </summary>
public class Credits : MonoBehaviour
{

    /*
    /// Metodo para fechar o Jogo
    */
    public void ExitGame()
    {
      Application.Quit();
    }

    /*
    /// Metodo que carrega a cena do Menu
    */
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
