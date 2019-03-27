using UnityEngine;
using Zenject;

namespace GravityDJ.Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameController.Settings GameSettings;
        public Spawner.Settings SpawnerSettings;
        public FieldController.Settings FieldSettings;
        public GravityController.Settings GravitySettings;
        public GameInstaller.Settings GameInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameSettings);
            Container.BindInstance(SpawnerSettings);
            Container.BindInstance(FieldSettings);
            Container.BindInstance(GravitySettings);
            Container.BindInstance(GameInstaller);
        }
    }
}