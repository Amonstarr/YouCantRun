// DialogueManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    // --- Referensi UI ---
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Transform choiceContainer; // GameObject dengan Vertical Layout Group
    public GameObject choiceButtonPrefab; // Prefab tombol yang kamu buat

    private DialogueNode currentNode;

    // Fungsi ini dipanggil oleh NPC untuk memulai dialog
    public void StartDialogue(DialogueNode startNode)
    {
        dialoguePanel.SetActive(true);
        // Time.timeScale = 0f; // Opsional: Pause game
        ShowNode(startNode);
    }

    // Menampilkan node dialog ke UI
    private void ShowNode(DialogueNode node)
    {
        currentNode = node;
        dialogueText.text = node.dialogueText;

        // 1. Bersihkan tombol-tombol pilihan lama
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Cek apakah ini akhir dialog (tidak ada pilihan)
        if (node.choices.Count == 0)
        {
            // Tampilkan tombol "Selesai" atau tutup otomatis
            // (Untuk simpelnya, kita panggil EndDialogue() di tombol custom)
            CreateChoiceButton("Selesai", 0, null); // Tombol "Selesai"
            return;
        }

        // 3. Buat tombol baru untuk setiap pilihan
        foreach (DialogueChoice choice in node.choices)
        {
            CreateChoiceButton(choice.choiceText, choice.pointsValue, choice.nextNode);
        }
    }

    // Fungsi untuk membuat satu tombol pilihan
    private void CreateChoiceButton(string text, int points, DialogueNode nextNode)
    {
        // Buat tombol dari prefab
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
        
        // Set teks tombol
        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // Tambahkan listener (apa yang terjadi saat diklik)
        buttonObj.GetComponent<Button>().onClick.AddListener(() => {
            SelectChoice(points, nextNode);
        });
    }

    // Dipanggil saat pemain mengklik sebuah pilihan
    private void SelectChoice(int points, DialogueNode nextNode)
    {
        // 1. Tambahkan/Kurangi poin global
        GameManager.Instance.poinEnding += points;

        // 2. Cek apakah ini akhir percakapan
        if (nextNode == null)
        {
            EndDialogue();
        }
        else
        {
            // 3. Lanjut ke node selanjutnya
            ShowNode(nextNode);
        }
    }

    // Menutup panel dialog
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        // Time.timeScale = 1f; // Opsional: Un-pause game
    }
}