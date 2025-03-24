using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RanjuRule : MonoBehaviour
{
    public GameObject[] objects;
    public OmokCell.MarkerType[,] markers;

    
    
    public void StartRule()
    {
        //렌주룰용 보드 초기화
        var totalcells = 15 * 15;
        
        objects = new GameObject[totalcells];
        
        markers = new OmokCell.MarkerType[15,15];
        UpdateBoardState();
    }
    public void UpdateBoardState()
    {
        //보드 업데이트
        for (var i = 0; i < this.objects.Length; i++)
        {
            var obj = transform.GetChild(i).gameObject;
            objects[i] = obj;
            markers[i/15,i%15] = objects[i].GetComponent<OmokCell>().My_MarkerType;
            
        }
        
    }
    public bool RanJu(int index)
    {
        
        if (CheckDoubleThree(index))
        {
            Debug.Log("RanjuThree");
            return true;
        }

        if (CheckDoubleFour(index))
        {
            Debug.Log("RanjuFour");
            return true;
        }

        if (CheckJangMok(index))
        {
            Debug.Log("RanjuJang");
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
        int checkOpenFour = 0;
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+5; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                        '0'  :
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "2";
                }
            }
            for (int k = 0; k < patternedFour.Length; k++)
            {
                if (pattern.Equals("01111") || pattern.Equals("11110"))
                {
                    checkOpenFour++;
                }
                if (pattern.Contains(patternedFour[k]))
                {
                    checkFour++;
                }
                if (pattern.Equals("11111"))
                {
                    return 0;
                }
            }

            
            pattern = "";
        }

        if (checkOpenFour >= 2)
        {
            return 1;
        }
        return checkFour;
    }
    public bool CheckFiveInAllDirections(int index)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckFive(index, directions[i]))
            {
                return true;
            }
        }
        return false;
    }
    bool CheckFive(int index, (int, int) direction )
    {
        string[] patternedFive = new string[] { "0111110","0111112","2111110","2111112" };
        string pattern = "";
        for (int i = -5; i < 1; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                            '0' : 
                            markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.PlaceMark ? 
                                '0' :
                                markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "2";
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

    bool CheckJangMok(int index)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckMoreThanFive(index, directions[i]))
            {
                return true;
            }
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
                            markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.PlaceMark ? 
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
