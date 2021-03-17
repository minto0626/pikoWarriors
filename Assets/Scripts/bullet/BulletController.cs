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

    /// <summary>
    /// 画面外なら非表示
    /// </summary>
    void Disappear()
    {
        // +1して画面外で消えるようにする
        if((gm.GetCameraTopLeft().y + 1f) < transform.position.y)
        {
            gameObject.SetActive(false);
            CollisionManager.Instance.Remove(gameObject.GetComponent<ObjectCollision>());
        }
    }
}
