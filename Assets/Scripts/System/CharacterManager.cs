using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトのレイヤー（文字列をそのままUnityのlayerとして扱う）
    /// </summary>
    public enum LayerType
    {
        Player = 0,
        Enemy,
        P_Bullet,
        E_Bullet,
        Weapon,

        Length,
    }

    /// <summary>
    /// 使用するオブジェクト
    /// </summary>
    public enum ObjectType
    {
        Player_Piko,
        Enemy_Mon,
        Player_Bullet,
        Enemy_Bullet,
        Weapon_OverBath,

        Num,
    }

    public abstract class CharacterFactory
    {
        public abstract GameCharacter GetCharacter();
        public abstract void ReleseCharacter(GameCharacter gameCharacter);
    }

    public class SingleUnit : CharacterFactory
    {
        readonly GameCharacter origin;
        public SingleUnit(GameCharacter origin)
        {
            this.origin = origin;
        }
        public override GameCharacter GetCharacter() => GameObject.Instantiate(origin).GetComponent<GameCharacter>();
        public override void ReleseCharacter(GameCharacter gameCharacter) => GameObject.Destroy(gameCharacter);
    }

    public class Pool : CharacterFactory
    {
        readonly ObjectPooler<GameCharacter> pooler;
        public Pool(GameCharacter origin, int capacity)
        {
            pooler = new(origin, capacity);
        }
        public override GameCharacter GetCharacter() => pooler.Get();
        public override void ReleseCharacter(GameCharacter gameCharacter)
        {
            // todo: ObjectPoolerにResourceを追加する。
        }
    }

    public class CharacterManager
    {
        readonly List<GameCharacter> manageCharaList;
        readonly Dictionary<int, int> charaLayerDic;

        readonly CollisionManager collisionManager;

        readonly Dictionary<int, CharacterFactory> characterFactoryDic;
        readonly Dictionary<int, LayerType> objectLayerDic = new()
        {
            {(int)ObjectType.Player_Piko, LayerType.Player},
            {(int)ObjectType.Player_Bullet, LayerType.P_Bullet},
            {(int)ObjectType.Enemy_Mon, LayerType.Enemy},
            {(int)ObjectType.Enemy_Bullet, LayerType.E_Bullet},
            {(int)ObjectType.Weapon_OverBath, LayerType.Weapon},
        };

        public CharacterManager()
        {
            charaLayerDic = new();
            for (var index = 0; index < (int)LayerType.Length; index++)
            {
                charaLayerDic.Add(index, LayerMask.NameToLayer(((LayerType)index).ToString()));
            }

            manageCharaList = new();
            collisionManager = new();

            characterFactoryDic = new();
            const int BULLET_POOL_MAX = 32;
            // todo: リソースはAddressableを使用する。何かしらのデータを使用して、forearchで回せるように。
            var playerBullet = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
            characterFactoryDic.Add((int)ObjectType.Player_Bullet, new Pool(playerBullet.GetComponent<GameCharacter>(), BULLET_POOL_MAX));
            var enemyBullet = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
            characterFactoryDic.Add((int)ObjectType.Enemy_Bullet, new Pool(enemyBullet.GetComponent<GameCharacter>(), BULLET_POOL_MAX));
            var player = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.PLAYER);
            characterFactoryDic.Add((int)ObjectType.Player_Piko, new SingleUnit(player.GetComponent<GameCharacter>()));
            var enemy = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.ENEMY);
            characterFactoryDic.Add((int)ObjectType.Enemy_Mon, new SingleUnit(enemy.GetComponent<GameCharacter>()));
            var wepon = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.OVER_BATH);
            characterFactoryDic.Add((int)ObjectType.Weapon_OverBath, new SingleUnit(wepon.GetComponent<GameCharacter>()));
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

                    // todo: オブジェクト(****)ごとで処分の仕方を変える
                    //characterFactoryDic[****].ReleseCharacter(obj);
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

        /// <summary>
        /// 指定したリソースを実体化し、管理に回します。
        /// </summary>
        /// <param name="objectType">リソース</param>
        /// <returns>初期化済オブジェクト</returns>
        public GameCharacter CreateChara(ObjectType objectType)
        {
            GameCharacter obj = characterFactoryDic[(int)objectType].GetCharacter();
            obj.Initialize(charaLayerDic[(int)objectLayerDic[(int)objectType]]);
            manageCharaList.Add(obj);
            collisionManager.AddList(obj);

            return obj;
        }
    }
}
