using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Obstacle : MonoBehaviour
{
    private Rigidbody2D obstacleBody;
    private Collider2D obstacleCollider;
    private GameManager gameManager;
    private bool scored;

    private void Awake()
    {
        obstacleBody = GetComponent<Rigidbody2D>();
        obstacleCollider = GetComponent<Collider2D>();
    }

    public void Prepare(GameManager manager, Vector2 position)
    {
        gameManager = manager;
        scored = false;
        transform.position = position;
        obstacleBody.position = position;
        obstacleBody.linearVelocity = Vector2.zero;
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (gameManager == null || !gameManager.IsPlaying) return;
        obstacleBody.MovePosition(obstacleBody.position + Vector2.left *
            (Time.fixedDeltaTime * gameManager.CurrentSpeed));
    }

    public void StopMovement()
    {
        obstacleBody.linearVelocity = Vector2.zero;
        obstacleBody.angularVelocity = 0f;
    }

    private void LateUpdate()
    {
        if (gameManager == null || !gameManager.IsPlaying || scored) return;
        
        if (obstacleCollider.bounds.max.x < gameManager.Player.LeftEdge)
        {
            scored = true;
            gameManager.AddScore();
        }
    }
}
