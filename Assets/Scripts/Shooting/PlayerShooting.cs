using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using Avatar = Alteruna.Avatar;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] string shootButton = "Fire1";
    [SerializeField] int shootDistance = 10;
    [SerializeField] float shootDistanceFromPlayer = 0.5f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] LineRenderer shootLinePrefab;

    private Multiplayer mp;
    private Avatar avatar;

    private float lastShotTime = float.NegativeInfinity;

    private const string FromX = "fromX";
    private const string FromY = "fromY";

    private const string ToX = "toX";
    private const string ToY = "toY";
    private const string ShootProcedureName = "Shoot";

    private const string HitProcedureName = "Hit";

    void Start()
    {
        // Get components
        avatar = GetComponent<Avatar>();

        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure(ShootProcedureName, ShootMethod);
        mp.RegisterRemoteProcedure(HitProcedureName, HitMethod);
    }

    void Update()
    {
        // Only let input affect the avatar if it belongs to me
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

                RaycastHit2D hit = Physics2D.Raycast(from, direction, shootDistance);
                Vector2 to;
                
                if (hit)
                {
                    to = hit.point;
                    Avatar other = hit.transform.GetComponent<Avatar>();
                    if (other)
                    {
                        CallHitProcedure(other.Possessor.Index);
                    }
                }
                else
                {
                    to = from + direction * shootDistance;
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