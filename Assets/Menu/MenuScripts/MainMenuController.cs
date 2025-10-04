using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string scene1 = "Level1";
    [SerializeField] private string scene2 = "Level2";
    [SerializeField] private string scene3 = "Level3";

    [Header("Options")]
    [SerializeField] private bool autoBuildUI = true;
    [SerializeField] private bool useAsyncLoading = false;

    [Header("References")]
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private Button firstButton;

    void Awake()
    {
        // Ensure SceneLoader is properly initialized
        if (string.IsNullOrEmpty(SceneLoader.MainMenuSceneName))
        {
            SceneLoader.MainMenuSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        EnsureEventSystem();

        if (menuRoot == null && autoBuildUI)
        {
            BuildUI();
        }

        // Add null check and delay the selection to next frame
        if (firstButton != null)
        {
            StartCoroutine(SelectFirstButtonNextFrame());
        }
    }

    private System.Collections.IEnumerator SelectFirstButtonNextFrame()
    {
        yield return null; // Wait one frame
        if (EventSystem.current != null && firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
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

    void BuildUI()
    {
        var canvasGO = new GameObject("MainMenu_Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        var panelGO = new GameObject("Panel");
        var rtp = panelGO.AddComponent<RectTransform>();
        panelGO.transform.SetParent(canvasGO.transform, false);
        var img = panelGO.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.65f);
        rtp.anchorMin = Vector2.zero;
        rtp.anchorMax = Vector2.one;
        rtp.anchoredPosition = Vector2.zero;
        rtp.sizeDelta = Vector2.zero;

        menuRoot = panelGO;

        var container = new GameObject("Buttons");
        var rt = container.AddComponent<RectTransform>();
        container.transform.SetParent(panelGO.transform, false);

        // Center the container
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(320, 300);

        // Add visible background to container
        var containerImg = container.AddComponent<Image>();
        containerImg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        var layout = container.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 20f;
        layout.padding = new RectOffset(20, 20, 20, 20);

        Button MakeButton(string label, System.Action action)
        {
            var bGO = new GameObject(label + "Button");
            bGO.AddComponent<RectTransform>();
            bGO.transform.SetParent(container.transform, false);
            var image = bGO.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 0.9f);
            var btn = bGO.AddComponent<Button>();

            // Add null check for the action
            if (action != null)
            {
                btn.onClick.AddListener(() => action());
            }

            var textGO = new GameObject("Text");
            var rtText = textGO.AddComponent<RectTransform>();
            textGO.transform.SetParent(bGO.transform, false);
            var txt = textGO.AddComponent<Text>();
            txt.text = label;

            // Use a safer font loading approach
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            txt.font = font;

            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            txt.fontSize = 16;

            rtText.anchorMin = Vector2.zero;
            rtText.anchorMax = Vector2.one;
            rtText.anchoredPosition = Vector2.zero;
            rtText.sizeDelta = Vector2.zero;
            return btn;
        }

        firstButton = MakeButton(scene1, () => LoadScene(scene1));
        MakeButton(scene2, () => LoadScene(scene2));
        MakeButton(scene3, () => LoadScene(scene3));
        MakeButton("Quit", QuitGame);
    }

    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneLoader.Load(sceneName, useAsyncLoading);
        }
        else
        {
            Debug.LogError("Scene name is null or empty!");
        }
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