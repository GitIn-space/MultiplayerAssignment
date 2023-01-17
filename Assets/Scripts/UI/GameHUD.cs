using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using System.Collections.Generic;
public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private Multiplayer _mp;

    private Dictionary<User, PlayerUI> playerUIDict = new Dictionary<User, PlayerUI>();

    void AddPlayerToList(Multiplayer mp, User user)
    {
        PlayerUI ui = Instantiate(_playerUIPrefab, _playerList.transform).GetComponent<PlayerUI>();
        ui.SetName(user.Name);
        playerUIDict[user] = ui;
    }

    [ContextMenu("Update Player One Score")]
    void UpdatePlayerOneScore()
    {
        playerUIDict[_mp.GetUser(0)].AddScore(5000);
    }

    [ContextMenu("Update Player Two Score")]
    void UpdatePlayerTwoScore()
    {
        playerUIDict[_mp.GetUser(1)].AddScore(5000);
    }

    [ContextMenu("Call Procedure")]
    void CallMyProcedure()
    {
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("value", 16.0f);

        var users = _mp.GetUsers();
        foreach (var user in users)
        {
            _mp.InvokeRemoteProcedure("MyProcedureFunction", user.Index);
        }
    }


    void UpdatePlayerList()
    {
        foreach (Transform child in _playerList.transform)
            Destroy(child.gameObject);

        var users = _mp.GetUsers();
        foreach (var user in users)
            AddPlayerToList(_mp, user);
    }



    void OnRoomJoined(Multiplayer mp, Room room, User user)
    {
        //_mp.OtherUserJoined.RemoveListener(AddPlayerToList);
    }

    void MyProcedureFunction(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        //float myValue = parameters.Get("value", 0);
        //string myString = parameters.Get("string_value", "default value");
        UpdatePlayerList();
    }

    void Start()
    {
        _mp.RegisterRemoteProcedure("MyProcedureFunction", MyProcedureFunction);
    }

    private void Awake()
    {
        //_mp.OtherUserJoined.AddListener(AddPlayerToList);
        //_mp.RoomJoined.AddListener(OnRoomJoined);
    }
    void Update()
    {

    }


}
