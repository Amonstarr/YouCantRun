// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int poinEnding = 0;

    private void Awake()
    {
        // Ini adalah pola Singleton sederhana
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jaga agar tidak hancur saat ganti scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
}