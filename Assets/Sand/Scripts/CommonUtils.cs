using UnityEngine;
public static class CommonUtils
{
    public static Vector3 ScreenToWorldPoint(Vector3 screenPoint, Camera camera = null)
    {
        Camera cam = camera ?? Camera.main;

        Ray ray = cam.ScreenPointToRay(screenPoint);

        float mapZ = 0f;
        float distance = (mapZ - ray.origin.z) / ray.direction.z;

        Vector3 worldPoint = ray.origin + ray.direction * distance;

        return worldPoint;
    }
}