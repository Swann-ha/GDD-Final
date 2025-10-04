using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject menuRoot; // Assign a panel (inactive) holding buttons
    public Button firstSelectedButton; // Optional focus target

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.Escape;
    public bool pauseTime = true;
    public bool autoBuildMenuIfMissing = true;
    public bool useAsyncLoading = false;

    public bool IsPaused { get; private set; }
    float _previousTimeScale = 1f;

    void Awake()
    {
        EnsureEventSystem();
        if (menuRoot == null && autoBuildMenuIfMissing)
        {
            BuildRuntimeMenu();
        }
        if (menuRoot != null) menuRoot.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey)) Toggle();
    }

    void EnsureEventSystem()
    {
        if (EventSystem.current == null)
        {
            var esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<StandaloneInputModule>();
        }
    }

    void BuildRuntimeMenu()
    {
        // Create Canvas
        var canvasGO = new GameObject("PauseMenu_Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create background panel
        var panelGO = new GameObject("Panel");
        var rtPanel = panelGO.AddComponent<RectTransform>();
        panelGO.transform.SetParent(canvasGO.transform, false);
        var img = panelGO.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.75f); // Dark semi-transparent background
        rtPanel.anchorMin = Vector2.zero;
        rtPanel.anchorMax = Vector2.one;
        rtPanel.anchoredPosition = Vector2.zero;
        rtPanel.sizeDelta = Vector2.zero;

        menuRoot = panelGO;

        // Create button container
        var containerGO = new GameObject("ButtonContainer");
        var rtContainer = containerGO.AddComponent<RectTransform>();
        containerGO.transform.SetParent(panelGO.transform, false);

        // Center the container
        rtContainer.anchorMin = new Vector2(0.5f, 0.5f);
        rtContainer.anchorMax = new Vector2(0.5f, 0.5f);
        rtContainer.anchoredPosition = Vector2.zero;
        rtContainer.sizeDelta = new Vector2(300, 280);

        // Add subtle background to container
        var containerImg = containerGO.AddComponent<Image>();
        containerImg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        // Setup layout
        var layout = containerGO.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 12f;
        layout.padding = new RectOffset(20, 20, 20, 20);

        // Create buttons with helper function
        Button MakeButton(string label, System.Action onClick)
        {
            var bGO = new GameObject(label + "Button");
            var rtButton = bGO.AddComponent<RectTransform>();
            bGO.transform.SetParent(containerGO.transform, false);

            rtButton.sizeDelta = new Vector2(260f, 45f);

            var image = bGO.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 0.9f); // Dark gray buttons
            var btn = bGO.AddComponent<Button>();

            // Button hover colors
            var colors = btn.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 0.9f);
            colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f, 0.9f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            btn.colors = colors;

            btn.onClick.AddListener(() => onClick());

            // Create button text
            var txtGO = new GameObject("Text");
            var rtText = txtGO.AddComponent<RectTransform>();
            txtGO.transform.SetParent(bGO.transform, false);
            var txt = txtGO.AddComponent<Text>();
            txt.text = label;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            txt.fontSize = 16;

            rtText.anchorMin = Vector2.zero;
            rtText.anchorMax = Vector2.one;
            rtText.anchoredPosition = Vector2.zero;
            rtText.sizeDelta = Vector2.zero;

            return btn;
        }

        // Create all buttons
        firstSelectedButton = MakeButton("Resume", Resume);
        MakeButton("Restart", RestartLevel);
        MakeButton("Main Menu", ReturnToMainMenu);
        MakeButton("Quit", QuitGame);
    }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;

        if (pauseTime)
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        if (menuRoot != null) menuRoot.SetActive(true);

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;

        if (pauseTime) Time.timeScale = _previousTimeScale;
        if (menuRoot != null) menuRoot.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Toggle()
    {
        if (IsPaused) Resume();
        else Pause();
    }

    public void RestartLevel()
    {
        if (pauseTime) Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        if (pauseTime) Time.timeScale = 1f;
        SceneLoader.LoadMainMenu(useAsyncLoading);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}