using Alteruna;
using Alteruna.Trinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private GameObject _playerContainer;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private Multiplayer _mp;

    private Dictionary<ushort, UserUIData> _playerUIDict = new Dictionary<ushort, UserUIData>();

    struct UserUIData
    {
        public UserUIData(PlayerUI ui, System.Guid p_guid) { playerUI = ui; guid = p_guid; }
        public PlayerUI playerUI;
        public System.Guid guid;
    }

    private ushort _hostIndex = ushort.MaxValue;
    private Coroutine _hostCoroutine = null;
    private bool IsHost => _hostIndex == _mp.Me.Index;

    void ResetChanges()
    {
        foreach (Transform player in _playerContainer.transform)
        {
            Destroy(player.gameObject);
        }
        _playerUIDict.Clear();
        _hostIndex = ushort.MaxValue;
    }

    void AddPlayerToList(ushort userindex, string username, System.Guid guid, int score = 0)
    {
        if (_playerUIDict.ContainsKey(userindex)) { return; }
        PlayerUI ui = Instantiate(_playerUIPrefab, _playerContainer.transform).GetComponent<PlayerUI>();
        ui.Initialize(username, guid, score);
        _playerUIDict[userindex] = new UserUIData(ui, guid);
    }
    System.Guid GenerateGuid()
    {
        var guid = System.Guid.NewGuid();
        return guid;
    }

    IEnumerator TryAssumeHost(User user)
    {
        yield return new WaitForSeconds(0.5f);
        if (_hostIndex == ushort.MaxValue)
        {
            _hostIndex = user.Index;
            var guid = GenerateGuid();
            AddPlayerToList(user.Index, user.Name, guid);
        }
    }
    void OnOtherUserJoined(Multiplayer mp, User user)
    {
        if (!IsHost)
        {
            StopCoroutine(_hostCoroutine);
            return;
        }
        var guid = GenerateGuid();
        AddPlayerToList(user.Index, user.Name, guid);
        CallJoinProcedure();
    }

    void OnRoomJoined(Multiplayer mp, Room room, User user)
    {
        _hostCoroutine = StartCoroutine(TryAssumeHost(user));
        _uiPanel.SetActive(true);
    }
    void OnRoomLeft(Multiplayer mp)
    {
        ResetChanges();
        _uiPanel.SetActive(false);
    }
    void OnOtherUserLeft(Multiplayer mp, User user)
    {
        if (!_playerUIDict.ContainsKey(user.Index))
            return;

        Destroy(_playerUIDict[user.Index].playerUI.gameObject);
        _playerUIDict.Remove(user.Index);
        if (user.Index != _hostIndex)
            return;

        ushort minIndex = ushort.MaxValue;
        foreach (var player in _mp.CurrentRoom.Users)
        {
            if (player.Index < minIndex && player.Index != _hostIndex)
                minIndex = player.Index;
        }

        _hostIndex = minIndex;
    }
    void CallJoinProcedure()
    {
        ProcedureParameters parameters = new ProcedureParameters();
        string userKeys = string.Join(",", _playerUIDict.Keys);
        parameters.Set("userDataKeys", userKeys);
        foreach (var pair in _playerUIDict)
        {
            var uiObj = pair.Value.playerUI;
            var guid = pair.Value.guid;
            parameters.Set($"userData{pair.Key}", $"{uiObj.Name},{guid},{uiObj.Score}");
        }
        _mp.InvokeRemoteProcedure("MyProcedureFunction", UserId.All, parameters);
    }

    void JoinProcedureFunction(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        _hostIndex = fromUser;
        parameters.Get("userDataKeys", out string userKeys);
        string[] keys = userKeys.Split(',');
        for (ushort i = 0; i < keys.Length; i++)
        {
            parameters.Get($"userData{keys[i]}", out string data);
            string[] args = data.Split(',');
            var guid = System.Guid.Parse(args[1]);
            AddPlayerToList(ushort.Parse(keys[i]), args[0], guid, int.Parse(args[2]));
        }
    }

    private void Awake()
    {
        _mp.OtherUserJoined.AddListener(OnOtherUserJoined);
        _mp.RoomJoined.AddListener(OnRoomJoined);
        _mp.RoomLeft.AddListener(OnRoomLeft);
        _mp.OtherUserLeft.AddListener(OnOtherUserLeft);
        EventHandler.UserHit += OnUserHit;
    }

    private void OnUserHit(User sender, User hitUser)
    {
        if (!_playerUIDict.ContainsKey(sender))
            return;

        _playerUIDict[sender].playerUI.AddScore(100);
    }

    void Start()
    {
        _mp.RegisterRemoteProcedure("MyProcedureFunction", JoinProcedureFunction);
    }
}
