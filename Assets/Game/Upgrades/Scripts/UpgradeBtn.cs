using Game.Scripts.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrades.Scripts
{
    public class UpgradeBtn : MonoBehaviour
    {
        [SerializeField] private UpgradeSource _source;

        [SerializeField] private Sprite _activeBtnSprite;
        [SerializeField] private Sprite _inactiveBtnSprite;

        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;


        [Header("Levels Display"), SerializeField]
        private TextMeshProUGUI _lvlText;

        [SerializeField] private UpgradeProgress _progress;

        public Button Button => _button;

        public UpgradeSource Source
        {
            get => _source;
            set
            {
                _source = value;
                UpdateDisplay();
                CheckBtn();
            }
        }

        public void DisableBtn()
        {
            _button.interactable = false;
            _buttonImage.sprite = _inactiveBtnSprite;
        }

        private void UpdateDisplay()
        {
            _priceText.text = !_source.Data.HasNextLvl ? "MAX" : _source.Data.NextLvlPrice.ToString();
            if (_lvlText) _lvlText.text = $"LVL {_source.Data.CurrentLvl + 1}";
            if (_progress) _progress.UpdateProgress(_source.Data);
        }

        private void CheckBtn()
        {
            SwitchBtn(_source.Data.CurrencyType, ResourceHandler.GetResourceCount(_source.Data.CurrencyType));
        }

        private void SwitchBtn(ResourceType type, int resourceCount)
        {
            _button.interactable = _source.Data.AbleToUp;
            _buttonImage.sprite = _button.interactable ? _activeBtnSprite : _inactiveBtnSprite;

            UpdateDisplay();
        }

        private void LvlUp()
        {
            if (!ResourceHandler.TrySubtractResource(_source.Data.CurrencyType, _source.Data.NextLvlPrice)) return;
            _source.Data.LvlUp();
        }

        private void OnEnable()
        {
            ResourceHandler.OnValueChanged += SwitchBtn;
            _source.Data.OnUpgrade += UpdateDisplay;
            _source.Data.OnUpgrade += CheckBtn;
            CheckBtn();
            UpdateDisplay();
        }

        private void OnDisable()
        {
            ResourceHandler.OnValueChanged -= SwitchBtn;
            _source.Data.OnUpgrade -= UpdateDisplay;
            _source.Data.OnUpgrade -= CheckBtn;
        }

        private void Awake()
        {
            _button.onClick.AddListener(LvlUp);
        }
    }
}