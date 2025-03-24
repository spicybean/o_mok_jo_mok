using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    //PlayerPref 저장용
    //사용자 정보
    //제일 낮은 등급 18등급
    public int omockTier = 18;
    //현재 승점
    public int currentPoint = 0;
    
    

    public int GetRequiredPoints()
    {
        if (omockTier >= 10) return 3;
        else if (omockTier >= 5) return 5;
        else return 7;
        
    }

    public void AddPoints()
    {
        if(omockTier == 1)
        {
            return;
        }
        else 
        {
            currentPoint ++;
        }
        
    }

    public void RankUp()
    {
       
        if (omockTier == 1)
        {
            
            return;
        }

        omockTier--;
        currentPoint = 0;
    
    }

    public void LosePoints()
    {
        if(omockTier == 18 && currentPoint ==0)
        {
            return;
        }
        else
        {
            currentPoint --;
            
        }
    }

    public void RankDown()
    {
        if (omockTier == 18)
        {
          
            return;
        }

        omockTier++;
        currentPoint = GetRequiredPoints() - 1;
     
    }
}
