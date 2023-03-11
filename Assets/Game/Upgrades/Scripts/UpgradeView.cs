using Game.Scripts.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrades.Scripts
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private UpgradeSource _source;

        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _activeIcon;
        [SerializeField] private Sprite _inactiveIcon;

        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _lvlText;


        private void OnEnable()
        {
            _source.Data.OnUpgrade += UpdateView;
            ResourceHandler.OnValueChanged += OnMoneyChanged;
            UpdateView();
        }

        private void OnMoneyChanged(ResourceType arg1, int arg2)
        {
            UpdateView();
        }

        private void OnDisable()
        {
            _source.Data.OnUpgrade -= UpdateView;
            ResourceHandler.OnValueChanged -= OnMoneyChanged;
        }


        private void UpdateView()
        {
            if (_source.Data.AbleToUp)
            {
                _description.color = Color.black;
                _lvlText.color = new Color(0.1764706f, 0.7254902f, 0.7215686f);
                _icon.sprite = _activeIcon;
            }
            else
            {
                _icon.sprite = _inactiveIcon;
                var disabledColor = new Color(0.6745098f, 0.6745098f, 0.6745098f);
                _description.color = disabledColor;
                _lvlText.color = disabledColor;
            }
        }
    }
}