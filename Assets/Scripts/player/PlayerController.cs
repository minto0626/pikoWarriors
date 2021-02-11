using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 0.05f;

    private void Update()
    {
        MoveControl();
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
}
