using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UpgradeSystem
{
    public class SkillUpgrade : MonoBehaviour
    {
        [SerializeField] private Image lockedImage = null;
        [SerializeField] private TowerObject[] towerObjects = null;
        [SerializeField] private string towerAlignment = null;
        [SerializeField] private float upgradeValue = 0f;
        [SerializeField] private int costOfUpgrade = 0;
        [SerializeField] private int attributeId = 0;
        private bool isUnlocked = false;

        public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; } }


        private void Start()
        {
            if(isUnlocked) { lockedImage.gameObject.SetActive(false); }
        }

        public void OnUpgrade()
        {
            if (!isUnlocked)
            {
                lockedImage.gameObject.SetActive(false);
                isUnlocked = true;


                foreach (TowerObject towerObject in towerObjects)
                {
                    switch (attributeId)
                    {
                        case 1:
                            Debug.Log(towerObject.baseDamage);
                            towerObject.baseDamage += upgradeValue;
                            Debug.Log(towerObject.baseDamage);
                            break;
                        case 2:

                            towerObject.shootDelayTime -= upgradeValue;
                            break;
                    }
                }
            }
        }
    }
}