[System.Serializable]
public class SaveData
{
    public int highscore;
    public int coins;
    public int skin;

    public bool[] skinsUnlocked;

    public SaveData(GameController controller)
    {
        highscore = controller.Highscore;
        coins = controller.Coins;
        skin = controller.Skin;
        skinsUnlocked = controller.SkinsUnlocked;
    }
}
