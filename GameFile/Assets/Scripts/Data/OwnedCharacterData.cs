[System.Serializable]
public class OwnedCharacterData
{
    public CharacterData characterData;
    public int count;

    // コンストラクタの修正
    public OwnedCharacterData(CharacterData characterData, int count = 0)
    {
        this.characterData = characterData;
        this.count = count;
    }
}
