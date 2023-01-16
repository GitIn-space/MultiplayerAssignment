using Alteruna;
using UnityEngine;
public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private Multiplayer _mp;

    [ContextMenu("Add Player")]
    void AddPlayerToList(Multiplayer mp, User user)
    {
        PlayerUI ui = Instantiate(_playerUIPrefab, _playerList.transform).GetComponent<PlayerUI>();
        ui.SetName(user.Name);
    }

    private void Awake()
    {
        _mp.OtherUserJoined.AddListener(AddPlayerToList);
    }

}
