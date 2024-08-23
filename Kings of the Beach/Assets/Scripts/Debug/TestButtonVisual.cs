using UnityEngine;
using UnityEngine.UI;

namespace KotB.Testing
{
    public class TestButtonVisual : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;

        private Color activeColor = new Color(0.2775892f, 0.8598382f, 0.221333f);
        private Color inactiveColor = Color.white;

        private Image img;

        private void Awake() {
            img = GetComponent<Image>();
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            inputReader.testEvent += OnTestPerformed;
            inputReader.testCanceledEvent += OnTestCanceled;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            inputReader.testEvent -= OnTestPerformed;
            inputReader.testCanceledEvent -= OnTestCanceled;
        }

        //---- EVENT LISTENERS ----

        private void OnTestPerformed()
        {
            img.color = activeColor;
        }

        private void OnTestCanceled()
        {
            img.color = inactiveColor;
        }
    }
}
