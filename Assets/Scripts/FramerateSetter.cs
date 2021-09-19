using UnityEngine;
using System.Collections;

public class FramerateSetter : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
  void Awake()
  {
    Application.targetFrameRate = 60;
  }
#endif
}
