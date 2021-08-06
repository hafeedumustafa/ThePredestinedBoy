using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class changingPostProcessingValues : MonoBehaviour
{

    public Volume volume;
    public Slider slider1;
    public Slider slider2;
    private Vignette vignette;
    private Bloom bloom;
    
    void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<Bloom>(out bloom);

        vignette.intensity.value = SaveManager.instance.activeSave.dungeonVignette;
        bloom.intensity.value = SaveManager.instance.activeSave.dungeonBloom;

        
        slider1.value = SaveManager.instance.activeSave.dungeonVignette;
        slider2.value = SaveManager.instance.activeSave.dungeonBloom;
    }

    public void ChangeVignetteValue(float val) {
        vignette.intensity.value = val;
        SaveManager.instance.activeSave.dungeonVignette = val;
    }

    public void SetVignetteDefault(float defaultValue) {
        slider1.value = defaultValue;
        SaveManager.instance.activeSave.dungeonVignette = defaultValue;
    }


    public void ChangeBloomValue(float val) {
        bloom.intensity.value = val;
        SaveManager.instance.activeSave.dungeonBloom = val;
    }

    public void SetBloomDefault(float defaultValue) {
        slider2.value = defaultValue;
        SaveManager.instance.activeSave.dungeonBloom = defaultValue;
    }
}