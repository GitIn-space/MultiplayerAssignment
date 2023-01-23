using Alteruna;
using TMPro;
using UnityEngine;

public class PlayerUI : Synchronizable
{
    [SerializeField] private TMP_Text _nameTextComponent;
    [SerializeField] private TMP_Text _scoreTextComponent;

    private string _name = "";
    private string _oldName = "";
    private int _score = 0;
    private int _oldScore = 0;

    public string Name => _name;
    public int Score => _score;
    public void Initialize(string name, System.Guid guid, int score)
    {
        _name = name;
        _score = score;
        _oldScore = score;
        _oldName = name;
        _nameTextComponent.text = _name;
        _scoreTextComponent.text = _score.ToString();
        OverrideUID(guid, true);
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        writer.Write(_name);
        writer.Write(_score);
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        _name = reader.ReadString();
        _score = reader.ReadInt();
        _oldName = _name;
        _oldScore = _score;
        _nameTextComponent.text = _name;
        _scoreTextComponent.text = _score.ToString();
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreTextComponent.text = _score.ToString();
    }

    private void Update()
    {
        if (_oldName != _name)
        {
            _oldName = _name;
            Commit();
        }
        if (_oldScore != _score)
        {
            _oldScore = _score;
            Commit();
        }
        base.SyncUpdate();
    }
}
