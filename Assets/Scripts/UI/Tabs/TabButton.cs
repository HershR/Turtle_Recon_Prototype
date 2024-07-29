using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TabSystemUI
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image backgroundImage;
        private TabGroup tabGroup;
        private void Start()
        {
            tabGroup = GetComponentInParent<TabGroup>();
            tabGroup.Add(this);
        }
        public void SetBGColor(Color color)
        {
            backgroundImage.color = color;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

    }
}