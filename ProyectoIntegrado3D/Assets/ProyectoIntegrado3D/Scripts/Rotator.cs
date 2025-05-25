using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void FixedUpdate()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.x + 15, transform.rotation.y + 30, transform.rotation.z + 45);

        transform.eulerAngles += new Vector3(1, 2, 3);
    }
}
