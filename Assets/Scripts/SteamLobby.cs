using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamLobby : MonoBehaviour
{
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private const string HostAddressKey = "HostAddress";

    public NetworkManager networkManager;

    public static SteamLobby instance;

    public CSteamID currentLobbyID;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        if (networkManager == null) { Debug.Log("NULL"); }

        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }
    public void HostLobby()
    {
        SceneManager.LoadScene("Game");
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());

    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = (CSteamID)callback.m_ulSteamIDLobby;
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);


        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
    }

    public void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(currentLobbyID);
    }
}
