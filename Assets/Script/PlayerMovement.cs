using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    // --- VARIABEL UNTUK ANIMASI & FLIPPING ---
    private Animator anim;
    private SpriteRenderer sprite;
    // ---------------------------------------------
    
    private bool isGrounded; // Variabel ini sekarang akan di-update di FixedUpdate

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // --- TAMBAHAN: Ambil komponen Animator dan SpriteRenderer ---
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        // -----------------------------------------------------------
    }

    // --- DIUBAH: Logika dipindah ke FixedUpdate() untuk Fisika & Animasi ---
    void FixedUpdate()
    {
        // 1. Cek Ground (dipindah ke sini agar selalu update)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // 2. Logika Gerakan (dipindah dari Update() ke FixedUpdate())
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        // 3. Logika Animasi (mengirim kecepatan ke Blend Tree)
        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

        // 4. Logika Membalik Sprite
        FlipSprite();
    }

    // --- DIHAPUS: Update() sudah tidak diperlukan ---
    // void Update() { ... }

    void Jump()
    {
        // --- DIUBAH: Menggunakan variabel 'isGrounded' ---
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // --- DIHAPUS: Fungsi IsGrounded() digabung ke FixedUpdate ---
    // bool IsGrounded() { ... }

    // --- FUNGSI BARU UNTUK MEMBALIK SPRITE ---
    void FlipSprite()
    {
        // Jika bergerak ke kanan
        if (moveInput.x > 0.1f)
        {
            sprite.flipX = false; // Menghadap kanan
        }
        // Jika bergerak ke kiri
        else if (moveInput.x < -0.1f)
        {
            sprite.flipX = true; // Balik sprite (menghadap kiri)
        }
    }
}