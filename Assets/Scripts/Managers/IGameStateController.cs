using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateController
{
    int exp { get; }

    string playerName { get; }

    int lvl { get; }
    int money { get; }

    void AddExp(int exp);
    void ChangePlayerName(string name);
    void IncrementLevel(); //ensure to reset exp to 0 after
    void MoneyBalanceChange(int amount); //can be + or -
}