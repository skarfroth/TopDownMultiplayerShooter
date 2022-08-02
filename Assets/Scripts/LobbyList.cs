using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    public GameObject lobbyItemPrefab;
    public GameObject lobbyListContent;

    public static LobbyList instance;

    public CSteamID testID;
    private readonly List<CSteamID> lobbyIDs = new();
    private readonly List<GameObject> lobbyItemList = new();

    protected Callback<LobbyMatchList_t> lobbyMatchList;
    protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
        lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);
        GetFriends();
    }

    public void GetListOfLobbies() // Triggers LobbyMatchList_t callback
    {
        if (lobbyIDs.Count > 0)
        {
            lobbyIDs.Clear();
        }

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }

    private void OnGetLobbiesList(LobbyMatchList_t result)
    {
        Debug.Log("Found " + result.m_nLobbiesMatching + " lobbies!");
        if (lobbyItemList.Count > 0)
        {
            DestroyOldLobbyListItems();
        }

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID); // Triggers LobbyDataUpdate_t callback
        }
    }

    private void OnGetLobbyInfo(LobbyDataUpdate_t result)
    {
        DisplayLobbies(lobbyIDs, result);
    }

    private void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDs.Count; i++)
        {
            if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby && lobbyListContent != null)
            {
                // Debug.Log("Lobby " + i + " belongs to: " + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name"));
                GameObject newLobbyItem = Instantiate(lobbyItemPrefab) as GameObject;
                LobbyItem lobbyItemScript = newLobbyItem.GetComponent<LobbyItem>();
                lobbyItemScript.lobbyName.text = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                lobbyItemScript.friendAvatar.texture = GetSteamImageAsTexture2D(SteamFriends.GetLargeFriendAvatar(lobbyIDs[i]));
                newLobbyItem.transform.SetParent(lobbyListContent.transform);
                newLobbyItem.transform.localScale = Vector3.one;
                lobbyItemList.Add(newLobbyItem);
            }
        }
    }

    private void GetFriends()
    {
        int friends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
        if (friends == -1) { friends = 0; }

        for (int i = 0; i < friends; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll);
            testID = SteamFriends.GetFriendByIndex(0, EFriendFlags.k_EFriendFlagAll);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
            // Debug.Log("Friend: " + i + " steam id: " + friendSteamID.ToString() + " and name: " + friendName);
        }
    }

    public void DestroyOldLobbyListItems()
    {
        Debug.Log("DestroyOldLobbyListItems");
        foreach (GameObject lobbyListItem in lobbyItemList)
        {
            GameObject lobbyListItemToDestroy = lobbyListItem;
            Destroy(lobbyListItemToDestroy);
        }
        lobbyItemList.Clear();
    }
    public static Texture2D GetSteamImageAsTexture2D(int iImage)
    {
        Texture2D ret = null;
        bool bIsValid = SteamUtils.GetImageSize(iImage, out uint ImageWidth, out uint ImageHeight);

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                ret.LoadRawTextureData(Image);
                ret.Apply();
            }
        }

        return ret;
    }
}


