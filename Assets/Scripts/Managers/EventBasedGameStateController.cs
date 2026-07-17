using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBasedGameStateController : IGameStateController
{
    public int exp
    {
        get { throw new NotImplementedException(); }
    }

    public int lvl
    {
        get { throw new NotImplementedException(); }
    }

    public int money
    {
        get { throw new NotImplementedException(); }
    }

    public string playerName
    {
        get { throw new NotImplementedException(); }
    }

    public void AddExp(int exp)
    {
        throw new NotImplementedException();
    }

    public void ChangePlayerName(string name)
    {
        throw new NotImplementedException();
    }

    public void IncrementLevel()
    {
    }

    public void MoneyBalanceChange(int amount)
    {
        throw new NotImplementedException();
    }
}