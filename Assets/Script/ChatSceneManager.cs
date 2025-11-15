using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ChatSceneManager : MonoBehaviour
{
    [Header("Referensi Scene")]
    public DialogueNode startNode; // Node dialog pertama
    public Transform chatContentPanel; // Objek 'Content' dari Scroll View
    public GameObject choicePanel; // Panel 'ChoicePanel'
    public Transform choiceContainer; // 'ChoiceContainer' di dalam ChoicePanel
    public ScrollRect chatScrollRect; // Scroll View utama

    [Header("Prefab")]
    public GameObject leftBubblePrefab; // Prefab gelembung kiri (NPC/Mawar)
    public GameObject rightBubblePrefab; // Prefab gelembung kanan (Player/Maya)
    public GameObject choiceButtonPrefab; // Prefab tombol pilihan

    [Header("Pengaturan Teks")]
    public float typingSpeed = 0.02f;
    public float choiceDelay = 1.0f; // Jeda sebelum pilihan muncul

    private DialogueNode currentNode;

    void Start()
    {
        choicePanel.SetActive(false);
        if (startNode != null)
        {
            StartCoroutine(RunDialogue(startNode));
        }
    }

    // GANTI SELURUH COROUTINE INI
    private IEnumerator RunDialogue(DialogueNode node)
    {
        currentNode = node;

        // --- Cek khusus untuk "Node Kosong" Pemicu Scene ---
        // Jika ini node pemicu scene yang tidak ada teksnya
        if (string.IsNullOrEmpty(node.dialogueText) && node.endsSceneOnComplete)
        {
            LoadNextScene(node.sceneToLoad);
            yield break; // Hentikan coroutine di sini
        }
        // ----------------------------------------------------

        // 1. Pilih prefab bubble yang benar (kiri/kanan)
        GameObject prefabToUse = (node.speaker == DialogueSpeaker.NPC) ? leftBubblePrefab : rightBubblePrefab;
        
        // 2. Buat bubble baru dan taruh di panel 'Content'
        GameObject newBubble = Instantiate(prefabToUse, chatContentPanel);
        TextMeshProUGUI textComponent = newBubble.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = "";

        // 3. Efek Ketik
        char[] characters = node.dialogueText.ToCharArray();
        foreach (char c in characters)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        // 4. Paksa scroll ke bawah
        yield return new WaitForEndOfFrame();
        chatScrollRect.verticalNormalizedPosition = 0f;

        // 5. Tentukan apa selanjutnya
        // A. Jika ada Pilihan
        if (node.choices.Count > 0)
        {
            yield return new WaitForSecondsRealtime(choiceDelay);
            choicePanel.SetActive(true);
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (DialogueChoice choice in node.choices)
            {
                CreateChoiceButton(choice);
            }
        }
        // B. Jika Lanjut Otomatis
        else if (node.autoContinueNode != null)
        {
            yield return new WaitForSecondsRealtime(choiceDelay);
            StartCoroutine(RunDialogue(node.autoContinueNode));
        }
        // C. Jika Selesai
        else
        {
            // --- MODIFIKASI UTAMA DI SINI ---
            // Cek apakah node terakhir ini memicu pindah scene
            if (node.endsSceneOnComplete && !string.IsNullOrEmpty(node.sceneToLoad))
            {
                // Beri jeda sedikit agar pemain bisa membaca teks terakhir
                yield return new WaitForSecondsRealtime(choiceDelay);
                LoadNextScene(node.sceneToLoad);
            }
            else
            {
                // Dialog berakhir
                Debug.Log("Skenario chat selesai.");
            }
            // --- AKHIR MODIFIKASI ---
        }
    }

    void CreateChoiceButton(DialogueChoice choice)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

        buttonObj.GetComponent<Button>().onClick.AddListener(() => {
            SelectChoice(choice);
        });
    }

    void SelectChoice(DialogueChoice choice)
    {
        choicePanel.SetActive(false); // Sembunyikan pilihan
        
        // 1. Buat bubble untuk JAWABAN yang kita pilih
        GameObject choiceBubble = Instantiate(rightBubblePrefab, chatContentPanel);
        choiceBubble.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

        // 2. Paksa scroll ke bawah
        StartCoroutine(ScrollToBottom());

        // 3. Cek apakah ada kelanjutan
        if (choice.nextNode != null)
        {
            StartCoroutine(RunDialogue(choice.nextNode));
        }
        else
        {
            // Scene berakhir
            Debug.Log("Pilihan mengakhiri scene.");
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }

    // --- TAMBAHKAN FUNGSI BARU INI ---
    // GANTI FUNGSI LAMA-MU DENGAN YANG INI
    void LoadNextScene(string sceneName)
    {
        Debug.Log($"Node ini memicu pemuatan scene: {sceneName}");
        
        // Langsung pindah scene, tanpa fade
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    // ---------------------------------
}