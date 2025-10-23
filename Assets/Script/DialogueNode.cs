// DialogueNode.cs
// (Tidak perlu di-attach ke GameObject manapun)
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText;          // Teks yang diucapkan NPC
    public List<DialogueChoice> choices; // Daftar pilihan untuk pemain
}