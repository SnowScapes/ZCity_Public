using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 camPos = new Vector3(-4, 15, -4);

    private Renderer obstacleRenderer;
    private Color transparentColor;
    private Vector3 rayDirection;

    private void Start()
    {
        transform.position = player.transform.position + camPos;
        rayDirection = player.transform.position - transform.position;
    }

    private void Update()
    {
        CameraRay();
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + camPos;
    }

    void SetColor(bool setTransparent)
    {
        if (setTransparent)
        {
            transparentColor = obstacleRenderer.material.color;
            transparentColor.a = 0.5f;
            obstacleRenderer.material.color = transparentColor;
        }
        else
        {
            transparentColor = obstacleRenderer.material.color;
            transparentColor.a = 1f;
            obstacleRenderer.material.color = transparentColor;
        }
    }

    void CameraRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDirection, out hit) && hit.collider.gameObject.layer != 10 && hit.collider.gameObject.layer != 11)
        {
            if (obstacleRenderer != hit.collider.gameObject.GetComponent<Renderer>())
            {
                if (obstacleRenderer == null)
                {
                    obstacleRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    SetColor(true);
                }
                SetColor(false);
                obstacleRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                SetColor(true);
            }
        }
        else
        {
            if (obstacleRenderer != null)
            {
                SetColor(false);
                obstacleRenderer = null;
            }
        }
    }
}
