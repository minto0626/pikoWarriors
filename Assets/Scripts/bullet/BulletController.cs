using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float moveSpeed = 0.1f;

    void Update()
    {
        gameObject.transform.Translate(0f, moveSpeed, 0f);
    }
}
