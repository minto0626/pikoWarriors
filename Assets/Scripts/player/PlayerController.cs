using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 弾の発射位置
    /// </summary>
    /// <returns></returns>
    Vector3 shotPoint = new Vector3(0f, 0.3f, 0f);
    /// <summary>
    /// プレイヤーの移動速度
    /// </summary>
    float moveSpeed = 0.05f;

    /// <summary>
    /// 発射可能フラグ
    /// </summary>
    bool isShot = false;
    /// <summary>
    /// 1ショットの発射回数
    /// </summary>
    float SHOT_LIMIT = 4;

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
            // 弾を発射していなかったら撃てる
            if (!isShot) 
            {
                StartCoroutine(OhuroShot());
            }
        }
    }

    private void OneShot()
    {
        var bullet = ObjectPooler.Instance.GetObject();
        bullet.transform.position = transform.localPosition + shotPoint;
        CollisionManager.Instance.AddList(bullet.GetComponent<ObjectCollision>());
    }

    /// <summary>
    /// オーバーフロッシャーのようなショット
    /// </summary>
    /// <returns></returns>
    private IEnumerator OhuroShot()
    {
        // 撃った数のカウント
        var shotCount = 0;
        // 撃ち始めのタイマー
        var shotTimer = 0f;
        
        isShot = true;

        while(true)
        {
            if(shotCount == SHOT_LIMIT)
            {
                isShot = false;
                yield break;
            }

            // 2fごとに弾を出す
            if(shotTimer >= 0.1f * shotCount)
            {
                OneShot();
                shotCount++;
            }

            shotTimer += Time.deltaTime;
            yield return null;
        }
    }
}
