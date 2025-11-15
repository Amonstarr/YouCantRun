using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Visual Karakter")] 
    public Sprite characterPortrait; 
    public bool flipPortrait = false; 

    [TextArea(3, 10)]
    public string dialogueText;
    public List<DialogueChoice> choices;

    // --- TAMBAHKAN BARIS DI BAWAH INI ---
    [Header("Lanjut Otomatis (Jika Pilihan Kosong)")]
    public DialogueNode autoContinueNode; // Node selanjutnya yang akan diputar
    // ------------------------------------
}