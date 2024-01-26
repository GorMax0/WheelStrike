using UnityEngine;
using Lean.Localization;

namespace UI.Views.Finish
{
    public class FinishViewTopLabelSetter : MonoBehaviour
    {
        [SerializeField] private LeanToken _topLabelEn;
        [SerializeField] private LeanToken _topLabelRu;
        [SerializeField] private LeanToken _topLabelTr;

        private const string GoodStartEn = "Good start!";
        private const string AlreadyHalfwayThereEn = "Already halfway there!";
        private const string AlmostThereEn = "Almost there!";
        private const string NextLevelIsOpenEn = "The next level is open!";

        private const string GoodStartRu = "Хорошее начало!";
        private const string AlreadyHalfwayThereRu = "Уже на полпути!";
        private const string AlmostThereRu = "Почти на месте!";
        private const string NextLevelIsOpenRu = "Следующий уровень открыт!";

        private const string GoodStartTr = "İyi bir başlangıç!";
        private const string AlreadyHalfwayThereTr = "Yolun yarısına geldik!";
        private const string AlmostThereTr = "Neredeyse geldik!";
        private const string NextLevelIsOpenTr = "Bir sonraki seviye açık!";

        public void SelectLabel(int distanceTraveled, int lengthRoad)
        {
            if (distanceTraveled >= lengthRoad)
            {
                _topLabelEn.SetValue(NextLevelIsOpenEn);
                _topLabelRu.SetValue(NextLevelIsOpenRu);
                _topLabelTr.SetValue(NextLevelIsOpenTr);
            }
            else if (distanceTraveled > lengthRoad * 0.75f)
            {
                _topLabelEn.SetValue(AlmostThereEn);
                _topLabelRu.SetValue(AlmostThereRu);
                _topLabelTr.SetValue(AlmostThereTr);
            }
            else if (distanceTraveled > lengthRoad * 0.5f)
            {
                _topLabelEn.SetValue(AlreadyHalfwayThereEn);
                _topLabelRu.SetValue(AlreadyHalfwayThereRu);
                _topLabelTr.SetValue(AlreadyHalfwayThereTr);
            }
            else
            {
                _topLabelEn.SetValue(GoodStartEn);
                _topLabelRu.SetValue(GoodStartRu);
                _topLabelTr.SetValue(GoodStartTr);
            }
        }
    }
}