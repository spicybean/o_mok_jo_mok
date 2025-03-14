using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OmokCell : MonoBehaviour
{
    public enum MarkerType{None,PlaceMark,Black,White}
    [SerializeField] private Sprite _blackStone;
    [SerializeField] private Sprite _whiteStone;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private MarkerType _markerType;
    [SerializeField] private Color _selectedColor;
    
    public int index;
    public int placedTurn;

    public int INDEX
    {
        get { return index; }
        set { }
    }

    public Sprite SpriteType(MarkerType markerType = MarkerType.None)
    {
         switch (markerType)
        {
            case MarkerType.PlaceMark:
                return _selectedSprite;
            case MarkerType.Black:
                return _blackStone;
            case MarkerType.White:
                return _whiteStone;
            
        }
         return null;
    }
    public MarkerType GetMarkerType{
        get
        {
            return _markerType;
        }
        
    }
    private delegate void OnStoneCellClicked(int index);
    private OnStoneCellClicked _onStoneCellClicked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void initCell(int index)
    {
        this.index = index;
        this.placedTurn = 0;
        _markerType = MarkerType.None;
        _selectedColor = new Color(1, 1, 1, 1);
    }
    public void SetTurn(int turn)
    {
        placedTurn = turn;
    }
    
    public void PlaceMark(int turn, MarkerType playerMarkerTypeType = MarkerType.None)
    {
        switch (playerMarkerTypeType)
        {
            case MarkerType.Black:
                this.GetComponent<Image>().sprite = _blackStone;
                GetComponent<Image>().color = _selectedColor;
                _markerType = MarkerType.Black;
                placedTurn = turn;
                //GetComponent<SpriteRenderer>().sprite = _blackStone;
                break;
            case MarkerType.White:
                GetComponent<Image>().sprite = _whiteStone;
                GetComponent<Image>().color = _selectedColor;
                _markerType = MarkerType.White;
                placedTurn = turn;
                break;
            case MarkerType.PlaceMark:
                this.GetComponent<Image>().sprite = _selectedSprite;
                GetComponent<Image>().color = _selectedColor;
                _markerType = MarkerType.PlaceMark;
                break;
            case MarkerType.None:
                this.GetComponent<Image>().sprite = null;
                GetComponent<Image>().color = new Color(0, 0, 0, 0f);
                _markerType = MarkerType.None;
                break;
                
        }
    }
    public void OnClickedStoneCell()
    {
        //_onStoneCellClicked?.Invoke(index);
        //_stoneIndexText.text = index.ToString();
        

    }
    
}
