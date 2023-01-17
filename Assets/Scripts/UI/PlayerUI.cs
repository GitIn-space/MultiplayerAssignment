using Alteruna;
using TMPro;
using UnityEngine;

public class PlayerUI : Synchronizable
{
    [SerializeField] private TMP_Text _nameTextComponent;
    [SerializeField] private TMP_Text _scoreTextComponent;

    private string _name = "";
    private int _score = 0;
    private string _oldName = "";
    private int _oldScore = 0;

    public override void AssembleData(Writer writer, byte LOD)
    {
        print("assembling data");
        writer.Write(_name);
        writer.Write(_score);
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        print("assembling data");
        _name = reader.ReadString();
        _score = reader.ReadInt();
        _nameTextComponent.text = _name;
        _scoreTextComponent.text = _score.ToString();
        _oldName = _name;
        _oldScore = _score;
    }

    public void SetName(string name)
    {
        _name = name;
        //Commit();
        //_nameTextComponent.text = _name;
    }

    public void AddScore(int score)
    {
        _score += score;
        //Commit();

        // _scoreTextComponent.text = _score;
    }

    private void Update()
    {
        if (_oldName != _name || _oldScore != _score)
        {
            _oldName = _name;
            _oldScore = _score;
            _scoreTextComponent.text = _score.ToString();
            _nameTextComponent.text = _name;
            print(_name);
            Commit();
        }
        base.SyncUpdate();
    }
}
