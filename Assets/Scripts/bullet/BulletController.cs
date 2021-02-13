using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float moveSpeed = 0.1f;
    GameMaster gm = null;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        gameObject.transform.Translate(0f, moveSpeed, 0f);

        Disappear();
    }

    void Setup()
    {
        gm = GameMaster.Instance;
    }

    }
}
