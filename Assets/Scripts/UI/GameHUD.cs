using Alteruna;
using UnityEngine;
public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private Multiplayer _mp;


    void AddPlayerToList(Multiplayer mp, User user)
    {
        PlayerUI ui = Instantiate(_playerUIPrefab, _playerList.transform).GetComponent<PlayerUI>();
        ui.SetName(user.Name);
    }

    [ContextMenu("Update Player Score")]
    void UpdatePlayerScore()
    {
        _playerList.transform.GetChild(0).GetComponent<PlayerUI>().SetScore(5000);
    }

    private void Awake()
    {
        _mp.OtherUserJoined.AddListener(AddPlayerToList);
    }

}
