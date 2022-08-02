using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject menuObjects;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuObjects.activeInHierarchy) menuObjects.SetActive(false);
            else
            {
                menuObjects.SetActive(true);
            }
        }
    }

    public void BackToMainMenuPressed()
    {
        if (NetworkClient.isHostClient)
        {
            NetworkManager.singleton.StopHost();
        }
        else { NetworkManager.singleton.StopClient(); }

        SteamLobby.instance.LeaveLobby();
        SceneManager.LoadScene("MainMenu");
    }
}
