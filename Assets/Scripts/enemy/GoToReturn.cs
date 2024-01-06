using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行って帰る
/// </summary>
public class GoToReturn : EnemyBase
{
    /// <summary>
    /// 弾の発射位置
    /// </summary>
    /// <returns></returns>
    Vector3 shotPoint = new Vector3(0f, -0.3f, 0f);

    private void Start()
    {
        base.Setup();
        movePos.y = base.moveSpeed;
        StartCoroutine(Move());
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        while (true)
        {
            if (gameObject.transform.position.y < 0f) break;
            transform.position += movePos;
            yield return null;
        }

        // 弾を撃つ
        yield return StartCoroutine(StraightShot());

        while (true)
        {
            if (base.Disappear()) break;
            transform.position -= movePos;
            yield return null;
        }
    }

    /// <summary>
    /// まっすぐ撃つショット
    /// </summary>
    /// <returns></returns>
    private IEnumerator StraightShot()
    {
        yield return new WaitForSeconds(1.5f);
        base.OneShot(shotPoint);
        yield return new WaitForSeconds(1.5f);
    }
}
