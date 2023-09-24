using Assets.Packages.Engine.Audio;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    public static class UIHelper
    {
        public static void RemoveAllChildren(VisualElement element)
        {
            if (element.childCount == 0)
                return;
            element.RemoveAt(0);
            if (element.childCount > 0)
                RemoveAllChildren(element);
        }


        public static void InitializeRoot(VisualElement ve, bool withAnim = true)
        {
            ve.style.flexGrow = 1;
            if (withAnim)
                HideRoot(ve);
        }


        public static void ShowRoot(VisualElement ve)
        {
            ve.Q<VisualElement>("root").RemoveFromClassList("hide-root");
        }


        public static void HideRoot(VisualElement ve)
        {
            ve.Q<VisualElement>("root").AddToClassList("hide-root");
        }


        public static void ButtonClickSoundSubscribe(VisualElement root)
        {
            root.Query<Button>().ForEach((btn) => btn.clicked += AudioUI.Instance.PlayButtonClick);
        }
    }
}
