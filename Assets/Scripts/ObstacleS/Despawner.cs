using UnityEngine;

public class Despawner : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
}
