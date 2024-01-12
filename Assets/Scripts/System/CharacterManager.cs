using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    public class CharacterManager
    {
        readonly List<GameCharacter> manageCharaList;
        readonly Dictionary<int, int> charaLayerDic;

        public CharacterManager()
        {
            charaLayerDic = new();
            for (var index = 0; index < (int)CollisionManager.ObjectType.Length; index++)
            {
                charaLayerDic.Add(index, LayerMask.NameToLayer(((CollisionManager.ObjectType)index).ToString()));
            }

            manageCharaList = new();

            CollisionManager.Instance.SetUp();
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
                    CollisionManager.Instance.Remove(obj);
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

            CollisionManager.Instance.OnUpdate();
        }

        public GameCharacter CreateChara(CollisionManager.ObjectType objectType)
        {
            GameCharacter obj;

            switch (objectType)
            {
                case CollisionManager.ObjectType.Player:
                {
                    var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.PLAYER);
                    obj = GameObject.Instantiate(preafab).GetComponent<GameCharacter>();
                }
                break;

                case CollisionManager.ObjectType.Enemy:
                {
                    var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.ENEMY);
                    obj = GameObject.Instantiate(preafab).GetComponent<GameCharacter>();
                }
                break;

                case CollisionManager.ObjectType.P_Bullet:
                case CollisionManager.ObjectType.E_Bullet:
                {
                    obj = ObjectPooler.Instance.GetObject().GetComponent<GameCharacter>();
                }
                break;

                default:
                return null;
            }

            obj.Initialize(charaLayerDic[(int)objectType]);
            manageCharaList.Add(obj);
            CollisionManager.Instance.AddList(obj.gameObject);

            return obj;
        }
    }
}
