// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int poinEnding = 0;

    // --- PAUSE MENU LOGIC ---
    [Header("Pause Menu")]
    public GameObject pauseMenuPanel; // Panel yang berisi 'Resume', 'Options', dll.
    public GameObject pauseLogoButton;  // <--- TAMBAHAN BARU: Tombol logo di UI
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    // -------------------------

    private void Awake()
    {
        // Ini adalah pola Singleton sederhana
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jaga agar tidak hancur saat ganti scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi untuk menghentikan game
    // Ini akan dipanggil oleh 'PauseLogoButton'
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);   // Tampilkan menu
        pauseLogoButton.SetActive(false); // Sembunyikan logo
        Time.timeScale = 0f;            // HENTIKAN WAKTU GAME
    }

    // Fungsi untuk melanjutkan game (untuk tombol Resume)
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);  // Sembunyikan menu
        pauseLogoButton.SetActive(true);  // Tampilkan lagi logonya
        Time.timeScale = 1f;            // Kembalikan waktu game
    }

    // Fungsi untuk tombol Main Menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    // Fungsi untuk tombol Options (placeholder)
    public void OpenOptions()
    {
        Debug.Log("Tombol Options diklik!");
    }
}