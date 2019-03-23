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
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameSettings);
            Container.BindInstance(SpawnerSettings);
            Container.BindInstance(FieldSettings);
        }
    }
}