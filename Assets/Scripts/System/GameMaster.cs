using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.System
{
    public class GameMaster : SingletonMonoBehaviour<GameMaster>
    {
        [SerializeField] new Camera camera;
        [SerializeField] Vector3 setPlayerPos = new Vector3(0f, -3f, 0f);

        [SerializeField] DisappearWall[] disappearWalls;

        CharacterManager characterManager = null;
        public CharacterManager CharacterManager => characterManager;

        WaveManager waveManager = null;

        readonly Dictionary<int, string> charaAddressDic = new()
        {
            {(int)ObjectType.Player_Piko, "player_piko"},
            {(int)ObjectType.Enemy_Mon, "enemy_mon"},
            {(int)ObjectType.Player_Bullet, "bullet_bubble"},
            {(int)ObjectType.Enemy_Bullet, "bullet_banana"},
            {(int)ObjectType.Weapon_OverBath, "weapon_over_bath"},
            {(int)ObjectType.Weapon_Gun, "weapon_gun"},
        };

        bool initialized = false;

        PlayerController player = null;

        async void Start()
        {
            if (CheckInstance())
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                return;
            }

            initialized = false;

            await LoadAssets();

            InputManager.Instance.Setup();

            characterManager = new CharacterManager();

            CreateCharaFactory();

            CreatePlayer();
            SetupDisappearWall();

            waveManager = new();

            initialized = true;
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

        void CreateCharaFactory()
        {
            // todo: 以下の処理は仮。ファクトリーはレイヤーごとで分ける必要があるかも。
            const int BULLET_POOL_MAX = 32;
            var playerBullet = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Player_Bullet]);
            characterManager.AddFactoryCharacter((int)ObjectType.Player_Bullet, new Pool(playerBullet.GetComponent<GameCharacter>(), BULLET_POOL_MAX));
            var enemyBullet = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Enemy_Bullet]);
            characterManager.AddFactoryCharacter((int)ObjectType.Enemy_Bullet, new Pool(enemyBullet.GetComponent<GameCharacter>(), BULLET_POOL_MAX));
            var player = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Player_Piko]);
            characterManager.AddFactoryCharacter((int)ObjectType.Player_Piko, new SingleUnit(player.GetComponent<GameCharacter>()));
            var enemy = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Enemy_Mon]);
            characterManager.AddFactoryCharacter((int)ObjectType.Enemy_Mon, new SingleUnit(enemy.GetComponent<GameCharacter>()));
            var wepon = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Weapon_OverBath]);
            characterManager.AddFactoryCharacter((int)ObjectType.Weapon_OverBath, new SingleUnit(wepon.GetComponent<GameCharacter>()));
            var gun = ResourceStore.Instance.Get(charaAddressDic[(int)ObjectType.Weapon_Gun]);
            characterManager.AddFactoryCharacter((int)ObjectType.Weapon_Gun, new SingleUnit(gun.GetComponent<GameCharacter>()));
        }

        void CreatePlayer()
        {
            player = characterManager.CreateChara(ObjectType.Player_Piko) as PlayerController;
            player.transform.position = setPlayerPos;
        }

        void SetupDisappearWall()
        {
            foreach (var wall in disappearWalls)
            {
                characterManager.SetChara(LayerType.Wall, wall);
            }
        }

        async Task LoadAssets()
        {
            List<Task> taskList = new();
            foreach (var address in charaAddressDic.Values)
            {
                taskList.Add(ResourceStore.Instance.Load(address));
            }

            await Task.WhenAll(taskList);
        }

        void Update()
        {
            if (!initialized) { return; }

            InputManager.Instance.OnUpdate();
            waveManager.OnUpdate();
            characterManager.OnUpdate();
            PlayerScreenCheck();
        }

        void PlayerScreenCheck()
        {
            var playerPos = player.transform.position;
            var playerRadius = player.Radius;
            var playerTopSeg = player.StartSegment;
            var screenTopLeft = GetCameraTopLeft();

            if (screenTopLeft.x > playerPos.x - playerRadius)
            {
                var diff = (playerPos.x - playerRadius) - screenTopLeft.x;
                playerPos.x -= diff;
                player.transform.position = playerPos;
            }
            if (screenTopLeft.y < playerTopSeg.y + playerRadius)
            {
                var diff = (playerTopSeg.y + playerRadius) - screenTopLeft.y;
                playerPos.y -= diff;
                player.transform.position = playerPos;
            }

            var playerBottomSeg = player.EndSegment;
            var screenBottomRight = GetCameraBottomRight();

            if (screenBottomRight.x < playerPos.x + playerRadius)
            {
                var diff = (playerPos.x + playerRadius) - screenBottomRight.x;
                playerPos.x -= diff;
                player.transform.position = playerPos;
            }
            if (screenBottomRight.y > playerBottomSeg.y - playerRadius)
            {
                var diff = (playerBottomSeg.y - playerRadius) - screenBottomRight.y;
                playerPos.y -= diff;
                player.transform.position = playerPos;
            }
        }
    }
}
