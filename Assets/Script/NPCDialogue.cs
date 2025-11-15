using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Data Dialog")]
    public DialogueNode startNode;
    public string npcName = "NPC";

    [Header("Trigger")]
    public KeyCode interactionKey = KeyCode.E;

    [Header("Prompt Icon")] // <--- DIUBAH: Dari UI ke Sprite
    // Drag objek 'InteractionIcon' (anak dari NPC) ke sini
    public GameObject interactionPromptSprite; 

    private DialogueManager dialogueManager;
    private bool playerInRange = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        
        // Pastikan ikon tersembunyi saat game dimulai
        if (interactionPromptSprite != null)
        {
            interactionPromptSprite.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            // Mulai dialog
            dialogueManager.StartDialogue(startNode, npcName); 
            
            // Sembunyikan ikon saat dialog dimulai
            if (interactionPromptSprite != null)
            {
                interactionPromptSprite.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            // Tampilkan ikon saat pemain masuk
            if (interactionPromptSprite != null)
            {
                interactionPromptSprite.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            // Sembunyikan ikon saat pemain keluar
            if (interactionPromptSprite != null)
            {
                interactionPromptSprite.SetActive(false);
            }
        }
    }
}