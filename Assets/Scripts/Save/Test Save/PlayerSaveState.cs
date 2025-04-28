using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    public int money;
    public PlayerSaveState(int money)
    {
        this.money = money;
    }

    public void LoadData()
    {
        PlayerStats.LoadStats(money);
    }
    public static PlayerSaveState Export()
    {
        return new PlayerSaveState(PlayerStats.Money);
    }

}
