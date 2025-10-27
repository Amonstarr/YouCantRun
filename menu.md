### 1. Setup Main Menu Scene

1. Buka scene MainMenu.unity
2. Buat GameObject baru dengan nama "MainMenuManager"
3. Attach script MainMenuManager.cs ke GameObject tersebut

### 2. Buat UI Elements

Di Canvas, buat UI elements berikut:

#### Main Menu Panel
```
Canvas
├── MainMenuPanel (Panel)
│   ├── Title (TextMeshPro)
│   ├── PlayButton (Button)
│   ├── OptionsButton (Button)
│   └── QuitButton (Button)
│
└── OptionsPanel (Panel) - Set inactive
    ├── OptionsTitle (TextMeshPro)
    ├── MusicSlider (Slider)
    ├── SFXSlider (Slider)
    └── BackButton (Button)
```

### 3. Setup MainMenuManager Component

Di Inspector MainMenuManager:

**Menu Panels:**
- Main Menu Panel → Drag MainMenuPanel GameObject
- Options Panel → Drag OptionsPanel GameObject

**Buttons:**
- Play Button → Drag PlayButton
- Options Button → Drag OptionsButton
- Quit Button → Drag QuitButton
- Back Button → Drag BackButton (dari OptionsPanel)

**Settings:**
- First Scene Name → "Game1" (atau nama scene pertama game Anda)
- Transition Delay → 0.5

### 4. Setup Scene Transition (Opsional)

Untuk efek fade transition yang smooth:

1. Buat GameObject baru "SceneTransition"
2. Attach script **SceneTransition.cs**
3. Di Canvas, buat **Image** baru (FadeImage):
   - Set Anchor ke **Stretch** semua
   - Set Color ke **Black** dengan Alpha **255**
   - Set Image ke posisi paling atas (render terakhir)
4. Drag FadeImage ke field "Fade Image" di SceneTransition
5. Set Fade Duration (default: 1 detik)

### 5. Setup Audio Manager (Opsional)

1. Buat GameObject baru "AudioManager"
2. Attach script **AudioManager.cs**
3. Tambahkan 2 AudioSource components:
   - Satu untuk Music → Drag ke "Music Source"
   - Satu untuk SFX → Drag ke "Sfx Source"
4. Drag audio clips Anda ke field yang sesuai

### 6. Build Settings

Pastikan scene sudah ditambahkan ke Build Settings:

1. File → Build Settings
2. Klik "Add Open Scenes" untuk menambahkan scene
3. Urutan scene:
   - 0: MainMenu
   - 1: Game1
   - 2: Game2

---

## 🔧 Cara Menggunakan Scripts

### MainMenuManager

Script ini otomatis setup button listeners. Anda juga bisa memanggil method-nya manual:

```csharp
// Di script lain
MainMenuManager mainMenu = FindObjectOfType<MainMenuManager>();
mainMenu.PlayGame();        // Mulai game
mainMenu.OpenOptions();     // Buka options
mainMenu.QuitGame();        // Keluar game
```

### SceneTransition

```csharp
// Load scene dengan fade effect
SceneTransition.LoadSceneWithFade("Game1");
SceneTransition.LoadSceneWithFade(1); // By index

// Fade in manual
SceneTransition.FadeInScene();
```

### SceneMovement

Attach ke GameObject atau gunakan sebagai helper:

```csharp
SceneMovement sceneMovement = FindObjectOfType<SceneMovement>();
sceneMovement.LoadScene("Game1");
sceneMovement.LoadNextScene();
sceneMovement.LoadMainMenu();
sceneMovement.ReloadCurrentScene();
```

### AudioManager

```csharp
// Play music
AudioManager.PlayMusic(myMusicClip);

// Play SFX
AudioManager.PlaySFX(mySFXClip);
AudioManager.PlayButtonClick();

// Volume control
AudioManager.SetMusicVolume(0.5f);
AudioManager.SetSFXVolume(0.7f);

// Pause/Resume
AudioManager.PauseMusic();
AudioManager.ResumeMusic();
```

---

## 🎨 Tips Desain Main Menu

1. **Button Hover Effects**: Tambahkan hover effect di Button component
2. **Button Sounds**: Tambahkan AudioManager.PlayButtonClick() di OnClick events
3. **Background**: Tambahkan background image atau animasi
4. **Title Animation**: Gunakan DOTween atau Animator untuk animasi title
5. **Particle Effects**: Tambahkan particle system untuk efek visual

---

## Checklist

- [done] UI Elements sudah dibuat
- [done] MainMenuManager sudah di-setup
- [done] Buttons sudah di-link ke MainMenuManager
- [done] Scene sudah ditambahkan ke Build Settings
- [nanti] Transition effect sudah di-test
- [nanti] Audio sudah di-setup (opsional)
- [done] Test play di Editor

---

## 🐛 Troubleshooting

**Button tidak berfungsi:**
- Pastikan EventSystem ada di scene
- Cek apakah Button component memiliki OnClick events
- Pastikan Raycast Target di Image component aktif

**Scene tidak load:**
- Cek nama scene di Build Settings
- Pastikan scene name di MainMenuManager benar
- Lihat Console untuk error messages

**Fade tidak muncul:**
- Pastikan FadeImage di Canvas
- Cek apakah Image component ada
- Pastikan Canvas Scaler sudah di-setup

---
*note*
option belum inactive