using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown ResDropdown;

    void Start()
    {
        resolutions = Screen.resolutions;

        ResDropdown.ClearOptions();
        List<string> Options = new List<string>();
        int CurrentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            Options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }
        ResDropdown.AddOptions(Options);
        ResDropdown.value = CurrentResolutionIndex;
        ResDropdown.RefreshShownValue();
    }

    public void SetResolution(int ResIndex)
    {
        Resolution resolution = resolutions[ResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void PlayClassic()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application quit!");
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
