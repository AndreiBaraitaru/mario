using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject gameOverPanel;
    public Transform restartButton;
    public Transform gameCamera;
    public Vector3 offset = new Vector3(7f, 4f, 10f);

    private TextMeshPro tmpWorld;
    private TextMeshProUGUI tmpUI;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Cache whichever type of TextMeshPro exists
        if (scoreText != null)
        {
            tmpWorld = scoreText.GetComponent<TextMeshPro>();
            tmpUI = scoreText.GetComponent<TextMeshProUGUI>();
        }
    }

    void LateUpdate()
    {
        if (scoreText != null && gameCamera != null)
        {
            scoreText.transform.position = gameCamera.position + offset;
            scoreText.transform.rotation = Quaternion.identity; // face forward
        }
    }

    public void GameStart()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void SetScore(int score)
    {
        string scoreString = "Score: " + score.ToString();

        if (tmpWorld != null)
            tmpWorld.text = scoreString;
        else if (tmpUI != null)
            tmpUI.text = scoreString;
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
