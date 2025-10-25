using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Data Dialog")]
    public DialogueNode startNode; // Drag node dialog pertamamu ke sini
    public string npcName = "NPC";   // <--- TAMBAHAN BARU

    [Header("Trigger")]
    public KeyCode interactionKey = KeyCode.E;
    
    private DialogueManager dialogueManager;
    private bool playerInRange = false;

    private void Start()
    {
        // Cari DialogueManager saat mulai
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        // Cek jika pemain di dalam jangkauan DAN menekan tombol
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            // --- MODIFIKASI DI SINI ---
            // Kita sekarang mengirim 'npcName' saat memulai dialog
            dialogueManager.StartDialogue(startNode, npcName); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}