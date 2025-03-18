using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public  int omockTier = 18;
    public int currentPoint = 0;

    

    public int GetRequiredPoints()
    {
        if (omockTier >= 10) return 3;
        else if (omockTier >= 5) return 5;
        else return 7;
        
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
           
        }
        
    }

    public void RankUp()
    {
        // 티어가 1일때 
        if (omockTier == 1)
        {
            //Debug.Log("You are already the highest rank.");
            return;
        }

        omockTier--;
        currentPoint = 0;
    
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
            
        }
    }

    public void RankDown()
    {
        if (omockTier == 18)
        {
           // Debug.Log("You are already the lowest rank.");
            return;
        }

        omockTier++;
        currentPoint = GetRequiredPoints() - 1;
     
    }
}
