using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RanjuRule : MonoBehaviour
{
    public GameObject[] objects;
    public OmokCell.MarkerType[,] markers;

    public Sprite XMarker;
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
            if (objects[i].GetComponent<OmokCell>().GetMarkerType != OmokCell.MarkerType.None)
            {
               // Debug.Log($"{objects[i].GetComponent<OmokCell>().GetMarkerType} : {objects[i].name}");
            }
        }

        for (var i = 0; i < objects.Length; i++)
        {
            if (markers[i/15,i%15] == OmokCell.MarkerType.None)
            {
                if (CheckThree(i,(0,1)))
                {
                    Debug.Log(objects[i].name);
                }
                if (CheckThree(i,(1,0)))
                {
                    Debug.Log(objects[i].name);
                }
                if (CheckThree(i,(1,1)))
                {
                    Debug.Log(objects[i].name);
                }
                if (CheckThree(i,(1,-1)))
                {
                    Debug.Log(objects[i].name);
                }
            }
        }
        
    }

    bool CheckThree(int index,(int,int) direction)
    {
        //열린 4를 만들 수 있는가
        (int,int) center = (index / 15, index % 15);
        int checkEmptyLeft = 0;
        int checkEmptyRight = 0;
        int countLeft = 0;
        int countRight = 0;
        int[] omokArray = new int[7];
        omokArray[3] = 1;
        for (int i = 0; i < 5; i++)
        {
            //오른쪽 탐색
            if (!CheckOutOfIndex(center.Item1 + direction.Item1 * i, center.Item2 + direction.Item2 * i))
            {
                if (markers[center.Item1 + direction.Item1 * i, center.Item2 + direction.Item2 * i] ==
                    OmokCell.MarkerType.White)
                {
                    return false;
                }
            }
            if (!CheckOutOfIndex(center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i))
            {
                if (markers[center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i] ==
                    OmokCell.MarkerType.White)
                {
                    return false;
                }
            }
            else
            {
                break;
            }
            
        }
        for (int i = 1; i < 4; i++)
        {
            //왼쪽 탐색
            if (!CheckOutOfIndex(center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i))
            {
                if (markers[center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i] ==
                    OmokCell.MarkerType.None)
                {
                    omokArray[3-i] = 0;
                }

                if (markers[center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i] ==
                    OmokCell.MarkerType.Black)
                {
                    omokArray[3-i] = 1;
                }

                if (markers[center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i] ==
                    OmokCell.MarkerType.White)
                {
                    omokArray[3-i] = -1;
                }
            }
        }
        for (int i = 0; i < omokArray.Length; i++)
        {
            
        }
        if (countLeft + countRight + 1 == 3)
        {
            return true;
        }
        return false;
    }
    bool CheckOpenFour(int index, (int,int) direction)
    {
        //방위별로 빈칸이 2개인지 체크
        //방위별로 흰색돌이 있는지 체크
        //
        int checkEmptyLeft = 0;
        int checkEmptyRight = 0;
        (int,int) center = (index / 15, index % 15);
        
        for (int i = 1; i < 5; i++)
        {
            if (!CheckOutOfIndex(center.Item1 + direction.Item1 * i, center.Item2 + direction.Item2 * i)
                &&markers[center.Item1 + direction.Item1 * i, center.Item2 + direction.Item2 * i] ==
                OmokCell.MarkerType.None && checkEmptyRight != 2)
            {
                
            }
            
        }

        for (int i = 0; i < 5; i++)
        {
            if (!CheckOutOfIndex(center.Item1 - direction.Item1 * i, center.Item2 - direction.Item2 * i) &&
                markers[center.Item1 + direction.Item1 * i, center.Item2 + direction.Item2 * i] ==
                OmokCell.MarkerType.None && (checkEmptyLeft !=2))
            {
                
            }
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
