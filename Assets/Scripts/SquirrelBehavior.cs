using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehavior : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float moveSpeed; // Speed at which the squirrel moves towards the player
    public GameObject hazelnutProjectile; // Reference to the hazelnut projectile
    private int hitCount = 0;
    private int maxHits = 3;

    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move towards the player
        Vector2 position = Vector2.MoveTowards(rigidbody2D.position, player.position, moveSpeed * Time.deltaTime);
        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage player
            // Implement player damage logic here
        }

        // Check for collision with hazelnut
        if (collision.gameObject.CompareTag("Hazelnut"))
        {
            hitCount++;
            if (hitCount < maxHits)
            {
                RespawnSquirrel();
            }
            else
            {
                // Optionally destroy or deactivate the squirrel permanently
                gameObject.SetActive(false);
            }
        }
    }

    void RespawnSquirrel()
    {
        // Deactivate squirrel
        gameObject.SetActive(false);

        // Wait for a short duration if needed

        // Reposition squirrel at a new location
        // Implement logic to choose a new location here

        // Reactivate squirrel
        gameObject.SetActive(true);
    }
}

