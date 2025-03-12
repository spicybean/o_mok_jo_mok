using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] omokButtons;
    public int[,] omokBoard;
    private Color myTurn = Color.black;
    private Color enenmyTurn = Color.blue;
    public GameObject omokPrefab;
    // Start is called before the first frame update
    void Start()
    {
        omokButtons = new GameObject[56];
        omokBoard = new int[7, 8];
        for (int i = 0; i < omokButtons.Length; i++)
        {
            omokButtons[i] = gameObject.GetComponentsInChildren<Button>()[i].gameObject;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedOmokButton(string buttonName)
    {
        
    }
}
