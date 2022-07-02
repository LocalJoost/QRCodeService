using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTKExtensions.Visualization
{
    public class HandVisualizationController : MonoBehaviour
    {
        [EnumFlags]
        [SerializeField]
        private VisualizationEnvironment environment = VisualizationEnvironment.Editor;
        
        private void Start()
        {
            var hands = GetComponentsInChildren<HandVisualizer>();
#if UNITY_EDITOR
            SetHandDisplayForEnvironment(hands, VisualizationEnvironment.Editor);
#else
            SetHandDisplayForEnvironment(hands, VisualizationEnvironment.RunTime);
#endif
        }
        
        private void SetHandDisplayForEnvironment(HandVisualizer[] hands, 
            VisualizationEnvironment requestedEnvironment)
        {
            for(var i = 0; i < hands.Length; i++)
            {
                hands[i].enabled = (environment & requestedEnvironment) != 0;
            }
        }
    }
}