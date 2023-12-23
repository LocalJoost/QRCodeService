using MRTKExtensions.QRCodes;
using UnityEngine;

public class JetController : MonoBehaviour
{
    [SerializeField]
    private BaseTrackerController trackerController;

    private Transform childObj;

    private void Start()
    {
        childObj = transform.GetChild(0);
        childObj.gameObject.SetActive(false);
        trackerController.PositionSet.AddListener(PoseFound);
    }

    private void PoseFound(Pose pose)
    {
        childObj.SetPositionAndRotation(pose.position, pose.rotation);
        childObj.gameObject.SetActive(true);
    }
}