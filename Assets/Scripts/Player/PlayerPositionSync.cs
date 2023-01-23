using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
public class PlayerPositionSync : Synchronizable
{
    private Transform transform;
    private Vector3 playerPosition;
    private Vector3 oldPlayerPosition;

    private Vector3 playerRotation;
    private Vector3 oldPlayerRotation;

    public PlayerPositionSync()
    {
    }

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(playerPosition);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        playerPosition = reader.ReadVector3();
        transform.position = playerPosition;
        oldPlayerPosition = playerPosition;

        playerRotation = reader.ReadVector3();
        transform.eulerAngles = playerRotation;
        oldPlayerRotation = playerRotation;
    }

    public override void Serialize(ITransportStreamWriter processor, byte LOD)
    {
        base.Serialize(processor, LOD);
    }

    public override void Unserialize(ITransportStreamReader processor, byte LOD, uint length)
    {
        base.Unserialize(processor, LOD, length);
    }

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        playerPosition = transform.position;
        playerRotation = transform.eulerAngles;
        print(playerRotation);

        if(playerPosition != oldPlayerPosition)
        {
            oldPlayerPosition = playerPosition;
            Commit();
        }

        if(playerRotation != oldPlayerRotation)
        {
            oldPlayerRotation = playerRotation;
            Commit();
        }


        base.SyncUpdate();
    }
}
