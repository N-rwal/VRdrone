using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is imperative to be executed first to set up the ndk, notice the 'void Awake()'

public class FFmpegKitInitializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Disabling SIGXCPU handling for FFmpegKit.");

        try
        {
            AndroidJavaClass configClass = new AndroidJavaClass("com.arthenica.ffmpegkit.FFmpegKitConfig");
            AndroidJavaObject paramVal = new AndroidJavaClass("com.arthenica.ffmpegkit.Signal").GetStatic<AndroidJavaObject>("SIGXCPU");
            configClass.CallStatic("ignoreSignal", new object[] { paramVal });

            Debug.Log("SIGXCPU handling disabled successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error disabling SIGXCPU handling: " + ex.Message);
        }
    }
}
