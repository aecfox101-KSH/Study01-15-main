using UnityEngine;

public class MovingPlatfrom : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField]
    private float arrivedDistance = 0.05f;

    private int currentIndex = 0;
    private int moveDirection = 1;

    // Update is called once per frame
    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (wayPoints == null || wayPoints.Length < 2)
        {
            return;
        }

        Transform target = wayPoints[currentIndex];

        // target 정보가 유효한지 체크하는 코드를 넣어보시오.
        if (target == null)
        {
            return; 
        }

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;

        float step = moveSpeed * Time.deltaTime;
        Vector3 nextPos = Vector3.MoveTowards(currentPos, targetPos, step);

        transform.position = nextPos;

        float dist = Vector3.Distance(nextPos, targetPos);

        if (dist <= arrivedDistance)
        {
            currentIndex += moveDirection;

            if (currentIndex >= wayPoints.Length)
            {
                moveDirection = -1;
                currentIndex = wayPoints.Length - 2;
            }
            else if (currentIndex < 0)
            {
                moveDirection = +1;
                currentIndex = 1;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ==  false)
        {
            return; 
        }

        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == false)
        {
            return;
        }

        collision.transform.SetParent(null);
    }
}

