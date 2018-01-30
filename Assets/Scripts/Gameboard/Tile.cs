using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex, yIndex;
//    Board m_Board;

    void Awake()
    {
    }

    void Update()
    {
    }

    public void Init(int x, int y/*, Board board*/)
    {
        xIndex = x;
        yIndex = y;
//        m_Board = board;
    }

    
}