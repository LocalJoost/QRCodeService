using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MRTKExtensions.QRCodes
{
    public class QRPoseTrackController : MonoBehaviour
    {
        [SerializeField]
        private BaseTrackerController trackerController;
        
        private AudioSource audioSource;

        private Transform childObj;

        private void Start()
        {
            audioSource = GetComponentInChildren<AudioSource>(true);
            childObj = transform.GetChild(0);
            childObj.gameObject.SetActive(false);
            trackerController.PositionSet.AddListener(PoseFound);
        }

        private void PoseFound(Pose pose)
        {
            childObj.SetPositionAndRotation(pose.position, pose.rotation);
            childObj.gameObject.SetActive(true);
            Task.Run(PlaySound);
        }

        private async Task PlaySound()
        {
            await Task.Yield();
            if(audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

        public void Reset()
        {
            trackerController.ResetTracking();
            childObj.gameObject.SetActive(false);
        }
    }
}