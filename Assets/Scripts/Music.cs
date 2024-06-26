using UnityEngine;

public class Music : Audio
{
    public static int Volume
    {
        get
        {
            return PlayerPrefs.GetInt(musicVolumePrefName, defaultVolumeValue);
        }
        set
        {
            PlayerPrefs.SetInt(musicVolumePrefName, value);
        }
    }

    private static string musicVolumePrefName = "musicVolume";

    protected override void Awake()
    {
        base.Awake();

        audioSource.volume = Volume;
    }
}
