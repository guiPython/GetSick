using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>
/// Essa classe gerencia a cena de creditos que se tem a funcao de fechar o game.
/// </para>
/// </summary>
public class Credits : MonoBehaviour
{

    /// <summary>
    /// Metodo para fechar o Jogo
    /// </summary>
    public void ExitGame()
    {
      Application.Quit();
    }

    /// <summary>
    /// Metodo que carrega a cena do Menu
    /// </summary>
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
