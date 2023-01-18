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
    [SerializeField] TraceLine shootLinePrefab;

    [Space(10)]
    [SerializeField] Health health;

    private Avatar avatar;
    private TeamComponent tc;
    private Multiplayer mp;

    private float lastShotTime = float.NegativeInfinity;

    private const string ShootProcedureName = "Shoot";
    private const string FromX = "fromX";
    private const string FromY = "fromY";
    private const string ToX = "toX";
    private const string ToY = "toY";

    void Start()
    {
        avatar = GetComponent<Avatar>();
        tc = GetComponent<TeamComponent>();
        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure(ShootProcedureName, ShootMethod);
    }

    void Update()
    {
        if (!avatar.IsMe)
            return;

        float time = Time.time;
        if (Input.GetButtonDown(shootButton) && time >= lastShotTime + timeBetweenShots)
        {
            lastShotTime = time;

            Vector2 position = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - position).normalized;
            Vector2 from = position + direction * shootDistanceFromPlayer;

            RaycastHit2D[] hits = Physics2D.RaycastAll(from, direction, shootRange);

            Vector2 to = from + direction * shootRange;

            foreach (RaycastHit2D hit in hits)
            {
                TeamComponent otherTeam = hit.transform.GetComponent<TeamComponent>();
                if (otherTeam)
                {
                    if (otherTeam.Team != tc.Team)
                    {
                        PlayerShooting other = hit.transform.GetComponent<PlayerShooting>();
                        if (other)
                        {
                            to = hit.point;
                            other.health.TakeDamage(1);
                            break;
                        }
                    }
                }
                else // If we hit something that wasnt a player like a wall
                {
                    to = hit.point;
                    break;
                }
            }

            CallShootProcedure(from, to);
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

        TraceLine line = Instantiate(shootLinePrefab);
        line.SetPositions(from, to);
    }
}