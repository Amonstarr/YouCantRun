using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    // --- Referensi UI ---
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    // --- TAMBAHAN BARU UNTUK POTRET KARAKTER ---
    [Header("Character Portraits")]
    public Image leftCharacterPortrait;  // Drag UI Image untuk potret kiri ke sini
    public Image rightCharacterPortrait; // Drag UI Image untuk potret kanan ke sini
    public Sprite playerPortrait;        // Drag gambar potret Player ke sini

    [Header("Timing & Efek")]
    public float typingSpeed = 0.02f;
    public float playerChoiceDelay = 0.75f;
    public float autoEndDelay = 2.0f;

    [Header("Speaker Names")]
    public string playerName = "Player";

    private DialogueNode currentNode;
    private Coroutine currentTypingCoroutine;
    private string currentNpcName;

    // --- DIUBAH: Menangkap 'speakerName' ---
    public void StartDialogue(DialogueNode startNode, string speakerName)
    {
        dialoguePanel.SetActive(true);
        currentNpcName = speakerName;
        
        // Pastikan semua potret tersembunyi di awal
        HideAllPortraits(); // <--- BARU

        ShowNode(startNode);
    }

    private void ShowNode(DialogueNode node)
    {
        StopAllCoroutines();
        currentNode = node;

        // --- TAMBAHAN BARU UNTUK MENAMPILKAN POTRET NPC ---
        nameText.text = currentNpcName;
        DisplayNpcPortrait(node.characterPortrait, node.flipPortrait); // <--- BARU

        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        currentTypingCoroutine = StartCoroutine(TypeText(node));
    }

    // GANTI SELURUH FUNGSI INI
    private IEnumerator TypeText(DialogueNode node)
    {
        dialogueText.text = "";
        char[] characters = node.dialogueText.ToCharArray();

        foreach (char c in characters)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        // Teks NPC selesai diketik. Sekarang cek apa selanjutnya.

        // 1. Cek apakah ada PILIHAN?
        if (node.choices.Count > 0)
        {
            // Ya, ada pilihan.
            
            // --- TAMBAHAN JEDA DI SINI ---
            // Beri jeda sejenak (sesuai nilai playerChoiceDelay)
            // sebelum menampilkan pilihan.
            yield return new WaitForSecondsRealtime(playerChoiceDelay);
            // -----------------------------

            // Ganti fokus ke Player
            DisplayPlayerPortrait();
            nameText.text = playerName;
            dialogueText.text = ""; // Kosongkan textbox

            // Buat tombol-tombol pilihan
            foreach (DialogueChoice choice in node.choices)
            {
                CreateChoiceButton(choice.choiceText, choice.pointsValue, choice.nextNode);
            }
        }
        // 2. Jika tidak ada pilihan, cek apakah ada LANJUTAN OTOMATIS?
        else if (node.autoContinueNode != null)
        {
            // Ya, ada node untuk dilanjut. Beri jeda sedikit agar bisa dibaca.
            yield return new WaitForSecondsRealtime(playerChoiceDelay); 
            
            // Tampilkan node selanjutnya
            ShowNode(node.autoContinueNode);
        }
        // 3. Jika tidak ada pilihan DAN tidak ada lanjutan otomatis
        else
        {
            // Ini adalah akhir dialog. Tutup otomatis.
            StartCoroutine(AutoEndDialogue());
        }
    }

    private void CreateChoiceButton(string text, int points, DialogueNode nextNode)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = text;

        buttonObj.GetComponent<Button>().onClick.AddListener(() => {
            SelectChoice(text, points, nextNode);
        });
    }

    private void SelectChoice(string choiceText, int points, DialogueNode nextNode)
    {
        StopAllCoroutines();
        GameManager.Instance.poinEnding += points;

        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        currentTypingCoroutine = StartCoroutine(ShowPlayerChoice(choiceText, nextNode));
    }

    private IEnumerator ShowPlayerChoice(string choiceText, DialogueNode nextNode)
    {
        nameText.text = playerName;
        DisplayPlayerPortrait(); // <--- BARU: Tampilkan potret Player

        dialogueText.text = "";
        string playerText = choiceText;

        char[] characters = playerText.ToCharArray();
        foreach (char c in characters)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        yield return new WaitForSecondsRealtime(playerChoiceDelay);

        if (nextNode == null)
        {
            StartCoroutine(AutoEndDialogue());
        }
        else
        {
            ShowNode(nextNode);
        }
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        if (nameText != null) nameText.text = "";
        HideAllPortraits(); // <--- BARU: Sembunyikan semua potret saat dialog selesai
    }

    private IEnumerator AutoEndDialogue()
    {
        // --- TAMBAHAN BARU DI SINI ---
        // Cek apakah node terakhir ini harus membuka pintu keluar?
        if (currentNode != null && currentNode.unlocksExitOnComplete)
        {
            // Panggil fungsi di GameManager untuk set flag-nya
            GameManager.Instance.UnlockExit();
        }
        // -----------------------------

        // Jeda sebelum dialog ditutup
        yield return new WaitForSecondsRealtime(autoEndDelay);
        
        // Panggil fungsi penutup
        EndDialogue();
    }

    // --- FUNGSI BARU UNTUK MENGONTROL POTRET ---
    private void DisplayNpcPortrait(Sprite portrait, bool flip)
    {
        HideAllPortraits(); // Sembunyikan semua potret sebelumnya

        if (portrait != null)
        {
            rightCharacterPortrait.sprite = portrait;
            rightCharacterPortrait.gameObject.SetActive(true);
            rightCharacterPortrait.transform.localScale = new Vector3(flip ? -1 : 1, 1, 1); // Flip jika perlu
        }
    }

    private void DisplayPlayerPortrait()
    {
        HideAllPortraits(); // Sembunyikan semua potret sebelumnya

        if (playerPortrait != null)
        {
            leftCharacterPortrait.sprite = playerPortrait;
            leftCharacterPortrait.gameObject.SetActive(true);
            leftCharacterPortrait.transform.localScale = Vector3.one; // Pastikan tidak ter-flip
        }
    }

    private void HideAllPortraits()
    {
        if (leftCharacterPortrait != null) leftCharacterPortrait.gameObject.SetActive(false);
        if (rightCharacterPortrait != null) rightCharacterPortrait.gameObject.SetActive(false);
    }
    // -----------------------------------------------------
}