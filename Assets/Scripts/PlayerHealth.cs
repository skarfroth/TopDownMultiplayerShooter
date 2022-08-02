using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float maxHealth = 100f;
    private TMP_Text healthText;
    private float currentHealth;
    private bool iAmDead = false;
    private float deathTimer;
    private readonly int timeToRespawn = 5;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (currentHealth <= 0)
        {
            DoDeathStuff();
        }
        if (iAmDead)
        {
            DoRespawnStuff();
        }
        if (SceneManager.GetActiveScene().name == "Game" && healthText == null)
        {
            healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        }
        if (SceneManager.GetActiveScene().name == "Game")
            healthText.text = currentHealth.ToString();
    }
    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !other.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
        {
            TakeDamage(25f);
            Destroy(other.gameObject);
        }
    }

    private void DoDeathStuff()
    {
        spriteRenderer.enabled = false;
        GetComponent<PlayerShooting>().enabled = false;
        iAmDead = true;
    }

    private void DoRespawnStuff()
    {
        deathTimer += Time.deltaTime;
        if (deathTimer > timeToRespawn)
        {
            transform.position = Vector3.zero;
            spriteRenderer.enabled = true;
            GetComponent<PlayerShooting>().enabled = true;
            currentHealth = maxHealth;
            deathTimer = 0;
            iAmDead = false;
        }
    }
}
