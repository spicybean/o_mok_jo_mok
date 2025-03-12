using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoneCell : MonoBehaviour
{
    [SerializeField] private Image _stoneImage;
    [SerializeField] private Sprite _stoneSprite;
    [SerializeField] private Color _stoneColor;
    [SerializeField] private TMP_Text _stoneIndexText;
    public int index;
    
    private delegate void OnStoneCellClicked(int index);
    private OnStoneCellClicked _onStoneCellClicked;
    // Start is called before the first frame update
    void Start()
    {
        index = 10;
    }

    public void OnClickedStoneCell()
    {
        //_onStoneCellClicked?.Invoke(index);
        _stoneIndexText.text = index.ToString();
        index--;

    }
    
}
