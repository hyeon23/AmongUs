using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static UnityEngine.Rendering.DebugUI;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{
    private static AmongUsRoomPlayer myRoomPlayer;
    public static AmongUsRoomPlayer MyRoomPlayer

    {
        get
        {
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach(var player in players)
                {
                    if (player.isOwned)
                    {
                        myRoomPlayer = player;
                    }
                }
            }
            return myRoomPlayer;
        }
    }

    //SyncVar: ��ũ��ũ ����ȭ ����
    //hook: �ش� SyncVar�� ����Ǹ�, �ڵ����� ȣ��Ǵ� �Լ�
    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(oldColor);
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(newColor);
    }

    [SyncVar]
    public string nickname;


    //Lobby Player Character ĳ���͸� LobbyCharacterMover�� �����ϱ� ���� ����
    public CharacterMover lobbyPlayerCharacter;

    private void Start()
    {
        //NetworkRoomPlayer�� Start() �Լ��� �����ϱ� ������ �̾� ����
        base.Start();

        if (isServer)
        {
            //RoomPlayer�� Server��� -> LobbyPlayer ����
            SpawnLobbyPlayer();
            LobbyUIManager.Instance.ActiveStartButton();
        }

        if (isLocalPlayer)
        {
            CmdSetNickname(PlayerSettings.nickname);
        }

        LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
    }

    private void OnDestroy()
    {
        if(LobbyUIManager.Instance != null)
        {
            LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(playerColor);
            LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
        }
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        lobbyPlayerCharacter.nickname = nick;
    }

    [Command]//Cmd �Լ� �ۼ� ��, �̸� �տ� Cmd�� ���δ�.
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        lobbyPlayerCharacter.playerColor = color;
    }

    /// <summary>
    /// LobbyPlayer�� �����ϴ� �Լ�
    /// </summary>
    private void SpawnLobbyPlayer()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        EPlayerColor color = EPlayerColor.Red;

        for(int i = 0; i < (int)EPlayerColor.Count; ++i)
        {
            bool isFindSameColor = false;
            
            foreach(var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                //������ �÷��̾ �ƴѵ�, �÷��̾� Į�� ���� ��� -> ���� �� ó��(�ߺ� ����)
                if(amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }

            //�ߺ��� ���� �ƴ� ���
            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }

        //-> �÷��̾� ������ ����
        playerColor = color;

        var spawnPositions = FindObjectOfType<SpawnPositions>();
        int index = spawnPositions.Index;

        //������ ������ ������� LobbyPlayer ����
        Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        var player = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        player.transform.localScale = (index < 5) ? new Vector3(0.5f, 0.5f, 1f) : new Vector3(-0.5f, 0.5f, 1f);

        NetworkServer.Spawn(player.gameObject, connectionToClient);

        //������ LobbyPlayer�� ���� ����
        player.ownerNetId = netId;

        player.playerColor = color;
    }
}
