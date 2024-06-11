using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Renderer HeadRenderer;
    public Renderer BodyRenderer;

    private Color visibleColor = new Color(1, 1, 1, 1);
    private Color invisibleColor = new Color(0, 0, 0, 0.2f);
    private float changeTime = 1f;
    public bool InSight { get; set; } = false;

    private void Update()
    {
        SetColor();
    }

    private void SetColor()
    {
        if (InSight && HeadRenderer.material.color != visibleColor)
        {
            Color curColor = HeadRenderer.material.color;
            
            float rgb = Mathf.Clamp01(curColor.r + (Time.deltaTime / changeTime));
            float a = Mathf.Clamp(curColor.a + (Time.deltaTime / changeTime), 0.2f, 1f);
            curColor = new Color(rgb, rgb, rgb, a);
            HeadRenderer.material.color = curColor;
            BodyRenderer.material.color = curColor;
        }
        else if (!InSight && HeadRenderer.material.color != invisibleColor)
        {
            Color curColor = HeadRenderer.material.color;
        
            float rgb = Mathf.Clamp01(curColor.r - (Time.deltaTime / changeTime));
            float a = Mathf.Clamp(curColor.a - (Time.deltaTime / changeTime), 0.2f, 1f);
            curColor = new Color(rgb, rgb, rgb, a);
            HeadRenderer.material.color = curColor;
            BodyRenderer.material.color = curColor;
        }
    }
}
