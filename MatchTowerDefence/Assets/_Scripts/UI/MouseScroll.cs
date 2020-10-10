using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class MouseScroll : MonoBehaviour
    {
        public bool clampScroll = true;

        public float scrollXBuffer;
        public float scrollYBuffer;

        protected ScrollRect scrollRect;
        protected RectTransform rectTransform;
        protected bool overrideScrolling, hasRightBuffer;

        public void SetHasRightBuffer(bool rightBuffer)
        {
            hasRightBuffer = rightBuffer;
        }

        // Start is called before the first frame update
        private void Start()
        {
            #if UNITY_STANDALONE
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.enabled = false;
            overrideScrolling = true;
            rectTransform = (RectTransform)scrollRect.transform;
            #else
            overrideScrolling = true;
            #endif
        }

        // Update is called once per frame
        private void Update()
        {
            if (!overrideScrolling) { return; }

            Vector3 mousePosition = Input.mousePosition;

            bool inside = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);

            if(!inside) { return; }

            Rect rect = rectTransform.rect;

            float adjustmentX = rect.width * scrollXBuffer, adjustmentY = rect.height * scrollYBuffer;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPoint);

            Vector2 pivot = rectTransform.pivot;
            float x = (localPoint.x + (rect.width - adjustmentX) * pivot.x) / (rect.width - 2 * adjustmentX);
            float y = (localPoint.y + (rect.height - adjustmentY) * pivot.y) / (rect.height - 2 * adjustmentY);

            if(clampScroll)
            {
                x = Mathf.Clamp01(x);
                y = Mathf.Clamp01(y);
            }

            scrollRect.normalizedPosition = new Vector2(x, y);
        }

        public void SelectChild(LevelSelectButton levelSelectButton)
        {
            int childCount = levelSelectButton.transform.parent.childCount - (hasRightBuffer ? 1 : 0);
            if(childCount > 1)
            {
                float normalised = (float)levelSelectButton.transform.GetSiblingIndex() / (childCount - 1);
                scrollRect.normalizedPosition = new Vector2(normalised, 0);
            }
        }
    }
}
