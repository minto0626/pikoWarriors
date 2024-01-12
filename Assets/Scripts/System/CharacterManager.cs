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

        Length,
    }

    public class CharacterManager
    {
        readonly List<GameCharacter> manageCharaList;
        readonly Dictionary<int, int> charaLayerDic;

        readonly CollisionManager collisionManager;

        public CharacterManager()
        {
            charaLayerDic = new();
            for (var index = 0; index < (int)ObjectType.Length; index++)
            {
                charaLayerDic.Add(index, LayerMask.NameToLayer(((ObjectType)index).ToString()));
            }

            manageCharaList = new();
            collisionManager = new();

            ObjectPooler.Instance.SetUp();
            var b = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
            const int BULLET_MAX = 30;
            ObjectPooler.Instance.CreatePool(b, BULLET_MAX);
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
                    obj = ObjectPooler.Instance.GetObject().GetComponent<GameCharacter>();
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
