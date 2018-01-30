using System.Collections;
using UnityEngine;

public enum InterpType
{
    Linear,
    EaseOut,
    EaseIn,
    SmoothStep,
    SmootherStep
}

public class GamePiece : MonoBehaviour
{
    public int xCoord;
    public int yCoord;
    Board m_Board;
    bool isMoving;
    [SerializeField] InterpType interpolation = InterpType.SmootherStep;

    void Awake()
    {
    }

    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//            Move(new Vector2(transform.position.x - 1, transform.position.y), 0.5f);
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//            Move(new Vector2(transform.position.x + 1, transform.position.y), 0.5f);
    }

    public void Init(Board board)
    {
        m_Board = board;
    }

    public void SetCoord(int x, int y)
    {
        xCoord = x;
        yCoord = y;
    }

    public void Move(float destX, float destY, float timeToMove)
    {
        if (!isMoving)
            StartCoroutine(MoveRoutine(new Vector2(destX, destY), timeToMove));
    }

    IEnumerator MoveRoutine(Vector2 destination, float timeToMove)
    {
        bool reachDestination = false;
        float elapsedTime = 0f;

        isMoving = true;

        while (!reachDestination)
        {
            if (Vector2.Distance(transform.position, destination) < 0.01f)
            {
                reachDestination = true;
                if (m_Board != null)
                {
                    m_Board.PlaceGamePiece(this, (int) destination.x, (int) destination.y);
                }
                break;
            }
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            switch (interpolation)
            {
                case InterpType.Linear:
                    break;
                case InterpType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseIn:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.SmoothStep:
                    t = t * t * (3 - 2 * t);
                    break;
                case InterpType.SmootherStep:
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }

            transform.position = Vector2.Lerp(transform.position, destination, t);
            yield return null;
        }
        isMoving = false;
    }

    void OnMouseDown()
    {
        if (m_Board != null)
        {
            m_Board.ClickDot(this);
        }
    }

    void OnMouseEnter()
    {
        if (m_Board != null)
        {
            m_Board.DragDot(this);
        }
    }

    void OnMouseUp()
    {
        if (m_Board != null)
        {
            m_Board.ReleaseDot();
        }
    }
}