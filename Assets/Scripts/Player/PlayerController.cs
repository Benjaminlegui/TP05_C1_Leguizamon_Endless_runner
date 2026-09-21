using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Collider2D ground;
    private Rigidbody2D body;
    private BoxCollider2D playerCollider;
    private GameManager gameManager;
    private Vector2 startPosition;
    private bool jumpRequested;
    private bool waitingForLanding;
    public event System.Action Jumped;
    public event System.Action Landed;
    public float LeftEdge => playerCollider.bounds.min.x;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        startPosition = body.position;
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = playerData.gravityScale;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        StopPlayer();
    }

    private void Update()
    {
        if (gameManager == null || !gameManager.IsPlaying || Time.timeScale == 0f || Keyboard.current == null) return;
        var keyboard = Keyboard.current;
        if (keyboard.spaceKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame ||
            keyboard.upArrowKey.wasPressedThisFrame) jumpRequested = true;
    }

    private void FixedUpdate()
    {
        if (gameManager == null || !gameManager.IsPlaying) return;
        if (jumpRequested && ground != null && playerCollider.IsTouching(ground) && body.linearVelocity.y <= 0.1f)
        {
            body.linearVelocity = new Vector2(0f, playerData.jumpSpeed);
            waitingForLanding = true;
            Jumped?.Invoke();
        }
        jumpRequested = false;
    }

    public void ResetPlayer()
    {
        body.position = startPosition;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        jumpRequested = false;
        body.simulated = true;
        waitingForLanding = false;
    }

    public void StopPlayer()
    {
        body.linearVelocity = Vector2.zero;
        body.simulated = false;
        jumpRequested = false;
        waitingForLanding = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager != null && collision.collider.GetComponentInParent<Obstacle>() != null)
        {
            gameManager.EndGame();
            return;
        }
        if (waitingForLanding && gameManager != null && gameManager.IsPlaying && collision.collider == ground)
        {
            waitingForLanding = false;
            Landed?.Invoke();
        }
    }
}
