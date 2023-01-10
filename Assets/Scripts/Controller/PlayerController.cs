using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 10.0f;
    [SerializeField] private float RotationSpeed = 180.0f;
    [SerializeField] private Multiplayer mp;
    [SerializeField] private TextMeshProUGUI text;

    private Alteruna.Avatar _avatar;
    private SpriteRenderer _renderer;

    void MyProcedureFunction(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        ushort outer = 0;
        parameters.Get("winner", outer);
        text.text = fromUser + " has won";
    }

    void CallMyProcedure()
    {
        foreach (User each in mp.GetUsers())
        {
            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("winner", _avatar.Possessor.Index);
            mp.InvokeRemoteProcedure("MyProcedureName", each.Index, parameters);
        }
    }

    void Start()
    {
        // Get components
        _avatar = GetComponent<Alteruna.Avatar>();
        _renderer = GetComponent<SpriteRenderer>();

        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure("MyProcedureName", MyProcedureFunction);

        text = FindObjectOfType<TextMeshProUGUI>();
    }

    void Update()
    {
        // Only let input affect the avatar if it belongs to me
        if (_avatar.IsMe)
        {
            // Set the avatar representing me to be green
            _renderer.color = Color.green;

            // Get the horizontal and vertical axis.
            float _translation = Input.GetAxis("Vertical") * Speed;
            float _rotation = -Input.GetAxis("Horizontal") * RotationSpeed;

            _translation *= Time.deltaTime;
            _rotation *= Time.deltaTime;

            transform.Translate(0, _translation, 0, Space.Self);
            transform.Rotate(0, 0, _rotation);

            if (Input.GetKeyDown("space"))
                CallMyProcedure();
        }
    }
}