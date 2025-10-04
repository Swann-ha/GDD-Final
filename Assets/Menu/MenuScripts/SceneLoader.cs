using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SceneLoader
{
    // Set this in MainMenuController or a bootstrap component
    public static string MainMenuSceneName = "MainMenu";

    public static void Load(string sceneName, bool useAsync = false)
    {
        // Check if scene exists in build settings
        if (!SceneExistsInBuild(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings! Add it via File -> Build Settings");
            return;
        }

        if (useAsync)
        {
            var runner = GetRunner();
            runner.StartCoroutine(LoadAsyncRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    static bool SceneExistsInBuild(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
#if UNITY_EDITOR
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
#else
            string scenePath = "";
#endif
            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (name == sceneName) return true;
        }
        return false;
    }

    public static void ReloadCurrent(bool useAsync = false)
    {
        var current = SceneManager.GetActiveScene().name;
        Load(current, useAsync);
    }

    public static void LoadMainMenu(bool useAsync = false)
    {
        Load(MainMenuSceneName, useAsync);
    }

    static IEnumerator LoadAsyncRoutine(string scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;
    }

    // Utility hidden runner for coroutines from a static context
    static SceneLoaderRunner _runner;
    static SceneLoaderRunner GetRunner()
    {
        if (_runner == null)
        {
            var go = new GameObject("~SceneLoader");
            Object.DontDestroyOnLoad(go);
            _runner = go.AddComponent<SceneLoaderRunner>();
        }
        return _runner;
    }

    class SceneLoaderRunner : MonoBehaviour { }
}
