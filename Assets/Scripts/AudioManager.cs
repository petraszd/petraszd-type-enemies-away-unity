using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance = null;

  public AudioSource musicSource;
  public AudioSource fxSource;

  public AudioClip[] shootClips;
  public AudioClip killClip;

  public float pitchRandomness = 0.2f;
  private float originalFXPitch;

  void Awake()
  {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  public static void PlayKill()
  {
    if (instance == null) {
      return;
    }
    instance.InstancePlayKill();
  }

  public static void PlayShoot()
  {
    if (instance == null) {
      return;
    }
    instance.InstancePlayShoot();
  }

  void Start()
  {
    originalFXPitch = fxSource.pitch;
  }

  void InstancePlayKill()
  {
    PlayFxClip(killClip);
  }

  void InstancePlayShoot()
  {
    AudioClip shootClip = shootClips[Random.Range(0, shootClips.Length)];
    PlayFxClip(shootClip);
  }

  private void PlayFxClip(AudioClip clip)
  {
    fxSource.pitch = originalFXPitch + Random.Range(-pitchRandomness, pitchRandomness);
    fxSource.PlayOneShot(clip);
  }
}
