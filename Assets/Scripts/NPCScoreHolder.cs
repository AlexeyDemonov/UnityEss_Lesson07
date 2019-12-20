using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NPCController))]
public class NPCScoreHolder : NPCUIHolder
{
    public Text TextPrefab;

    Text _scoreText;
    int _score;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText = Instantiate<Text>(TextPrefab);
        _scoreText.text = string.Empty;
        _scoreText.transform.SetParent(/*base.*/MainCanvas.transform);

        base.UI = _scoreText.GetComponent<RectTransform>();

        GetComponent<NPCController>().KilledSomeone += IncreaseScore;
    }

    void IncreaseScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    public void HideScore()
    {
        _scoreText.gameObject.SetActive(false);
    }
}