using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : SingletonMonoBehaviour<CollisionManager>
{
    /// <summary>
    /// オブジェクトの種類
    /// </summary>
    public enum ObjectType
    {
        Player = 0,
        Enemy,
        P_Bullet,
        E_Bullet,
        Length
    }

    /// <summary>
    /// コリジョンの種類
    /// </summary>
    public enum CollisionType
    {
        Circle = 0,
        Square,
        Capsule,
        Length
    }

    /// <summary>
    /// コリジョンが付いているオブジェクトのリスト
    /// </summary>
    ObjectCollision[] targetList;

    /// <summary>
    /// 管理する最大数
    /// </summary>
    int MANAGE_MAX = 256;

    /// <summary>
    /// 管理中の個数カウンター
    /// </summary>
    int targetCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetUp()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }

        targetList = new ObjectCollision[MANAGE_MAX];
    }

    /// <summary>
    /// 管理するオブジェクトを追加
    /// </summary>
    /// <param name="oc"></param>
    public void AddList(ObjectCollision oc)
    {
        for(var i = 0; i < MANAGE_MAX; i++)
        {
            if (targetList[i] != null) continue;
            oc.collisionID = i;
            oc.isHit = false;
            targetList[i] = oc;
            targetCount++;
            break;
        }
    }

    /// <summary>
    /// 管理しなくなったオブジェクトを削除
    /// </summary>
    /// <param name="oc"></param>
    public void Remove(ObjectCollision oc)
    {
        targetList[oc.collisionID] = null;
        targetCount--;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void OnUpdate()
    {
        JudgmentUpdate();
    }

    /// <summary>
    /// コリジョンの判定更新
    /// </summary>
    void JudgmentUpdate()
    {
        for (var i = 0; i < targetList.Length - 1; i++)
        {
            var c = targetList[i];
            if (c == null) continue;
            if (c.collisionID > targetCount) break;

            for (var j = i + 1; j < targetList.Length; j++)
            {
                var t = targetList[j];
                if (t == null) continue;
                if (t.collisionID > targetCount) break;

                if (!CheckInterferenceObj(c.GetObjectType, t.GetObjectType)) continue;

                if (CircleToCircle(c, t))
                {
                    c.isHit = true;
                    t.isHit = true;

                    // 試しに消してみる
                    Remove(c);
                    Remove(t);
                    c.gameObject.SetActive(false);
                    t.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 干渉しあうかどうかの判定
    /// </summary>
    /// <param name="c"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    bool CheckInterferenceObj(ObjectType c, ObjectType t)
    {
        switch (c)
        {
            case ObjectType.Player:
                if (t == ObjectType.P_Bullet) return false;
                break;
            case ObjectType.Enemy:
                if (t == ObjectType.Enemy) return false;
                if (t == ObjectType.E_Bullet) return false;
                break;
            case ObjectType.P_Bullet:
                if (t == ObjectType.Player) return false;
                if (t == ObjectType.P_Bullet) return false;
                break;
            case ObjectType.E_Bullet:
                if (t == ObjectType.Enemy) return false;
                if (t == ObjectType.E_Bullet) return false;
                break;
        }

        return true;
    }

    /// <summary>
    /// 円と円
    /// </summary>
    /// <param name="current">比較元</param>
    /// <param name="target">比較対象</param>
    /// <returns></returns>
    bool CircleToCircle(ObjectCollision current, ObjectCollision target)
    {
        if (current.isHit || target.isHit) return false;

        var cr = current.GetRadius;
        var tr = target.GetRadius;
        var dis = (current.GetPosision - target.GetPosision).sqrMagnitude;
        var calcDis = (cr + tr) * (cr + tr);

        if (dis < calcDis) return true;

        return false;
    }
}
