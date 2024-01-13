using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    public enum ObjectType
    {
        Player = 0,
        Enemy,
        P_Bullet,
        E_Bullet,
        Weapon,

        Length,
    }

    public class CharacterManager
    {
        readonly List<GameCharacter> manageCharaList;
        readonly Dictionary<int, int> charaLayerDic;

        readonly CollisionManager collisionManager;
        readonly ObjectPooler<GameCharacter> bulletPool;

        public CharacterManager()
        {
            charaLayerDic = new();
            for (var index = 0; index < (int)ObjectType.Length; index++)
            {
                charaLayerDic.Add(index, LayerMask.NameToLayer(((ObjectType)index).ToString()));
            }

            manageCharaList = new();
            collisionManager = new();

            var bulletPrefab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
            const int BULLET_POOL_MAX = 32;
            bulletPool = new(bulletPrefab.GetComponent<GameCharacter>(), BULLET_POOL_MAX);
        }

        public void OnUpdate()
        {
            var remove = manageCharaList.Where(chara => chara.IsDestroy);
            if (remove.Count() > 0)
            {
                var removeList = remove.ToArray();
                foreach (var obj in removeList)
                {
                    manageCharaList.Remove(obj);
                    collisionManager.Remove(obj);
                }
            }
            // todo: OnUpdate中に管理数が増減する処理があると、GetEnumeratorでエラーが発生する。
            //       現状インクリメント方式で対応しているが、いずれ削除＆追加は予約制を取る。
            // foreach (var chara in manageCharaList)
            for (var i = 0; i < manageCharaList.Count; i++)
            {
                //chara.OnUpdate();
                manageCharaList[i].OnUpdate();
            }

            collisionManager.OnUpdate();
        }

        public GameCharacter CreateChara(ObjectType objectType)
        {
            GameCharacter obj;

            // todo: 取り出し口共通化
            switch (objectType)
            {
                case ObjectType.Player:
                {
                    var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.PLAYER);
                    obj = GameObject.Instantiate(preafab).GetComponent<GameCharacter>();
                }
                break;

                case ObjectType.Enemy:
                {
                    var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.ENEMY);
                    obj = GameObject.Instantiate(preafab).GetComponent<GameCharacter>();
                }
                break;

                case ObjectType.P_Bullet:
                case ObjectType.E_Bullet:
                {
                    obj = bulletPool.Get();
                }
                break;

                case ObjectType.Weapon:
                {
                    var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.OVER_BATH);
                    obj = GameObject.Instantiate(preafab).GetComponent<GameCharacter>();
                }
                break;

                default:
                return null;
            }

            obj.Initialize(charaLayerDic[(int)objectType]);
            manageCharaList.Add(obj);
            collisionManager.AddList(obj);

            return obj;
        }
    }
}
