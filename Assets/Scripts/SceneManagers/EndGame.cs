using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Essa classe gerencia toda a cena do final do jogo.
/// Carrega as cenas de Menu, Creditos ou Jogo dependendo do input do usuario.
/// </summary>
public class EndGame : MonoBehaviour
{
    public PlayerData player1Data; // informações do player 1
    public PlayerData player2Data; // informações do player 1

    public RawImage rawImagePlayer1; // imagem de render do video do player 1
    public RawImage rawImagePlayer2; // imagem de render do video do player 2

    public GameObject canvas; // canvas

    public VideoClip alive; // video da situação do jogador quando vivo ao final do game

    public VideoClip dead; // video da situação do jogador quando morto ao final do game

    /*
        Setta o titulo com o player vencedor 
        Configura e atribui o video ao respectivo jogador morto e vivo
    */
    private void SetStatusForPlayerWinner(PlayerData player, string playerNumber)
    {
        var statusObject = GameObject.Find("PlayerStatus");
        statusObject.GetComponent<Text>().color = Color.green;
        statusObject.GetComponent<Text>().text = player.name + " venceu!";

        if (playerNumber == "1")
        {
            // caso o jogador 1 vença
            var videoPlayer1 = canvas.transform.Find("VideoPlayer1").GetComponent<VideoPlayer>();
            videoPlayer1.clip = (VideoClip) Resources.Load("Videos/alive");
            videoPlayer1.targetTexture = (RenderTexture) Resources.Load("Videos/AliveRT");

            var videoPlayer1Render = canvas.transform.Find("VideoPlayer1Render").GetComponent<RawImage>();
            videoPlayer1Render.texture = (RenderTexture) Resources.Load("Videos/AliveRT");

            var videoPlayer2 = canvas.transform.Find("VideoPlayer2").GetComponent<VideoPlayer>();
            videoPlayer2.clip = (VideoClip) Resources.Load("Videos/dead");
            videoPlayer2.targetTexture = (RenderTexture) Resources.Load("Videos/DeadRT");

            var videoPlayer2Render = canvas.transform.Find("VideoPlayer2Render").GetComponent<RawImage>();
            videoPlayer2Render.texture = (RenderTexture) Resources.Load("Videos/DeadRT");
        } else if (playerNumber == "2")
        {
            // caso o jogador 2 vença
            var videoPlayer1 = canvas.transform.Find("VideoPlayer1").GetComponent<VideoPlayer>();
            videoPlayer1.clip = (VideoClip) Resources.Load("Videos/dead");
            videoPlayer1.targetTexture = (RenderTexture) Resources.Load("Videos/DeadRT");

            var videoPlayer1Render = canvas.transform.Find("VideoPlayer1Render").GetComponent<RawImage>();
            videoPlayer1Render.texture = (RenderTexture) Resources.Load("Videos/DeadRT");

            var videoPlayer2 = canvas.transform.Find("VideoPlayer2").GetComponent<VideoPlayer>();
            videoPlayer2.clip = (VideoClip) Resources.Load("Videos/alive");
            videoPlayer2.targetTexture = (RenderTexture) Resources.Load("Videos/AliveRT");

            var videoPlayer2Render = canvas.transform.Find("VideoPlayer2Render").GetComponent<RawImage>();
            videoPlayer2Render.texture = (RenderTexture) Resources.Load("Videos/AliveRT");
        }
    }

    
    /*
        Setta o titulo como empate
        Configura e atribui aos dois jogadores o video de morto
    */
    private void SetStatusForTie()
    {
        var statusObject = GameObject.Find("PlayerStatus");
        statusObject.GetComponent<Text>().color = Color.red;
        statusObject.GetComponent<Text>().text = "Empate";

        var videoPlayer1 = canvas.transform.Find("VideoPlayer1").GetComponent<VideoPlayer>();
        videoPlayer1.clip = (VideoClip) Resources.Load("Videos/dead");
        videoPlayer1.targetTexture = (RenderTexture) Resources.Load("Videos/DeadRT");

        var videoPlayer1Render = canvas.transform.Find("VideoPlayer1Render").GetComponent<RawImage>();
        videoPlayer1Render.texture = (RenderTexture) Resources.Load("Videos/DeadRT");

        var videoPlayer2 = canvas.transform.Find("VideoPlayer2").GetComponent<VideoPlayer>();
        videoPlayer2.clip = (VideoClip) Resources.Load("Videos/dead");
        videoPlayer2.targetTexture = (RenderTexture) Resources.Load("Videos/DeadRT");

        var videoPlayer2Render = canvas.transform.Find("VideoPlayer2Render").GetComponent<RawImage>();
        videoPlayer2Render.texture = (RenderTexture) Resources.Load("Videos/DeadRT");
    }


    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        if (player1Data.ShouldBeDead() && player2Data.ShouldBeDead())
        {
            SetStatusForTie();
        } else if (player2Data.ShouldBeDead())
        {
            SetStatusForPlayerWinner(player1Data, "1");
        } else if (player1Data.ShouldBeDead())
        {
            SetStatusForPlayerWinner(player2Data, "2");
        }
    }


    /*
        Carrega a cena do Jogo novamente.
    */
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /*
        Carrega a cena do Menu novamente.
    */
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /*
        Carrega a cena de Cr�ditos.
    */
    public void ExitGame()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
