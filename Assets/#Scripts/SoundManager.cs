using UnityEngine;

#region Enum & Structs
public enum S2DMusic
{
    Gameplay,
    MainMenu
}

public enum S2DSounds
{
    Meow,
}
[System.Serializable]
public struct Sound2D
{
    public AudioClip clip;
    public S2DSounds type;
}
[System.Serializable]
public struct Music2D
{
    public AudioClip clip;
    public S2DMusic type;
} 
#endregion
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    #region Audio Clips
    [Header("Audio Clips")]
    
    [Space(10)]
    [Header("2D SFX")]
    [SerializeField] private Sound2D[] Sounds2D;

    [Space(10)]
    [Header("2D Musics")]
    [SerializeField] private Music2D[] Music2D;
    #endregion

    #region Volumes
    private float _musicVolume = 1f;
    public float musicVolume { get => _musicVolume; set { value = _musicVolume; SetMusicVolume(value); } }

    private float _soundVolume = 1f;
    public float soundVolume { get => _soundVolume; set { value = _soundVolume; SetSFXVolume(value); } }
    #endregion

    #region Audio Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sourceMusic;
    [SerializeField] private AudioSource[] source2D;

    int index2D = 0;
    #endregion
    private void Awake()
    {
        instance = this;
    }

    #region Play Funcs
    public void PlayMusic(S2DMusic clip)
    {
        sourceMusic.Stop();
        sourceMusic.clip = Music2D[(int)clip].clip;
        sourceMusic.Play();
    }
    public void PlaySFX2D(S2DSounds clip)
    {
        source2D[index2D].PlayOneShot(Sounds2D[(int)clip].clip);
        index2D = (index2D >= Sounds2D.Length - 1) ? index2D + 1 : 0;
    }
    #endregion

    #region Stop Funcs
    public void StopMusic()
    {
        sourceMusic.Stop();
    }
    public void StopAllSFX()
    {
        for (int i = 0; i < source2D.Length; i++)
        {
            source2D[i].Stop();
        }
        index2D = 0;
    }
    #endregion

    #region Volume Settings
    private void SetSFXVolume(float volume)
    {
        for (int i = 0; i < source2D.Length; i++)
        {
            source2D[i].volume = volume;
        }
    }
    private void SetMusicVolume(float volume)
    {
        sourceMusic.volume = volume;
    } 
    #endregion
}
