using MixedReality.Toolkit;
using UnityEngine;

namespace MRTKExtensions.Animation
{
    public class LookatHorizontalDirection : MonoBehaviour
    {
        public float RotateAngle = 180f;

        void Update()
        {
            Vector3 targetPosition = new Vector3(Camera.main.transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z);
            this.transform.LookAt(targetPosition);
            gameObject.transform.Rotate(Vector3.up, RotateAngle);
        }
    }
}
