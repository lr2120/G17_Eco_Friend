using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerHealthAndScore : MonoBehaviour
{
    [Tooltip("The score value of a coin or pickup.")]
    public int coinValue = 5;
    [Tooltip("The amount of points a player loses on death.")]
    public int deathPenalty = 20;
    public TMP_Text scoreText;
    public TMP_Text timerText; // UI Text element for the timer

    // Health indicators for the player
    public GameObject health1;
    public GameObject health2;
    public GameObject health3;
    public GameObject health4;
    public GameObject health5;

    [Tooltip("Optional sound effect that plays when a coin is picked up.")]
    public AudioClip Chirp;

    [Tooltip("Duration of invincibility after taking damage.")]
    public float invincibilityDuration = 2f;
    private bool isInvincible = false;

    private float timer = 0.0f; // Timer variable
    private static int playerScore;
    private Transform respawn;

    void Start()
    {
       // respawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        scoreText.text = playerScore.ToString("D4");

        // Initialize player health
        health1.SetActive(true);
        health2.SetActive(true);
        health3.SetActive(true);
        health4.SetActive(false);
        health5.SetActive(false);
    }

    void Update()
    {
        // Timer update
        timer += Time.deltaTime;
        timerText.text = "Time: " + FormatTime(timer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Respawn();
        }
        else if (collision.CompareTag("Coin"))
        {
            AddPoints(coinValue);
            Destroy(collision.gameObject);

            if (Chirp)
            {
                AudioSource.PlayClipAtPoint(Chirp, transform.position);
            }
        }
        else if (collision.CompareTag("Finish"))
        {
            Time.timeScale = 0;
        }
        else if (collision.CompareTag("Health"))
        {
            AddHealth();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EnemyProjectile"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        if (health5.activeInHierarchy)
        {
            health5.SetActive(false);
        }
        else if (health4.activeInHierarchy)
        {
            health4.SetActive(false);
        }
        else if (health3.activeInHierarchy)
        {
            health3.SetActive(false);
        }
        else if (health2.activeInHierarchy)
        {
            health2.SetActive(false);
        }
        else
        {
            health1.SetActive(false);
            Respawn();
        }

        StartCoroutine(BecomeInvincible());
    }

    private void AddHealth()
    {
        if (!health2.activeInHierarchy)
        {
            health2.SetActive(true);
        }
        else if (!health3.activeInHierarchy)
        {
            health3.SetActive(true);
        }
        else if (!health4.activeInHierarchy)
        {
            health4.SetActive(true);
        }
        else if (!health5.activeInHierarchy)
        {
            health5.SetActive(true);
        }
    }

    public void Respawn()
    {
        Debug.Log("Player Respawn called");

        health1.SetActive(true);
        health2.SetActive(true);
        health3.SetActive(true);
        health4.SetActive(false);
        health5.SetActive(false);

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.position = respawn.transform.position;

        AddPoints(-deathPenalty);
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void AddPoints(int amount)
    {
        playerScore += amount;
        playerScore = (int)Mathf.Clamp(playerScore, 0, Mathf.Infinity);
        scoreText.text = playerScore.ToString("D4");
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}

