using System;
using UnityEngine;
using Zenject;

namespace GravityDJ.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private Settings settings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<Spawner>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindFactory<Boundary, Boundary.Factory>()
                .FromComponentInNewPrefab(settings.boundaryPrefab)
                .UnderTransformGroup("Boundaries");
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject boundaryPrefab;    
        }
    }
}
