using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using Avatar = Alteruna.Avatar;

[RequireComponent(typeof(Avatar), typeof(TeamComponent))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] string shootButton = "Fire1";
    [SerializeField] float shootRange = 10f;
    [SerializeField] float shootDistanceFromPlayer = 1f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] LineRenderer shootLinePrefab;

    private Avatar avatar;
    private TeamComponent tc;
    private Multiplayer mp;

    private float lastShotTime = float.NegativeInfinity;

    private const string ShootProcedureName = "Shoot";
    private const string FromX = "fromX";
    private const string FromY = "fromY";
    private const string ToX = "toX";
    private const string ToY = "toY";

    private const string HitProcedureName = "Hit";

    void Start()
    {
        avatar = GetComponent<Avatar>();
        tc = GetComponent<TeamComponent>();
        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure(ShootProcedureName, ShootMethod);
        mp.RegisterRemoteProcedure(HitProcedureName, HitMethod);
    }

    void Update()
    {
        if (avatar.IsMe)
        {
            float time = Time.time;
            if (Input.GetButtonDown(shootButton) && time >= lastShotTime + timeBetweenShots)
            {
                lastShotTime = time;

                Vector2 position = transform.position;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePos - position).normalized;
                Vector2 from = position + direction * shootDistanceFromPlayer;

               // RaycastHit2D hit = Physics2D.Raycast(from, direction, shootRange);
                RaycastHit2D[] hits = Physics2D.RaycastAll(from, direction, shootRange);

                Vector2 to = from + direction * shootRange;

                foreach (RaycastHit2D hit in hits) 
                {
                    TeamComponent otherTeam = hit.transform.GetComponent<TeamComponent>();
                    if (otherTeam && otherTeam.Team != tc.Team)
                    {
                        Avatar other = hit.transform.GetComponent<Avatar>();
                        if (other)
                        {
                            to = hit.point;
                            CallHitProcedure(other.Possessor.Index);
                            break;
                        }
                    }
                }

                CallShootProcedure(from, to);
            }
        }
    }

    void CallShootProcedure(Vector2 from, Vector2 to)
    {
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set(FromX, from.x);
        parameters.Set(FromY, from.y);
        parameters.Set(ToX, to.x);
        parameters.Set(ToY, to.y);
        mp.InvokeRemoteProcedure(ShootProcedureName, UserId.AllInclusive, parameters);
    }

    void ShootMethod(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        float fromX = parameters.Get(FromX, 0f);
        float fromY = parameters.Get(FromY, 0f);
        float toX = parameters.Get(ToX, 0f);
        float toY = parameters.Get(ToY, 0f);
        Vector2 from = new Vector2(fromX, fromY);
        Vector2 to = new Vector2(toX, toY);

        LineRenderer line = Instantiate(shootLinePrefab);
        line.SetPosition(0, from);
        line.SetPosition(1, to);
    }

    void CallHitProcedure(ushort UserID)
    {
        ProcedureParameters parameters = new ProcedureParameters();
        mp.InvokeRemoteProcedure(HitProcedureName, UserID, parameters);
    }

    void HitMethod(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        Debug.Log("I've been hit, take damage somehow!");
    }
}