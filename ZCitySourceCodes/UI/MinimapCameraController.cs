using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0, 200, 0);

    void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(90f, 45f, 0f);
        transform.Find("PlayerPoint").transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}