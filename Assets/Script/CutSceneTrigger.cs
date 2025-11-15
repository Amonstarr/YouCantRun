using UnityEngine;
using UnityEngine.Playables; // Wajib untuk Playable Director
using UnityEngine.InputSystem; // Wajib untuk menonaktifkan input

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Referensi")]
    // Drag Playable Director dari objek ini ke sini
    public PlayableDirector playableDirector;
    
    // GameObject yang akan menonaktifkan input (misal Player)
    public GameObject playerToControl;

    [Header("Pengaturan Pemicu")]
    [Tooltip("Centang ini jika cutscene ingin otomatis main saat scene dimulai")]
    public bool triggerOnStart = false; // "Saklar" baru kita

    // Variabel privat untuk melacak status
    private bool hasTriggered = false;

    private void Start()
    {
        // Ambil PlayableDirector secara otomatis jika belum di-set
        if (playableDirector == null)
            playableDirector = GetComponent<PlayableDirector>();

        // Berlangganan event "stopped". 
        // Saat Timeline berhenti, panggil fungsi OnCutsceneEnd
        playableDirector.stopped += OnCutsceneEnd;

        // --- LOGIKA UNTUK CUTSCENE DI AWAL ---
        // Jika 'triggerOnStart' dicentang, langsung mulai cutscene
        if (triggerOnStart)
        {
            hasTriggered = true; // Langsung tandai sudah dipicu
            StartCutscene();
        }
    }

    // --- LOGIKA UNTUK CUTSCENE DI TANGGUL ---
    // Fungsi trigger ini sekarang HANYA akan jalan jika 'triggerOnStart' TIDAK dicentang
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jangan jalankan jika sudah dipicu atau jika ini adalah 'triggerOnStart'
        if (hasTriggered || triggerOnStart) return;

        // Hanya picu jika yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCutscene();
        }
    }

    // Fungsi utama untuk memulai cutscene
    public void StartCutscene()
    {
        // 1. Ambil alih kontrol
        if (playerToControl != null)
        {
            // Nonaktifkan skrip gerakan
            var movementScript = playerToControl.GetComponent<PlayerMovement>();
            if (movementScript != null) movementScript.enabled = false;

            // Nonaktifkan PlayerInput agar tidak bisa lompat, dll.
            var playerInput = playerToControl.GetComponent<PlayerInput>();
            if (playerInput != null) playerInput.enabled = false;
        }

        // 2. Mulai Timeline
        playableDirector.Play();
    }

    // Fungsi ini akan dipanggil otomatis saat Timeline selesai
    // karena kita sudah mendaftarkannya di event 'playableDirector.stopped'
    void OnCutsceneEnd(PlayableDirector director)
    {
        // 3. Kembalikan kontrol ke pemain
        if (playerToControl != null)
        {
            // Aktifkan lagi skrip gerakan
            var movementScript = playerToControl.GetComponent<PlayerMovement>();
            if (movementScript != null) movementScript.enabled = true;

            // Aktifkan lagi PlayerInput
            var playerInput = playerToControl.GetComponent<PlayerInput>();
            if (playerInput != null) playerInput.enabled = true;
        }

        // 4. (Opsional) Nonaktifkan objek ini agar tidak bisa dipakai lagi
        // Kita tidak lakukan ini untuk 'triggerOnStart'
        if (!triggerOnStart)
        {
            gameObject.SetActive(false);
        }
    }
}