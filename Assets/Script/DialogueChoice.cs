// DialogueChoice.cs
// (Tidak perlu di-attach ke GameObject manapun)

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;       // Teks yang muncul di tombol
    public int pointsValue;         // Poin (+1, -1, 0)
    public DialogueNode nextNode;     // Node selanjutnya jika ini dipilih
}