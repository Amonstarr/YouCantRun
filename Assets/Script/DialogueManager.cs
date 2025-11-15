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
    public TextMeshProUGUI nameText; // <--- TAMBAHAN BARU: Referensi UI Nama
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    [Header("Speaker Names")]
    public string playerName = "Player"; // <--- TAMBAHAN BARU: Nama Player

    [Header("Timing & Efek")]
    public float typingSpeed = 0.02f;
    public float playerChoiceDelay = 0.75f;
    public float autoEndDelay = 2.0f;

    private DialogueNode currentNode;
    private Coroutine currentTypingCoroutine;
    private string currentNpcName; // <--- TAMBAHAN BARU: Menyimpan nama NPC

    // --- MODIFIKASI: Menangkap 'speakerName' ---
    public void StartDialogue(DialogueNode startNode, string speakerName)
    {
        dialoguePanel.SetActive(true);
        currentNpcName = speakerName; // Simpan nama NPC
        // Time.timeScale = 0f;
        
        ShowNode(startNode);
    }

    private void ShowNode(DialogueNode node)
    {
        StopAllCoroutines();
        currentNode = node;

        nameText.text = currentNpcName; // <--- TAMBAHAN BARU: Set nama NPC

        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        currentTypingCoroutine = StartCoroutine(TypeText(node));
    }

    private IEnumerator TypeText(DialogueNode node)
    {
        dialogueText.text = "";
        char[] characters = node.dialogueText.ToCharArray();

        foreach (char c in characters)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (node.choices.Count == 0)
        {
            StartCoroutine(AutoEndDialogue());
        }
        else
        {
            foreach (DialogueChoice choice in node.choices)
            {
                CreateChoiceButton(choice.choiceText, choice.pointsValue, choice.nextNode);
            }
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
        nameText.text = playerName; // <--- TAMBAHAN BARU: Set nama Player

        dialogueText.text = "";
        
        // --- MODIFIKASI: Tanda ">" DIHILANGKAN ---
        string playerText = choiceText; 

        char[] characters = playerText.ToCharArray();
        foreach (char c in characters)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(playerChoiceDelay);

        if (nextNode == null)
        {
            StartCoroutine(AutoEndDialogue());
        }
        else
        {
            ShowNode(nextNode); // Panggil ShowNode untuk respon NPC
        }
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        if (nameText != null) nameText.text = ""; // Bersihkan nama saat selesai
        //Time.timeScale = 1f;
    }

    private IEnumerator AutoEndDialogue()
    {
        yield return new WaitForSeconds(autoEndDelay);
        EndDialogue();
    }
}