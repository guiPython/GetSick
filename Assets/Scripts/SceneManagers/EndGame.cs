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
    public PlayerData player1Data;
    public PlayerData player2Data;

    public RawImage rawImagePlayer1;
    public RawImage rawImagePlayer2;

    public GameObject canvas;

    public VideoClip alive;

    public VideoClip dead;

    /// <summary>
    /// Encontra o GameObject status.
    /// 
    /// /// </summary>
    private void SetStatusForPlayerWinner(PlayerData player, string playerNumber)
    {
        var statusObject = GameObject.Find("PlayerStatus");
        statusObject.GetComponent<Text>().color = Color.green;
        statusObject.GetComponent<Text>().text = player.name + " venceu!";

        if (playerNumber == "1")
        {
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

    
    /// <summary>
    /// Encontra o GameObject status.
    /// 
    /// </summary>
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
