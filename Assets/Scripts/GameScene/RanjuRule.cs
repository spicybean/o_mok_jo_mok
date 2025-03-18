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
    
    
    // Start is called before the first frame update
    void Start()
    {
        var totalcells = 15 * 15;
        
        objects = new GameObject[totalcells];
       
        markers = new OmokCell.MarkerType[15,15];
        for (var i = 0; i < totalcells; i++)
        {
            var obj = transform.GetChild(i).gameObject;
            objects[i] = obj;
            markers[i/15,i%15] = obj.GetComponent<OmokCell>().GetMarkerType;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<OmokCell>().GetMarkerType == OmokCell.MarkerType.None)
            {
                 if (CheckDoubleThree(i))
                 {
                     objects[i].GetComponent<Image>().sprite = XMarker;
                     objects[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }

                if (CheckDoubleFour(i))
                {
                    objects[i].GetComponent<Image>().sprite = XMarker;
                    objects[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }
        }

       // CheckFour(217,(1,0));
       // CheckFour(217,(1,1));
       // CheckFour(217,(0,1));
       // CheckFour(217,(1,-1));

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
  
    
      
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
