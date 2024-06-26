using UnityEngine;

public class SoundEffect : Audio
{
    public static int Volume
    {
        get
        {
            return PlayerPrefs.GetInt(soundEffectVolumePrefName, defaultVolumeValue);
        }
        set
        {
            PlayerPrefs.SetInt(soundEffectVolumePrefName, value);
        }
    }

    private static string soundEffectVolumePrefName = "soundEffectVolume";

    protected override void Awake()
    {
        base.Awake();

        audioSource.volume = Volume;
    }
}
