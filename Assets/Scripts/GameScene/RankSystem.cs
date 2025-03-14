using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public  int omockTier = 18;
    public int currentPoint = 0;

    private int GetRequiredPoints()
    {
        if (omockTier >= 10) return 3;
        else if (omockTier >= 5) return 5;
        else return 10;
        
    }

    public void AddPoints(int points)
    {
        if(omockTier == 1)
        {
            return;
        }
        else 
        {
            currentPoint += points;
            if (currentPoint >= GetRequiredPoints())
            {
                RankUp();
            }
        }
        
    }

    public void RankUp()
    {
        if (omockTier == 1)
        {
            Debug.Log("You are already the highest rank.");
            return;
        }

        omockTier--;
        currentPoint = 0;
        Debug.Log($"RankUp! {omockTier}");
        Debug.Log($"RequiredPoints: {GetRequiredPoints()}");
    }

    public void LosePoints(int points)
    {
        if(omockTier == 18 && currentPoint ==0)
        {
            return;
        }
        else
        {
            currentPoint -= points;
            if (currentPoint < 0)
            {
                RankDown();
            }
        }
    }

    public void RankDown()
    {
        if (omockTier == 18)
        {
            Debug.Log("You are already the lowest rank.");
            return;
        }

        omockTier++;
        currentPoint = GetRequiredPoints() - 1;
        Debug.Log($"RankDown! {omockTier}");
        Debug.Log($"RequiredPoints: {GetRequiredPoints()}");
    }
}
