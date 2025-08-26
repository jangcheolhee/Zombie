using UnityEngine;

public class Rotation : MonoBehaviour
{
    float speed = 100f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, speed  * Time.deltaTime, 0);
    }
}
