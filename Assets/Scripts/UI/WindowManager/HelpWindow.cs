namespace GravityDJ.UI
{
    public class HelpWindow : WindowBase
    {
        public void OnReturnButtonClicked()
        {
            WindowManager.Instance.CloseWindow(gameObject.scene.name);
        }
    }
}
