using PotionsPlease.InGame;
using PotionsPlease.Util.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class GameManager : ManagerBase<GameManager>
    {
        public static Camera CameraMain => Static.GetComponentByNameProperty(ref _cameraMain, "Main Camera");
        private static Camera _cameraMain;

        public ItemObject DraggedItemCurrent { get; set; }

        [field: SerializeField] public CauldronObject Cauldron { get; private set; }
        [field: SerializeField] public DropAreaObject DropAreaObject { get; private set; }


        private void Start()
        {
            _cameraMain = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}