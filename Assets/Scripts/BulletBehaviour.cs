using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Destroy(this.gameObject, 5f);
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(transform.position + 10f * Time.fixedDeltaTime * transform.up);
    }
}
