using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 5f; 
    private Vector3[] corners; 
    private int currentCornerIndex = 0; 

    private void Start()
    {
        corners = new Vector3[]
        {
            new Vector3(-16, 17.2f, 0),  
            new Vector3(-9.6f, 17.2f, 0),   
            new Vector3(-9.6f, 12, 0),  
            new Vector3(-16, 12, 0)  
        };
    }

    private void Update()
    {
        Vector3 direction = (corners[currentCornerIndex] - transform.position).normalized;

        float moveAmount = speed * Time.deltaTime;

        transform.position += direction * moveAmount;

        if (Vector3.Distance(transform.position, corners[currentCornerIndex]) <= moveAmount)
        {
            currentCornerIndex = (currentCornerIndex + 1) % corners.Length;
        }
    }
}
