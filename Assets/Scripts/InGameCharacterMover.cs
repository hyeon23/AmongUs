using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerType 
{ 
    Crew,
    Imposter,
    CrewGhost,
    ImpoaterGhost,
    Count
}


public class InGameCharacterMover : CharacterMover
{
    [SyncVar]
    public EPlayerType playerType;

    public override void Start()
    {
        base.Start();

        if (isOwned)
        {
            IsMovable = true;

            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;

            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        GameSystem.instance.AddPlayer(this);
    }

    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor playerColor)
    {
        this.nickname = nickname;
        this.playerColor = playerColor;
    }
}
