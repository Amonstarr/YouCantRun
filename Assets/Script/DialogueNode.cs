using System.Collections.Generic;
using UnityEngine;

// --- TAMBAHKAN ENUM INI DI LUAR KELAS ---
public enum DialogueSpeaker { NPC, Player } // NPC = Kiri, Player = Kanan
// ----------------------------------------

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    // --- TAMBAHKAN VARIABEL INI ---
    public DialogueSpeaker speaker; // Siapa yang berbicara di node ini
    // --------------------------------
    
    [Header("Visual Karakter")]
    public Sprite characterPortrait; // Kita tidak pakai ini di scene chat
    public bool flipPortrait = false; // Kita tidak pakai ini

    [TextArea(3, 10)]
    public string dialogueText;
    public List<DialogueChoice> choices;

    [Header("Lanjut Otomatis (Jika Pilihan Kosong)")]
    public DialogueNode autoContinueNode;
    public bool unlocksExitOnComplete = false;

    // --- TAMBAHKAN DUA BARIS DI BAWAH INI ---
    [Header("Pengaturan Pindah Scene")]
    public bool endsSceneOnComplete = false; // Centang ini jika node ini mengakhiri scene
    public string sceneToLoad = ""; // Nama scene yang akan dimuat
    // -----------------------------------------
}