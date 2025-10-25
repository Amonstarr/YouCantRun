using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    
    [Header("Settings")]
    [SerializeField] private string firstSceneName = "Game1";
    [SerializeField] private float transitionDelay = 0.5f;
    
    private void Start()
    {
        // Pastikan main menu aktif di awal
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        
        // Setup button listeners
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        if (backButton != null)
            backButton.onClick.AddListener(BackToMainMenu);
    }
    
    //  pindah ke scene pertama
    public void PlayGame()
    {
        Debug.Log("Starting game...");
        StartCoroutine(LoadSceneWithDelay(firstSceneName, transitionDelay));
    }
    
    // Buka options
    public void OpenOptions()
    {
        Debug.Log("Opening options...");
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }
    
    // Kembali ke main menu 
    public void BackToMainMenu()
    {
        Debug.Log("Back to main menu...");
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
    
    // Keluar dari game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Load scene
    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    
    // gak tau ini apa
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
