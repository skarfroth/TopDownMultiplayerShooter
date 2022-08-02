using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public float movementSpeed = 5.0f;
    private Rigidbody2D rb2d;
    private Vector2 movementVector;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (hasAuthority)
            {
                movementVector.x = Input.GetAxis("Horizontal");
                movementVector.y = Input.GetAxis("Vertical");
                Rotation();
            }
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Game" && hasAuthority)
        {
            Movement();
        }
    }

    public void Movement()
    {
        rb2d.velocity = Vector2.ClampMagnitude(movementVector, 1) * movementSpeed;
    }

    public void SetPosition()
    {
        transform.position = Vector2.zero;
    }

    public void Rotation()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
