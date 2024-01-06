using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;
    protected Vector3 movePos = Vector3.zero;
    protected GameMaster gm = null;

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Setup()
    {
        gm = GameMaster.Instance;
    }

    /// <summary>
    /// 弾を一発出す
    /// </summary>
    /// <param name="shotPoint">出す座標</param>
    protected void OneShot(Vector3 shotPoint)
    {
        var bullet = ObjectPooler.Instance.GetObject();
        bullet.GetComponent<ObjectCollision>().SetUp(CollisionManager.CollisionType.Circle, 
                                                        CollisionManager.ObjectType.E_Bullet,
                                                        0.25f);
        bullet.transform.position = transform.localPosition + shotPoint;
    }

    /// <summary>
    /// 画面外なら非表示
    /// </summary>
    protected bool Disappear()
    {
        // +1して画面外で消えるようにする
        if ((gm.GetCameraTopLeft().y + 1f) < transform.position.y)
        {
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}
