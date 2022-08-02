using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (hasAuthority && Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation);
        NetworkServer.Spawn(bulletClone);
    }
}
