using System;
using UnityEngine;
using Microsoft.MixedReality.OpenXR;

namespace MRTKExtensions.QRCodes
{
    public class OpenXRSpatialGraphCoordinateSystemSetter : SpatialGraphCoordinateSystemSetter
    {
        protected override void UpdateLocation(Guid spatialGraphNodeId, float physicalSideLength)
        {
            var node = spatialGraphNodeId != Guid.Empty ? SpatialGraphNode.FromStaticNodeId(spatialGraphNodeId) : null;
            if (node != null && node.TryLocate(FrameTime.OnUpdate, out Pose pose))
            {
                if (Camera.main.transform.parent != null)
                {
                    pose = pose.GetTransformedBy(Camera.main.transform.parent);
                }

                MovePoseToCenter(pose,physicalSideLength);
            }
            else
            {
                PositionAcquisitionFailed?.Invoke(this, null);
            }
        }
    }
}