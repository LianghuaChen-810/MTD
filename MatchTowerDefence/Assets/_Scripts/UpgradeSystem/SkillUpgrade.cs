using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UpgradeSystem
{
    [RequireComponent(typeof(Image))]
    public class SkillUpgrade : MonoBehaviour
    {
        [SerializeField] private Image lockedImage;

        private void Awake()
        {
            lockedImage = GetComponentInChildren<Image>();
        }

        public void OnUpgrade()
        {
            lockedImage.gameObject.SetActive(false);
        }
    }
}