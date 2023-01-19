using Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdsReward
{
    public class ParameterRewardPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _multiplierReward;
        [SerializeField] private Image _speed;
        [SerializeField] private Image _size;
        [SerializeField] private Image _income;

        public void DisplayReward(Parameter parameter, int count)
        {
            Prepare();

            switch (parameter.Type)
            {
                case ParameterType.Speed:
                    Display(_speed, count);
                    break;
                case ParameterType.Size:
                    Display(_size, count);
                    break;
                case ParameterType.Income:
                    Display(_income, count);
                    break;
            }

            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }    

        private void Prepare()
        {
            _speed.gameObject.SetActive(false);
            _size.gameObject.SetActive(false);
            _income.gameObject.SetActive(false);
        }

        private void Display(Image icon, int count)
        {
            icon.gameObject.SetActive(true);
            _multiplierReward.text = $"+{count}";
        }    
    }
}