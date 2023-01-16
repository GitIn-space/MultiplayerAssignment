using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;
    [SerializeField] private GameObject _playerUIPrefab;


    [ContextMenu("Add Player")]
    void AddPlayerToList()
    {
        Instantiate(_playerUIPrefab, _playerList.transform);
    }


}
