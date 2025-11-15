// EndingTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement; // <-- JANGAN LUPA!

public class EndingTrigger : MonoBehaviour
{
    [Header("Nama Scene Ending")]
    public string happyEndingSceneName = "HappyEnd";
    public string sadEndingSceneName = "SadEnd";

    [Header("Batas Skor")]
    public int happyEndingThreshold = 0; // Jika skor > 0, dapat happy ending

    // Variabel untuk memastikan trigger hanya jalan sekali
    private bool hasBeenTriggered = false;
        
    // Gunakan ini jika pemicunya adalah Collider (misal, pintu)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika yang masuk adalah Player dan belum pernah dipicu
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true; // Tandai sudah dipicu
            CheckEnding();
        }
    }

    // Kamu juga bisa memanggil fungsi ini dari event lain (misal, tombol)
    public void CheckEnding()
    {
        // 1. Ambil skor final dari GameManager
        int finalScore = GameManager.Instance.poinEnding;

        Debug.Log($"Pengecekan Ending Dimulai. Skor Final: {finalScore}");

        // 2. Tentukan scene yang akan di-load
        if (finalScore > happyEndingThreshold)
        {
            // Muat Happy Ending
            Debug.Log("Hasil: Happy Ending");
            SceneManager.LoadScene(happyEndingSceneName);
        }
        else
        {
            // Muat Sad Ending (skor 0 atau negatif)
            Debug.Log("Hasil: Sad Ending");
            SceneManager.LoadScene(sadEndingSceneName);
        }
    }
}