using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneCTRL : MonoBehaviour
{
    private static string nextScene; // The name of the next scene to load
    [SerializeField] private Image loadingBar; // The loading bar UI
    [SerializeField] private TMP_Text tipText; // Tip text during loading
    [SerializeField] private List<string> loadingTips = new List<string>();

    // Call this static method to start loading a scene
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading"); // Ensure you have a loading scene in your project
    }

    private void Start()
    {
        // Start the loading process when the loading scene is loaded
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        // Start loading the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false; // Prevent automatic scene activation

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            // Update loading bar fill based on the loading progress
            if (asyncLoad.progress < 0.9f)
            {
                loadingBar.fillAmount = asyncLoad.progress; // Update progress up to 90%
            }
            else
            {
                // Smoothly fill the remaining progress bar over time
                timer += Time.unscaledDeltaTime;
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer / 1f); // Adjust 1f for speed

                // When fully loaded, display the continue button
                if (loadingBar.fillAmount >= 1f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
