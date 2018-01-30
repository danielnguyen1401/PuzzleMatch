using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] float borderSize = 1f;

    private Tile[,] m_allTiles;

//    private GamePiece[,] m_gamePiece;
    private GameObject dotParent;

    [SerializeField] GameObject[] gamePiecePrefabs;
    [SerializeField] private float swapTime = 1f;

    private GamePiece m_clickedDot;
    private GamePiece m_targetDot;

    void Start()
    {
        m_allTiles = new Tile[width, height];
//        m_gamePiece = new GamePiece[width, height];
        dotParent = GameObject.FindGameObjectWithTag(NameTags.DOT_PARENT);
//        SetupTiles();
        SetupCamera();
        FillRandom();
    }

    void Update()
    {
    }

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.name = "Tile " + i + "," + j;
                m_allTiles[i, j] = tile.GetComponent<Tile>();
                tile.transform.parent = transform;
                m_allTiles[i, j].Init(i, j /*, this*/);
            }
        }
    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((width - 1) * 0.5f, (height - 1) * 0.5f, -10);
        float aspectRatio = (float) Screen.width / (float) Screen.height;
        float verticalSize = height / 2f + borderSize;
        float horizontalSize = (width / 2f + borderSize) / aspectRatio;
        Camera.main.orthographicSize = verticalSize > horizontalSize ? verticalSize : horizontalSize;
    }

    GameObject RandomGamePiece()
    {
        int index = Random.Range(0, gamePiecePrefabs.Length);
        return gamePiecePrefabs[index];
    }

    public void PlaceGamePiece(GamePiece piece, int x, int y)
    {
        if (piece == null)
            return;

        piece.transform.position = new Vector3(x, y, 0);
        piece.transform.rotation = Quaternion.identity;

        piece.SetCoord(x, y);
    }

    void FillRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomPiece = Instantiate(RandomGamePiece(), Vector3.zero, Quaternion.identity);
                if (randomPiece != null)
                {
                    GamePiece p = randomPiece.GetComponent<GamePiece>();
                    p.Init(this);
                    PlaceGamePiece(p, i, j);
                    randomPiece.transform.parent = dotParent.transform;
                }
            }
        }
    }

    public void ClickDot(GamePiece dot)
    {
        if (m_clickedDot == null)
            m_clickedDot = dot;
    }

    public void DragDot(GamePiece dot)
    {
        if (m_clickedDot != null && IsNextTo(dot, m_clickedDot))
            m_targetDot = dot;
    }

    public void ReleaseDot()
    {
        if (m_clickedDot != null && m_targetDot != null)
        {
            SwitchDot(m_clickedDot, m_targetDot);
        }

        m_clickedDot = null;
        m_targetDot = null;
    }

    void SwitchDot(GamePiece clickedDot, GamePiece targetDot)
    {
        m_clickedDot.Move(targetDot.xCoord, targetDot.yCoord, swapTime);
        m_targetDot.Move(clickedDot.xCoord, clickedDot.yCoord, swapTime);
    }

    bool IsNextTo(GamePiece start, GamePiece end)
    {
        if (Mathf.Abs(start.xCoord - end.xCoord) == 1 && start.yCoord == end.yCoord)
            return true;

        if (Mathf.Abs(start.yCoord - end.yCoord) == 1 && start.xCoord == end.xCoord)
            return true;

        return false;
    }
}