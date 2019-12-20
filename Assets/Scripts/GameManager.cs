using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Teams")]
    public TargetRegisterSystem KyleTeam;
    public TargetRegisterSystem JakeTeam;

    [Header("UI")]
    public GameObject UIPanel;
    public Text GameOverText;

    // Start is called before the first frame update
    void Start()
    {
        KyleTeam.RanOutOfTargets += () => EndGame("Jakes Win");
        JakeTeam.RanOutOfTargets += () => EndGame("Kyles Win");
    }

    void EndGame(string message)
    {
        GameOverText.text = message;
        UIPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}