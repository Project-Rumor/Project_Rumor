using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DeveloperManager : MonoBehaviour
{
    VideoPlayer video;

    void Start()
    {
        video = this.GetComponent<VideoPlayer>();

        StartCoroutine("VideoCo");
    }

    IEnumerator VideoCo()
    {
        yield return new WaitForSeconds(1.5f);

        video.Play();

        yield return new WaitForSeconds(1f);
        
        while (true)
        {
            yield return null;
            if (!video.isPlaying)
            {
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Scene_00_Title");
                yield break;
            }
        }
    }
}
