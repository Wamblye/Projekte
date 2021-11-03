using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : IComparable<Shop>
{
    public float reward;
    public long cost;
    public int level;
    public GameObject button;
    // Start is called before the first frame update
    public Shop(float newReward, long newCost, int newLevel, GameObject newButton)
    {
        reward = newReward;
        cost = newCost;
        level = newLevel;
        button = newButton;
    }

    public int CompareTo(Shop other)
    {
        if (other == null)
        {
            return 1;
        }
        return level - other.level;
    }
}
