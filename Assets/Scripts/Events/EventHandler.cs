using Alteruna;
using UnityEngine;
public class EventHandler : MonoBehaviour
{

    public delegate void UserHitHandler(User sender, User hitUser);
    public static event UserHitHandler UserHit;

    public static void RaiseUserHit(User sender, User hitUser)
    {
        if (UserHit != null)
        {
            UserHit(sender, hitUser);
        }
    }
    //[SerializeField] private float Speed = 10.0f;
    //[SerializeField] private float RotationSpeed = 180.0f;
    //[SerializeField] private Multiplayer mp;

    //[SerializeField] private int health = 10;

    //void GetHit(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    //{
    //    int outer = 0;
    //    parameters.Get("damage", outer);
    //    health -= outer;

    //    if (health <= 0)
    //        ;//do something
    //}

    //void CallGetHit(Alteruna.Avatar otherPlayer)
    //{
    //    ProcedureParameters parameters = new ProcedureParameters();
    //    parameters.Set("damage", 1);
    //    mp.InvokeRemoteProcedure("GetHit", otherPlayer.Possessor.Index, parameters);
    //}

    //void Start()
    //{
    //    mp = GetComponent<Multiplayer>();
    //    mp.RegisterRemoteProcedure("GetHit", GetHit);
    //}

    //void Update()
    //{

    //}
}