using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

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
            Container.BindInterfacesAndSelfTo<FieldController>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<GravityController>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.BindFactory<Boundary, Boundary.Factory>()
                .FromComponentInNewPrefab(settings.boundaryPrefab)
                .UnderTransformGroup("Boundaries");

            Container.BindFactory<Target, Target.Factory>()
                .FromComponentInNewPrefab(settings.targetPrefab)
                .UnderTransformGroup("Targets");
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject boundaryPrefab;
            public GameObject targetPrefab;
        }
    }
}
