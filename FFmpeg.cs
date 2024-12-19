using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// the actual FFmpeg player, this displays the stream on an object's mesh render

public class RTSPVideoPlayer : MonoBehaviour
{
    // just some QoL features to be configurable from the Unity editor:
    [Header("RTSP Settings")]
    [Tooltip("The RTSP URL of the video stream.")]
    public string rtspUrl = "rtsp://192.168.1.10:8554/live?resolution=640x320";

    [Header("Video Settings")]
    [Tooltip("The width of the video stream in pixels.")]
    public int videoWidth = 640;
    [Tooltip("The height of the video stream in pixels.")]
    public int videoHeight = 320;

    [Header("Target Renderer")]
    [Tooltip("The renderer where the video will be displayed.")]
    public Renderer targetRenderer;

    private Texture2D videoTexture;
    private byte[] videoBuffer;

    void Start()
    {
        // Initialize video texture and buffer
        videoTexture = new Texture2D(videoWidth, videoHeight, TextureFormat.RGB24, false);
        targetRenderer.material.mainTexture = videoTexture;
        videoBuffer = new byte[videoWidth * videoHeight * 3];

        // start the video stream
        StartRTSPStream();
        StartCoroutine(StreamVideoCoroutine());
    }

    public void StartRTSPStream()
    {
        string command = $"-fflags nobuffer -i {rtspUrl} -vf scale={videoWidth}:{videoHeight} -pix_fmt rgb24 -f rawvideo -";
        Debug.Log($"Starting RTSP stream with command: {command}");

        ExecuteFFmpegCommand(command);
    }

    private void ExecuteFFmpegCommand(string command)
    {
        try
        {
            AndroidJavaClass ffmpegKitClass = new AndroidJavaClass("com.arthenica.ffmpegkit.FFmpegKit");
            AndroidJavaObject session = ffmpegKitClass.CallStatic<AndroidJavaObject>("execute", new object[] { command });

            Debug.Log("RTSP stream started.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error executing FFmpeg command: {ex.Message}");
        }
    }

    IEnumerator StreamVideoCoroutine()
    {
        while (true)
        {
            // simulate reading video data while waiting for the feed
            for (int i = 0; i < videoBuffer.Length; i++)
            {
                videoBuffer[i] = (byte)Random.Range(0, 255);
            }

            // update the object texture
            videoTexture.LoadRawTextureData(videoBuffer);
            videoTexture.Apply();

            yield return null;
        }
    }
}
