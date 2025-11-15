using UnityEngine;
using UnityEngine.SceneManagement; // <-- KITA BUTUH INI LAGI

public class SceneChanger : MonoBehaviour
{
    [Header("Pengaturan Pindah Scene")]
    public string sceneToLoad;
    
    [Header("UI Peringatan")]
    public GameObject lockedPrompt; 
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // 1. Cek bendera di GameManager
            if (GameManager.Instance.canExitLevel)
            {
                // Jika "kunci" sudah ada, pindah scene
                hasTriggered = true; // Kunci trigger
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    // --- MODIFIKASI DI SINI ---
                    // Langsung pindah scene, tanpa fade
                    SceneManager.LoadScene(sceneToLoad);
                    // --------------------------
                }
                else
                {
                    Debug.LogWarning("Nama scene di SceneChanger belum diisi!", this.gameObject);
                }
            }
            else
            {
                // 2. Jika "kunci" BELUM ada
                Debug.Log("Pintu masih terkunci. Bicara dengan NPC dulu.");
                
                if (lockedPrompt != null)
                {
                    lockedPrompt.SetActive(true);
                }
            }
        }
    }

    // Sembunyikan pesan "Terkunci" saat pemain pergi
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (lockedPrompt != null)
            {
                lockedPrompt.SetActive(false);
            }
        }
    }
}