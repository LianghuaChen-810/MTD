using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace LevelEditor
{
    public class LRouteOptionDeleteBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LEditorManager.GetInstance().routes.Remove(route);
            LAddRouteBtn.options.Remove(option.GetComponent<RectTransform>());
            LAddRouteBtn.RefreshOptions();

            LMonsterOptionRouteDp.dropdownnum -= 1;
            Destroy(option);

        }

        public LRoute route;
        public GameObject option;
    }
}
