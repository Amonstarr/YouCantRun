using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;
    
    private static SceneTransition instance;
    
    private void Awake()
    {
        // Singleton pattern agar tidak hilang saat pindah scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (fadeImage != null)
            {
                fadeImage.color = fadeColor;
                StartCoroutine(FadeIn());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Fade in (dari gelap ke terang)
    public static void FadeInScene()
    {
        if (instance != null && instance.fadeImage != null)
        {
            instance.StartCoroutine(instance.FadeIn());
        }
    }
    
    // Fade out lalu load scene
    public static void LoadSceneWithFade(string sceneName)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.FadeOutAndLoadScene(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    // Fade out lalu load scene by index
    public static void LoadSceneWithFade(int sceneIndex)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.FadeOutAndLoadScene(sceneIndex));
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
    
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = fadeColor;
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
    
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color startColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        Color endColor = fadeColor;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
    
    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeIn());
    }
    
    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(FadeIn());
    }
}
