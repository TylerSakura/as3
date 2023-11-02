using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5.0f;
    public LayerMask wallLayer;

    private Vector2 lastInput;
    private Vector2 currentInput;
    private Vector2 nextPosition;
    private bool isLerping;

    void Start()
    {
        nextPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        if (!isLerping)
        {
            TryMove(lastInput);
            if (CanMoveTo(nextPosition + currentInput))
            {
                StartLerp(nextPosition + currentInput);
            }
        }
        else
        {
            ContinueLerp();
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W)) lastInput = Vector2.up;
        else if (Input.GetKey(KeyCode.A)) lastInput = Vector2.left;
        else if (Input.GetKey(KeyCode.S)) lastInput = Vector2.down;
        else if (Input.GetKey(KeyCode.D)) lastInput = Vector2.right;
    }

    void TryMove(Vector2 direction)
    {
        if (CanMoveTo(nextPosition + direction))
        {
            StartLerp(nextPosition + direction);
            currentInput = direction;
        }
    }

    bool CanMoveTo(Vector2 position)
    {
        return !Physics2D.OverlapCircle(position, 0.1f, wallLayer);
    }

    void StartLerp(Vector2 position)
    {
        nextPosition = position;
        isLerping = true;
    }

    void ContinueLerp()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, nextPosition) < 0.01f)
        {
            transform.position = nextPosition;
            isLerping = false;
        }
    }
}
