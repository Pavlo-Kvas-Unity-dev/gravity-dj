using Zenject;

namespace GravityDJ.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
