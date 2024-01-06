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
    /// 当たり判定パラメータ設定
    /// </summary>
    /// <param name="ct">当たり判定の形</param>
    /// <param name="ot">オブジェクトのレイヤー</param>
    /// <param name="r">半径</param>
    /// <param name="h">高さ</param>
    public ObjectCollision(CollisionManager.CollisionType ct, CollisionManager.ObjectType ot, float r = 0f, float h = 0f)
    {
        collType = ct;
        objType = ot;
        gameObject.layer = LayerMask.NameToLayer(ot.ToString());
        radius = r;
        height = h;
    }

    public void SetUp(CollisionManager.CollisionType ct, CollisionManager.ObjectType ot, float r = 0f, float h = 0f)
    {
        collType = ct;
        objType = ot;
        gameObject.layer = LayerMask.NameToLayer(ot.ToString());
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
