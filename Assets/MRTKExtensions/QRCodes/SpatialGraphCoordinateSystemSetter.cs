using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MRTKExtensions.QRCodes
{
    public abstract class SpatialGraphCoordinateSystemSetter : MonoBehaviour
    {
        public EventHandler<Pose> PositionAcquired;
        public EventHandler PositionAcquisitionFailed; 
        private Pose? lastPose;

        private readonly Queue<Tuple<Guid, float>> locationIdSizes = new();

        public void SetLocationIdSize(Guid spatialGraphNodeId, float physicalSideLength)
        {
            locationIdSizes.Enqueue(new Tuple<Guid, float>(spatialGraphNodeId, physicalSideLength));
        }

        private void Update()
        {
            if (locationIdSizes.Any())
            {
                var locationIdSize = locationIdSizes.Dequeue();
                UpdateLocation(locationIdSize.Item1, locationIdSize.Item2);
            }
        }

        protected abstract void UpdateLocation(Guid spatialGraphNodeId, float physicalSideLength);

        protected void MovePoseToCenter(Pose pose,float physicalSideLength)
        {
            // Rotate 90 degrees 'forward' over 'right' so 'up' is pointing straight up from the QR code
            pose.rotation *= Quaternion.Euler(90, 0, 0);

            // Move the anchor point to the *center* of the QR code
            var deltaToCenter = physicalSideLength * 0.5f;
            pose.position += (pose.rotation * (deltaToCenter * Vector3.right) -
                              pose.rotation * (deltaToCenter * Vector3.forward));
            CheckPosition(pose);
        }


        private void CheckPosition(Pose pose)
        {
            if (lastPose == null)
            {
                lastPose = pose;
                return;
            }

            if (Mathf.Abs(Quaternion.Dot(lastPose.Value.rotation, pose.rotation)) > 0.99f &&
                Vector3.Distance(lastPose.Value.position, pose.position) < 0.5f)
            {
                locationIdSizes.Clear();
                lastPose = null;
                gameObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
                PositionAcquired?.Invoke(this, pose);
            }
            else
            {
                lastPose = pose;
            }
        }
    }
}