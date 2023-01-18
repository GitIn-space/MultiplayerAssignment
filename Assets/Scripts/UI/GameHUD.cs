using Alteruna;
using Alteruna.Trinity;
using System.Collections.Generic;
using UnityEngine;
public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _playerContainer;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private Multiplayer _mp;

    private Dictionary<int, PlayerUI> playerUIDict = new Dictionary<int, PlayerUI>();
    private List<System.Guid> guids = new List<System.Guid>();


    #region placeholderfunctions
    [ContextMenu("Update Player One Score")]
    public void UpdatePlayerOneScore()
    {
        User user = _mp.GetUser(0);
        print($"Updating score for Player 0, user: {user}");
        playerUIDict[user].AddScore(5000, user);
    }

    [ContextMenu("Update Player Two Score")]
    public void UpdatePlayerTwoScore()
    {
        User user = _mp.GetUser(1);
        print($"Updating score for Player 1, user: {user}");
        playerUIDict[user].AddScore(5000, user);
    }
    [ContextMenu("Update Player Three Score")]
    public void UpdatePlayerThreeScore()
    {
        User user = _mp.GetUser(2);
        print($"Updating score for Player 2, user: {user}");
        playerUIDict[user].AddScore(5000, user);
    }
    #endregion

    void AddPlayerToList(int userindex, string username, System.Guid guid, int score = 0)
    {
        if (playerUIDict.ContainsKey(userindex)) { return; }
        PlayerUI ui = Instantiate(_playerUIPrefab, _playerContainer.transform).GetComponent<PlayerUI>();
        print($"asdf {userindex} {username}");
        ui.Initialize(username, guid, score);
        playerUIDict[userindex] = ui;
    }
    System.Guid GenerateGuid()
    {
        var guid = System.Guid.NewGuid();
        guids.Add(guid);
        return guid;
    }

    void OnOtherUserJoined(Multiplayer mp, User user)
    {
        if (_mp.Me.Index != 0)
        {
            return;
        }
        print($"HOST: OtherUserJoined, user: {user} joined.");
        var guid = GenerateGuid();
        AddPlayerToList(user.Index, user.Name, guid);
        CallJoinProcedure();
    }

    void OnRoomJoined(Multiplayer mp, Room room, User user)
    {
        if (_mp.Me.Index != 0)
        {
            return;
        }
        print($"HOST: RoomJoined, user: {user} joined.");
        var guid = GenerateGuid();
        AddPlayerToList(user.Index, user.Name, guid);
    }


    void CallJoinProcedure()
    {
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("userDataCount", guids.Count);
        for (int i = 0; i < guids.Count; i++)
        {
            parameters.Set($"userData{i}", $"{playerUIDict[i].Name},{guids[i]},{playerUIDict[i].Score}");
        }
        _mp.InvokeRemoteProcedure("MyProcedureFunction", UserId.All, parameters);
    }

    void JoinProcedureFunction(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        parameters.Get("userDataCount", out int dataCount);
        for (int i = 0; i < dataCount; i++)
        {
            parameters.Get($"userData{i}", out string data);
            string[] args = data.Split(',');
            AddPlayerToList(i, args[0], System.Guid.Parse(args[1]), int.Parse(args[2]));
        }
    }
    private void Awake()
    {
        _mp.OtherUserJoined.AddListener(OnOtherUserJoined);
        _mp.RoomJoined.AddListener(OnRoomJoined);
    }

    void Start()
    {
        _mp.RegisterRemoteProcedure("MyProcedureFunction", JoinProcedureFunction);
    }
}
