using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RanjuRule : MonoBehaviour
{
    public GameObject[] objects;
    public OmokCell.MarkerType[,] markers;

    public Sprite XMarker;
    public int[] Line = new int[9];

    public bool RanJu(int index, OmokCell.MarkerType marker)
    {
        if (CheckDoubleThree(index))
        {
            return true;
        }

        if (CheckDoubleFour(index))
        {
            return true;
        }

        return false;
    }
    
    bool CheckOutOfIndex(int row, int col)
    {
        if (row < 0 || row > 14 || col < 0 || col > 14)
        {
            return true;
        }

        return false;
    }

   
    bool CheckDoubleThree(int index)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        int CountOpenThree = 0;
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckThree(index, directions[i]))
            {
                CountOpenThree++;
            }
            
        }

        if (CountOpenThree >= 2)
        {
            return true;
        }
        return false;
    }
    bool CheckThree(int index, (int, int) direction)
    {
        string[] patternedThree = new string[] { "01110","011010","010110" };
        string pattern = "";
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                        '0' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "2";
                }
                
            }
            for (int k = 0; k < patternedThree.Length; k++)
            {
                if (pattern.Contains(patternedThree[k]))
                {
                    return true;
                }
            }

            pattern = "";
        }
        return false;
    }
    bool CheckDoubleFour(int index)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (1, -1)};
        int CountFour = 0;
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckFour(index, directions[i]) == 1)
            {
                CountFour++;
            }
            else if (CheckFour(index, directions[i]) > 1)
            {
                return true;
            }
        }
        if (CountFour >= 2)
        {
            return true;
        }
        return false;
    }
    int CheckFour(int index, (int,int) direction)
    {
        string[] patternedFour = new string[] { "01111","11110","11011","10111","11101" };
        string pattern = "";
        int checkFour = 0;
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+5; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                            '0' : 
                            markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "2";
                }
            }
            for (int k = 0; k < patternedFour.Length; k++)
            {
                if (pattern.Equals(patternedFour[k]))
                {
                    checkFour++;
                    Debug.Log($"{direction}, {index}: {pattern}");
                }
            }
            
            pattern = "";
        }
        Debug.Log(checkFour);
        return checkFour;
    }
    bool CheckFiveInAllDirections(int index, OmokCell.MarkerType marker)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckFive(index, directions[i],marker))
            {
                return true;
            }
        }
        return false;
    }
    bool CheckFive(int index, (int, int) direction, OmokCell.MarkerType marker)
    {
        string[] patternedFive = new string[] { "0111110","0111112","2111110","2111112" };
        string pattern = "";
        for (int i = -5; i < 1; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                            '0' : 
                            markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == marker? "1" : "2";
                }
                
            }
            //Debug.Log($"{direction}, {index}: {pattern}");
            for (int k = 0; k < patternedFive.Length; k++)
            {
                if (pattern.Equals(patternedFive[k]))
                {
                    return true;
                }
            }
            
            pattern = "";
        }

        return false;
    }

    bool CheckMoreThanFive(int index, (int, int) direction)
    {
        string patternedSix = "111111";
        string pattern = "";
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+5; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                            '0' : 
                            markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "2";
                }
            }
            if (pattern.Equals(patternedSix))
            {
                return true;
            }
            
            pattern = "";
        }
        return false;
    }
}
