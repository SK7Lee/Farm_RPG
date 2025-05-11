using UnityEngine;

public class PlayerStats 
{
    public static int Money { get; private set; }
    public static int Stamina;

    public const string CURRENCY = "G";

    public static void Spend(int cost)
    {
        if (cost > Money)
        {
            Debug.LogError("Not enough money");
            return;
        }
        Money -= cost;
        UIManager.Instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        Money += income;
        UIManager.Instance.RenderPlayerStats();

    }

    public static void LoadStats(int money, int stamina) 
    {
        Money = money;
        Stamina = stamina;
        UIManager.Instance.RenderPlayerStats();
    }

    public static void UseStamina(int staminaLost)
    {
        Stamina -= staminaLost;
        UIManager.Instance.RenderPlayerStats();
    }

    public static void IncreaseStamina(int amount)
    {
        //for now set the max stamina to 150
        Stamina += amount;
        if (Stamina > 150) Stamina = 150;
        UIManager.Instance.RenderPlayerStats();

    }

    public static void RestoreStamina()
    {
        Stamina = 150;
        UIManager.Instance.RenderPlayerStats();
    }


}
