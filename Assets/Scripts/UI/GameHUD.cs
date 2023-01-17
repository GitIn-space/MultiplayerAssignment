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

    //void UpdatePlayerList()
    //{
    //    foreach (Transform child in _playerList.transform)
    //        Destroy(child.gameObject);

    //    var users = _mp.GetUsers();
    //    foreach (var user in users)
    //        AddPlayerToList(user);
    //}

    //void OnRoomJoined(Multiplayer mp, Room room, User user)
    //{
    //    AddPlayerToList(user);
    //}

    private void Awake()
    {
        _mp.OtherUserJoined.AddListener(AddPlayerToList);
        //_mp.RoomJoined.AddListener(OnRoomJoined);
    }
    void Update()
    {

    }


}
