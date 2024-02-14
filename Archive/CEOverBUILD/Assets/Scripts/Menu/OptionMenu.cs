using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MapValues;

public class OptionMenu : MonoBehaviour {

    public float maxSensitivity;
    public float minSensitivity;
    public GameObject optionsCanvas;
    public GameObject pauseCanvas;
    GameObject player;

    //  Resolution[] resolutions;
    //  public Dropdown resolutionsDD;

    public Toggle[] resolutionToggles;
    public int[] screenWidth;
    int screenResIndex;

    vp_FPInput input;
    Slider ySlide;
    Slider xSlide;

    // Use this for initialization
    void Start ()
    {
        screenResIndex = PlayerPrefs.GetInt("Screen Res Index");
        bool isFullScreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

        for (int i = 0; i < resolutionToggles.Length; ++i)
        {
            resolutionToggles[i].isOn = i == screenResIndex;
        }
        FullScreen(isFullScreen);

        /*    resolutions = Screen.resolutions;
            resolutionsDD.ClearOptions();
            List<string> options = new List<string>();

            int currentResIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = 1;
                }
            }
            resolutionsDD.AddOptions(options);
            resolutionsDD.value = currentResIndex;
            resolutionsDD.RefreshShownValue();
            */

        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<vp_FPInput>();

        optionsCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(10).gameObject;
        pauseCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(9).gameObject;

        ySlide = optionsCanvas.transform.GetChild(4).gameObject.GetComponent<Slider>();
        xSlide = optionsCanvas.transform.GetChild(5).gameObject.GetComponent<Slider>();

        ySlide.value = 0.5f;
        xSlide.value = 0.5f;
        ySlide.onValueChanged.AddListener(delegate { YValueChangeCheck(); });
        xSlide.onValueChanged.AddListener(delegate { XValueChangeCheck(); });

        input.MouseLookSensitivity = new Vector2(MapValuesExtension.Map(xSlide.value, 0, 1, minSensitivity, maxSensitivity), MapValuesExtension.Map(ySlide.value, 0, 1, minSensitivity, maxSensitivity));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            optionsCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void YValueChangeCheck()
    {
        input.MouseLookSensitivity = new Vector2(input.MouseLookSensitivity.x, MapValuesExtension.Map(ySlide.value, 0, 1, minSensitivity, maxSensitivity));
    }

    public void XValueChangeCheck()
    {
        input.MouseLookSensitivity = new Vector2(MapValuesExtension.Map(xSlide.value, 0, 1, minSensitivity, maxSensitivity), input.MouseLookSensitivity.y);
    }

    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            screenResIndex = i;
            float ratio = 16 / 9f;
            Screen.SetResolution(screenWidth[i], (int)(screenWidth[i] / ratio), false);
            PlayerPrefs.SetInt("Screen Res Index", screenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void FullScreen(bool isFullScreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullScreen;
        }

        if (!isFullScreen)
        {
            Resolution[] availResolutions = Screen.resolutions;
            Resolution maxRes = availResolutions[availResolutions.Length - 1];
            Screen.SetResolution(maxRes.width, maxRes.height, true);
        }
        else
        {
            ScreenResolution(screenResIndex);
        }

        PlayerPrefs.SetInt("Fullscreen", ((isFullScreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    /*   public void SetResolution(int resolutionIndex)
       {
           Resolution resolution = resolutions[resolutionIndex];
           Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
       }
       */

    public void Back()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0.00001f;
    }
}
