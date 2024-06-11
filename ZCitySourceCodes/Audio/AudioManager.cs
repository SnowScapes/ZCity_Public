using UnityEngine;
using UnityEngine.Audio;

public enum PlayerSfxSound
{
    FootStep = 0,
    Attack,
    Die,
    Hit,
    CollectItem,
    CollectWood,
    Collectstone,
    CollectBuilding
}

public enum MonsterSfxSound
{
    Attack = 0,
    Die,
    Hit
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("AudioSource")]
    public AudioSource backgroundSource;
    public AudioSource sfxSource;
    [Header("AudioClip")]
    public AudioClip backgroundClip;
    public AudioClip[] playerSfxClips;
    public AudioClip[] playerHitSfxClips;
    public AudioClip[] monsterSfxClips;
    public AudioClip[] monsterAttackSfxClips;
    [Header("AudioMixer")]
    public AudioMixerGroup backgroundMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    private void Start()
    {
        if (backgroundSource == null)
        {
            backgroundSource = gameObject.AddComponent<AudioSource>();
            backgroundSource.outputAudioMixerGroup = backgroundMixerGroup;
            backgroundSource.loop = true;
            backgroundSource.clip = backgroundClip;
            backgroundSource.Play();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    public void PlayPlayerSFX(PlayerSfxSound sfxSound)
    {
        if (sfxSource != null)
        {
            if (sfxSound == PlayerSfxSound.Hit)
            {
                int randomIndex = Random.Range(0, playerHitSfxClips.Length);
                sfxSource.PlayOneShot(playerHitSfxClips[randomIndex]);
            }
            else
            {
                sfxSource.PlayOneShot(playerSfxClips[(int)sfxSound]);
            }
        }
    }
    
    public void PlayMonsterSFX(MonsterSfxSound sfxSound)
    {
        if (sfxSource != null)
        {
            if (sfxSound == MonsterSfxSound.Attack)
            {
                int randomIndex = Random.Range(0, monsterAttackSfxClips.Length);
                sfxSource.PlayOneShot(monsterAttackSfxClips[randomIndex]);
            }
            else
            {
                sfxSource.PlayOneShot(monsterSfxClips[(int)sfxSound]);
            }
        }
    }
}
