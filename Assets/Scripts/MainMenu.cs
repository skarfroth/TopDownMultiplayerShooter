using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject lobbyList;
    public GameObject mainMenuButtons;

    public void HostGamePressed()
    {
        SteamLobby.instance.HostLobby();
    }

    public void JoinGamePressed()
    {
        lobbyList.SetActive(true);
        LobbyList.instance.GetListOfLobbies();
        mainMenuButtons.SetActive(false);
    }

    public void BackFromLobbyListPressed()
    {
        lobbyList.SetActive(false);
        LobbyList.instance.DestroyOldLobbyListItems();
        mainMenuButtons.SetActive(true);
    }

    public void QuitGamePressed()
    {
        Application.Quit();
    }
}
