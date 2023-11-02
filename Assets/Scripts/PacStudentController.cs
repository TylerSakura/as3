using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5.0f;
    public LayerMask wallLayer;
    public AudioClip eatingClip;
    public AudioClip movingClip;

    private Vector2 lastInput;
    private Vector2 currentInput;
    private Vector2 nextPosition;
    private bool isLerping;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        nextPosition = transform.position;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if (Input.GetKey(KeyCode.W))
        {
            lastInput = Vector2.up;
            SetAnimationBools("W");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            lastInput = Vector2.left;
            SetAnimationBools("A");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            lastInput = Vector2.down;
            SetAnimationBools("S");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            lastInput = Vector2.right;
            SetAnimationBools("D");
        }
    }

    void SetAnimationBools(string activeBool)
    {
        animator.SetBool("W", activeBool == "W");
        animator.SetBool("A", activeBool == "A");
        animator.SetBool("S", activeBool == "S");
        animator.SetBool("D", activeBool == "D");
    }

    private bool CheckIfEating(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1.28f, LayerMask.GetMask("Pellet"));
        bool ateSomething = false;

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                audioSource.PlayOneShot(eatingClip);
                Destroy(hit.collider.gameObject);
                ateSomething = true;
            }
        }

        return ateSomething;
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
        audioSource.PlayOneShot(movingClip);
    }

    void ContinueLerp()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, nextPosition) < 0.01f)
        {
            transform.position = nextPosition;
            isLerping = false;
            CheckIfEating(currentInput);
        }
    }
}
