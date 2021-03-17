using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    /// <summary>
    /// コリジョンの種類
    /// </summary>
    [SerializeField] CollisionManager.CollisionType collType;

    /// <summary>
    /// オブジェクトの種類
    /// </summary>
    [SerializeField] CollisionManager.ObjectType objType;

    /// <summary>
    /// 半径
    /// </summary>
    [SerializeField] float radius;

    /// <summary>
    /// 高さ
    /// </summary>
    [SerializeField] float height;

    /// <summary>
    /// 管理用ID
    /// </summary>
    public int collisionID { get; set; }

    /// <summary>
    /// 何かに当たったか
    /// </summary>
    public bool isHit { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ct">コリジョンの種類</param>
    /// <param name="r">半径</param>
    /// <param name="h">高さ</param>
    public ObjectCollision(CollisionManager.CollisionType ct, float r, float h)
    {
        collType = ct;
        radius = r;
        height = h;
    }

    /// <summary>
    /// コリジョンの種類を取得
    /// </summary>
    public CollisionManager.CollisionType GetCollisionType
    {
        get { return collType; }
    }

    /// <summary>
    /// オブジェクトの種類を取得
    /// </summary>
    public CollisionManager.ObjectType GetObjectType
    {
        get { return objType; }
    }

    /// <summary>
    /// 半径を取得
    /// </summary>
    public float GetRadius
    {
        get { return radius; }
    }

    /// <summary>
    /// 高さを取得
    /// </summary>
    public float GetHeight
    {
        get { return height; }
    }

    /// <summary>
    /// 位置を取得
    /// </summary>
    public Vector3 GetPosision
    {
        get { return gameObject.transform.position; }
    }
}
