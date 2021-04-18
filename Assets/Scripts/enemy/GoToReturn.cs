using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行って帰る
/// </summary>
public class GoToReturn : EnemyBase
{
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

        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (base.Disappear()) break;
            transform.position -= movePos;
            yield return null;
        }
    }
}
