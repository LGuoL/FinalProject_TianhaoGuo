using UnityEngine;

public class WalkingFish : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float destroyDistance = 0.5f;

    private void Update()
    {
        if (target == null)
            return;

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        Vector3 dir = targetPos - transform.position;

        if (dir.magnitude <= destroyDistance)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += dir.normalized * moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }
}