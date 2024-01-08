using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    public class GameMaster : SingletonMonoBehaviour<GameMaster>
    {
        [SerializeField] new Camera camera;
        [SerializeField] Vector3 setPlayerPos = new Vector3(0f, -3f, 0f);
        [SerializeField] GameObject[] generatePos;
        const int BULLET_MAX = 30;

        // エディタで見えるようにしておく
        public List<GameCharacter> managedGameCharacterList;
        EnemyGenerator enemyGenerator;
        public Dictionary<int, int> GameCharaLayerDic;

        void Start()
        {
            if (CheckInstance())
            {
                DontDestroyOnLoad(gameObject);
            }

            GameCharaLayerDic = new();
            for (var index = 0; index < (int)CollisionManager.ObjectType.Length; index++)
            {
                GameCharaLayerDic.Add(index, LayerMask.NameToLayer(((CollisionManager.ObjectType)index).ToString()));
            }

            InputManager.Instance.Setup();
            CollisionManager.Instance.SetUp();
            ObjectPooler.Instance.SetUp();
            enemyGenerator = new EnemyGenerator();
            enemyGenerator.LoadPrefab();
            managedGameCharacterList = new List<GameCharacter>();
            var b = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
            var pool = ObjectPooler.Instance.CreatePool(b, BULLET_MAX);
            CreatePlayer();
            foreach (var pos in generatePos)
            {
                var enemy = enemyGenerator.Generate(pos.transform.position);
                var chara = enemy.GetComponent<GameCharacter>();
                chara.Initialize(GameCharaLayerDic[(int)CollisionManager.ObjectType.Enemy]);
                managedGameCharacterList.Add(chara);
                CollisionManager.Instance.AddList(enemy);
            }
        }

        public Vector3 GetCameraTopLeft()
        {
            var tl = camera.ScreenToWorldPoint(Vector3.zero);
            tl.Scale(new Vector3(1f, -1f, 1f));
            return tl;
        }

        public Vector3 GetCameraBottomRight()
        {
            var br = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
            br.Scale(new Vector3(1f, -1f, 1f));
            return br;
        }

        void CreatePlayer()
        {
            var preafab = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.PLAYER);
            var player = Instantiate(preafab, setPlayerPos, Quaternion.identity);
            var chara = player.GetComponent<GameCharacter>();
            chara.Initialize(GameCharaLayerDic[(int)CollisionManager.ObjectType.Player]);
            managedGameCharacterList.Add(chara);
            CollisionManager.Instance.AddList(player);
        }

        void Update()
        {
            InputManager.Instance.OnUpdate();
            var remove = managedGameCharacterList.Where(chara => chara.IsDestroy);
            if (remove.Count() > 0)
            {
                var removeList = remove.ToArray();
                foreach (var obj in removeList)
                {
                    managedGameCharacterList.Remove(obj);
                    CollisionManager.Instance.Remove(obj);
                }
            }
            foreach (var gameCharacter in managedGameCharacterList)
            {
                gameCharacter.OnUpdate();
            }
            CollisionManager.Instance.OnUpdate();
        }
    }
}
