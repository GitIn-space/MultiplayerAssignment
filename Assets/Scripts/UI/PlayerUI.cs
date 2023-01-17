using Alteruna;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTextComponent;
    [SerializeField] private TMP_Text _scoreTextComponent;

    private string _name = "";
    private int _score = 0;
    private string _oldName = "";
    private int _oldScore = 0;

    //public override void AssembleData(Writer writer, byte LOD)
    //{
    //    print($"assembling data, name == {_name}, score == {_score}");
    //    writer.Write(_name);
    //    writer.Write(_score);
    //}

    //public override void DisassembleData(Reader reader, byte LOD)
    //{
    //    print($"disassembling data, name == {_name}, score == {_score}");
    //    _name = reader.ReadString();
    //    _score = reader.ReadInt();
    //    _oldName = _name;
    //    _oldScore = _score;
    //    _nameTextComponent.text = _name;
    //    _scoreTextComponent.text = _score.ToString();
    //}

    public void SetName(string name)
    {
        _name = name;
        _nameTextComponent.text = _name;
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreTextComponent.text = _score.ToString();
    }

    //private void Update()
    //{
    //    if (_oldName != _name || _oldScore != _score)
    //    {
    //        _oldScore = _score;
    //        _oldName = _name;
    //        Commit();
    //    }
    //    base.SyncUpdate();
    //}
}
