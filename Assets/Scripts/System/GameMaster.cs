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

        CharacterManager characterManager = null;
        public CharacterManager CharacterManager => characterManager;

        void Start()
        {
            if (CheckInstance())
            {
                DontDestroyOnLoad(gameObject);
            }

            InputManager.Instance.Setup();

            characterManager = new CharacterManager();

            CreatePlayer();
            CreateEnemy();
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
            var player = characterManager.CreateChara(ObjectType.Player_Piko);
            player.transform.position = setPlayerPos;
        }

        void CreateEnemy()
        {
            foreach (var pos in generatePos)
            {
                var enemy = characterManager.CreateChara(ObjectType.Enemy_Mon);
                enemy.transform.position = pos.transform.position;
            }
        }

        void Update()
        {
            InputManager.Instance.OnUpdate();
            characterManager.OnUpdate();
        }
    }
}
