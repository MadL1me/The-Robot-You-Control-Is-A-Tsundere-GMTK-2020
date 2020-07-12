using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class OutOfControlTextSetter : MonoBehaviour
    {
        [SerializeField] private Text _mainText;

        public void SetText(GlitchType glitchType)
        {
            var text = "";
            switch (glitchType)
            {
                case GlitchType.None: text = "None"; break;
                case GlitchType.RandomReload: text = "Reload"; break;
                case GlitchType.RandomWalkLeft: text = "Left walk"; break;
                case GlitchType.RandomWalkForward: text = "Forward walk"; break;
                case GlitchType.RandomWalkBack: text = "Back walk"; break;
                case GlitchType.RandomWalkRight: text = "Right walk"; break;
            }

            _mainText.text = text;
        }
    }
}