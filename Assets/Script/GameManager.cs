using UnityEngine;
using UnityEngine.SceneManagement; // <-- TAMBAHKAN INI

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int poinEnding = 0;

    // --- PAUSE MENU LOGIC ---
    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public GameObject pauseLogoButton;
    public string mainMenuSceneName = "MainMenuScene";
    private bool isPaused = false;
    
    // --- TAMBAHAN BARU UNTUK SYARAT PINDAH SCENE ---
    [Header("Quest Flags")]
    public bool canExitLevel = false; // "Bendera" kita
    // --------------------------------------------------

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // --- TAMBAHAN BARU ---
            // Berlangganan event "sceneLoaded"
            // Ini agar kita bisa mereset flag di level baru
            SceneManager.sceneLoaded += OnSceneLoaded;
            // ---------------------
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- TAMBAHKAN FUNGSI BARU DI BAWAH INI ---

    // Dipanggil setiap kali scene baru selesai dimuat
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset flag setiap kali masuk scene baru
        // Ini penting agar di level 2 kamu harus bicara dgn NPC lagi
        canExitLevel = false; 
    }

    // Fungsi ini akan kita panggil dari dialog untuk "membuka kunci"
    public void UnlockExit()
    {
        canExitLevel = true;
        Debug.Log("Pintu Keluar SUDAH DIBUKA!");
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
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    // Fungsi untuk tombol Options (placeholder)
    public void OpenOptions()
    {
        Debug.Log("Tombol Options diklik!");
    }
}