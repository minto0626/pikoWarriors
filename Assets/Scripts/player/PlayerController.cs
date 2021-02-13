using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    Vector3 shotPoint = new Vector3(0f, 0.3f, 0f);

    float moveSpeed = 0.05f;

    private void Update()
    {
        MoveControl();
        BulletShot();
    }

    private void MoveControl()
    {
        // 上
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(0f, moveSpeed, 0f);
        }// 左
        else if (Input.GetKey(KeyCode.A)){
            gameObject.transform.Translate(-moveSpeed, 0f, 0f);
        }// 下
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(0f, -moveSpeed, 0f);
        } // 右
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(moveSpeed, 0f, 0f);
        }
    }

    private void BulletShot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OneShot();
        }
    }

    private void OneShot()
    {
        var bullet = ObjectPooler.Instance.GetObject();
        bullet.transform.position = transform.localPosition;
    }
}
