using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmokCell : MonoBehaviour
{
    // Èò»ö
    [SerializeField] private Sprite white;
    [SerializeField] private Sprite whiteAlpha;
    //°ËÀº»ö
    [SerializeField] private Sprite black;
    [SerializeField] private Sprite blackAlpha;

    public enum CellType
    {
        None,
        White,
        Black
    }

    public delegate void OnCellClicked(int index);
    public OnCellClicked oncellClicked;
    private int _cellIndex;

    
}
