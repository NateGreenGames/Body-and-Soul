using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction, speed * Time.deltaTime);
    }
}
