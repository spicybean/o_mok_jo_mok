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
    
    private string[] patternedFour = new string[] { "01110,011010,010110" };
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

        // for (int i = 0; i < objects.Length; i++)
        // {
        //     if (objects[i].GetComponent<OmokCell>().GetMarkerType == OmokCell.MarkerType.None)
        //     {
        //         if (CheckDoubleThree(i))
        //         {
        //             objects[i].GetComponent<Image>().sprite = XMarker;
        //             objects[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //         }
        //     }
        // }
        
        Debug.Log(CheckThree(42,(0,1)));
    }

    bool CheckThree(int index, (int, int) direction)
    {
        string[] patternedThree = new string[] { "01110","011010","010110" };
        string pattern = "";
        for (int i = -4; i < 5; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.None ?
                        '0' : 
                        markers[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == OmokCell.MarkerType.Black? "1" : "-1";
                }
                
            }
            Debug.Log(pattern);
            for (int k = 0; k < patternedThree.Length; k++)
            {
                if (pattern.Contains(patternedThree[k]))
                {
                    return true;
                }
                Debug.Log(patternedThree[k].Equals(pattern));
            }

            pattern = "";
        }
        return false;
    }
    bool CheckOpenFour(int index, (int,int) direction)
    {
        string[] patternedFour = new string[] { "01110,011010,010110" };
       
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
    
      
    bool CheckDoubleFour(int index)
    {
        //directions[0]: 가로  directions[1]: 세로 directions[2]: 대각선 directions[3]: 반대 대각선
        (int, int)[] dir = { (0, 1), (1, 0), (1, 1), (1, -1) };
        (int,int) center = (index/15,index%15);
        int[] addedLines = new int[4]{0,0,0,0};
        int fourTrue = 0;
        
        for (int i = 0; i < 5; i++)
        {
            
            //가로 카운트
            if (CheckOutOfIndex(center.Item1 + dir[0].Item1 * i,center.Item2 + dir[0].Item2 * i) &&
                (markers[center.Item1 + (dir[0].Item1 * i), center.Item2 + (dir[0].Item2 * i)] ==
                OmokCell.MarkerType.Black))
            {
                addedLines[0]++;
            }
            if (CheckOutOfIndex(center.Item1 - dir[0].Item1 * i,center.Item2 - dir[0].Item2 * i) &&
                markers[center.Item1 - (dir[0].Item1 * i), center.Item2 - (dir[0].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[0]++;
            }
            //세로 카운트
            if (CheckOutOfIndex(center.Item1 + dir[1].Item1 * i,center.Item2 + dir[1].Item2 * i)&&
                markers[center.Item1 + (dir[1].Item1 * i), center.Item2 + (dir[1].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[1]++;
            }
            if (CheckOutOfIndex(center.Item1 - dir[1].Item1 * i,center.Item2 - dir[1].Item2 * i)&&
                markers[center.Item1 - (dir[1].Item1 * i), center.Item2 - (dir[1].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[1]++;
            }
            //대각선카운트
            if (CheckOutOfIndex(center.Item1 + dir[2].Item1 * i,center.Item2 + dir[2].Item2 * i)&&
                markers[center.Item1 + (dir[2].Item1 * i), center.Item2 + (dir[2].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[2]++;
            }
            if (CheckOutOfIndex(center.Item1 - dir[2].Item1 * i,center.Item2 - dir[2].Item2 * i)&&
                markers[center.Item1 - (dir[2].Item1 * i), center.Item2 - (dir[2].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[2]++;
            }
            //반대각선카운트
            if (CheckOutOfIndex(center.Item1 + dir[3].Item1 * i,center.Item2 + dir[3].Item2 * i)&&
                markers[center.Item1 + (dir[3].Item1 * i), center.Item2 + (dir[3].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[3]++;
            }
            if (CheckOutOfIndex(center.Item1 - dir[3].Item1 * i,center.Item2 - dir[3].Item2 * i)&&
                markers[center.Item1 - (dir[3].Item1 * i), center.Item2 - (dir[3].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[3]++;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            
            if (addedLines[i] >= 3)
            {
                fourTrue++;
            }
            
        }
        Debug.Log($"vertical: {addedLines[0]}, horizontal: {addedLines[1]}, " +
                  $"diagonal: {addedLines[2]} anti-diagonal: {addedLines[3]} four true: {fourTrue} objectsname{objects[index].name}");

        if (fourTrue >= 2)
        {
            return true;
        }
        return false;
    }
    bool CheckDoubleThree(int index)
    {
        //directions[0]: 가로  directions[1]: 세로 directions[2]: 대각선 directions[3]: 반대 대각선
        (int, int)[] dir = { (0, 1), (1, 0), (1, 1), (1, -1) };
        (int,int) center = (index/15,index%15);
        int[] addedLines = new int[4]{0,0,0,0};
        int fourTrue = 0;
        
        for (int i = 0; i < 4; i++)
        {
            
            //가로 카운트
            if (!CheckOutOfIndex(center.Item1 + dir[0].Item1 * i,center.Item2 + dir[0].Item2 * i) &&
                (markers[center.Item1 + (dir[0].Item1 * i), center.Item2 + (dir[0].Item2 * i)] ==
                OmokCell.MarkerType.Black))
            {
                addedLines[0]++;
            }
            if (!CheckOutOfIndex(center.Item1 - dir[0].Item1 * i,center.Item2 - dir[0].Item2 * i) &&
                markers[center.Item1 - (dir[0].Item1 * i), center.Item2 - (dir[0].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[0]++;
            }
            //세로 카운트
            if (!CheckOutOfIndex(center.Item1 + dir[1].Item1 * i,center.Item2 + dir[1].Item2 * i)&&
                markers[center.Item1 + (dir[1].Item1 * i), center.Item2 + (dir[1].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[1]++;
            }
            if (!CheckOutOfIndex(center.Item1 - dir[1].Item1 * i,center.Item2 - dir[1].Item2 * i)&&
                markers[center.Item1 - (dir[1].Item1 * i), center.Item2 - (dir[1].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[1]++;
            }
            //대각선카운트
            if (!CheckOutOfIndex(center.Item1 + dir[2].Item1 * i,center.Item2 + dir[2].Item2 * i)&&
                markers[center.Item1 + (dir[2].Item1 * i), center.Item2 + (dir[2].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[2]++;
            }
            if (!CheckOutOfIndex(center.Item1 - dir[2].Item1 * i,center.Item2 - dir[2].Item2 * i)&&
                markers[center.Item1 - (dir[2].Item1 * i), center.Item2 - (dir[2].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[2]++;
            }
            //반대각선카운트
            if (!CheckOutOfIndex(center.Item1 + dir[3].Item1 * i,center.Item2 + dir[3].Item2 * i)&&
                markers[center.Item1 + (dir[3].Item1 * i), center.Item2 + (dir[3].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[3]++;
            }
            if (!CheckOutOfIndex(center.Item1 - dir[3].Item1 * i,center.Item2 - dir[3].Item2 * i)&&
                markers[center.Item1 - (dir[3].Item1 * i), center.Item2 - (dir[3].Item2 * i)] ==
                OmokCell.MarkerType.Black)
            {
                addedLines[3]++;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            
            if (addedLines[i] >= 2)
            {
                fourTrue++;
            }
            
        }
        
        if (fourTrue >= 2)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
