using UnityEngine;

public class BucketTest : MonoBehaviour
{
    public BucketSystem bucketSystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bucketSystem.AddFish(1);
            Debug.Log("往桶里加了 1 条鱼");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            int removed = bucketSystem.RemoveFish(1);
            Debug.Log("从桶里拿走了 " + removed + " 条鱼");
        }
    }
}