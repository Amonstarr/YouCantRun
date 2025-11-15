using UnityEngine;
using UnityEngine.UI; // <-- Jangan lupa
using TMPro;          // <-- Jangan lupa
using System.Collections; // <-- Jangan lupa

public class SceneIntroManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public Image fadePanel; // Drag FadePanel hitam ke sini
    public TextMeshProUGUI objectiveText; // Drag ObjectiveText ke sini

    [Header("Pengaturan Waktu")]
    public float fadeInTime = 1.5f; // Durasi fade-in dari hitam
    public float textFadeInTime = 1.0f; // Durasi teks muncul
    public float textDisplayTime = 3.0f; // Durasi teks diam di layar
    public float textFadeOutTime = 1.0f; // Durasi teks menghilang

    void Start()
    {
        // Set teks yang diinginkan
        objectiveText.text = "Temui Mr. Orchid";

        // Pastikan status awalnya benar
        fadePanel.color = Color.black;
        objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 0);

        // Mulai urutan animasinya
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        // 1. Fade-in (panel hitam memudar jadi transparan)
        yield return StartCoroutine(FadeImage(fadePanel, 1f, 0f, fadeInTime));
        
        // Nonaktifkan panel agar tidak memblokir apa-apa
        fadePanel.gameObject.SetActive(false);

        // 2. Teks Objektif Muncul (fade-in)
        yield return StartCoroutine(FadeText(objectiveText, 0f, 1f, textFadeInTime));

        // 3. Teks Diam di Layar
        yield return new WaitForSeconds(textDisplayTime);

        // 4. Teks Objektif Hilang (fade-out)
        yield return StartCoroutine(FadeText(objectiveText, 1f, 0f, textFadeOutTime));
    }

    // Coroutine helper untuk fade Image
    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    // Coroutine helper untuk fade Teks (TextMeshPro)
    private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }
}