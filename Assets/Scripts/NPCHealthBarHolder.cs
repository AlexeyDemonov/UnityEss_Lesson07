using UnityEngine;
using UnityEngine.UI;

public class NPCHealthBarHolder : NPCUIHolder
{
    public Slider HealthBarPrefab;

    Slider _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _healthBar = Instantiate<Slider>(HealthBarPrefab);
        _healthBar.transform.SetParent(/*base.*/MainCanvas.transform);
        base.UI = _healthBar.GetComponent<RectTransform>();
    }

    public void SetHealth(int value)
    {
        _healthBar.value = value;
    }

    public void HideHealthBar()
    {
        _healthBar.gameObject.SetActive(false);
    }
}