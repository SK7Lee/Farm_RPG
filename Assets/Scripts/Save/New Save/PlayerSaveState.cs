using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    public int money;
    public int stamina;
    public PlayerSaveState(int money, int stamina)
    {
        this.money = money;
        this.stamina = stamina;
    }

    public void LoadData()
    {
        PlayerStats.LoadStats(money, stamina);
    }
    public static PlayerSaveState Export()
    {
        return new PlayerSaveState(PlayerStats.Money, PlayerStats.Stamina);
    }

}
