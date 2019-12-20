using UnityEngine;

public class NPCUIHolder : MonoBehaviour
{
    public Transform UIHolder;
    public RectTransform UI;

    static protected Canvas MainCanvas;
    static Camera _mainCamera;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (MainCanvas == null)
            MainCanvas = FindObjectOfType<Canvas>();

        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (UI != null && UI.gameObject.activeSelf)
            UI.position = _mainCamera.WorldToScreenPoint(UIHolder.position);
    }
}