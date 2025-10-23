// NPCDialogue.cs
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueNode startNode; // Drag node dialog pertamamu ke sini
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
            dialogueManager.StartDialogue(startNode);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Opsional: Tampilkan UI "Tekan E"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Opsional: Sembunyikan UI "Tekan E"
        }
    }
}