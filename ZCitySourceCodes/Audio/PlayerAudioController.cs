using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public void Step()
    {
        AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.FootStep);
    }
    
    public void BatSwing()
    {
        AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.Attack);
    }
}
