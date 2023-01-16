using Alteruna;
using TMPro;
using UnityEngine;

public class PlayerUI : Synchronizable
{
    [SerializeField] private TMP_Text _nameTextComponent;
    [SerializeField] private TMP_Text _scoreTextComponent;

    private string _name = "";
    private string _score = "";
    private string _oldName = "";
    private string _oldScore = "";

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(_name);
        writer.Write(_score);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        _name = reader.ReadString();
        _score = reader.ReadString();
        _oldName = _name;
        _oldScore = _score;
    }

    public void SetName(string name)
    {
        _name = name;
        _oldName = _name;
        Commit();
        _nameTextComponent.text = name;
    }

    public void SetScore(int score)
    {
        _score = score.ToString();
        _oldScore = _score;
        Commit();
    }

    private void Update()
    {
        base.SyncUpdate();
    }
}
