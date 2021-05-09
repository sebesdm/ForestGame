using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    public float skyboxRotationRate;
    public AudioClip mushroomCollectSound;
    public int mushroomPickupGoal;
    public string NextLevel;

    void Start()
    {
        Physics.gravity *= 5;

        audioSources = GetComponents<AudioSource>().ToList();
        mainAudioSource = audioSources[0];
        AudioSource levelEffectsAmbientAudioSource = audioSources[1];
        AudioSource levelEffectsOneShotAudioSource = audioSources[2];
        AudioSource playerAudioSource = audioSources[3];




        LevelEffects = GameObject.Find("LevelEffects").GetComponent<ILevelEffects>();
        LevelEffects.SetAudioSources(levelEffectsAmbientAudioSource, levelEffectsOneShotAudioSource);
        LevelEffects.SetDirectionalLight(GameObject.Find("Directional Light").GetComponent<Light>());


        playerController = GetComponentInChildren<PlayerController>();
        playerController.SetAudioSource(playerAudioSource);



        mushroomBarUI = GameObject.Find("MushroomBarCounter").GetComponentInChildren<Slider>();
        levelSpawner = GetComponentInChildren<LevelSpawner>();

        playerController.OnPlayerMushroomPickup += Player_OnPlayerMushroomPickup;

        mushroomBarUI.value = 0;
        mushroomBarUI.maxValue = mushroomPickupGoal;

        levelFader = GameObject.Find("LevelFader").GetComponent<LevelFader>();
        levelFader.FadeIn(audioSources, () => { });


        LevelEffects.StartAmbient();
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotationRate);
        Cheats();
    }

    private void Player_OnPlayerMushroomPickup(object sender, MushroomPickupEventArgs e)
    {
        Destroy(e.PickedUpMushroom.gameObject);
        levelSpawner.SpawnNewMushroom();
        mushroomBarUI.value += 1;
        mainAudioSource.PlayOneShot(mushroomCollectSound);

        if(mushroomBarUI.value == mushroomPickupGoal)
        {
            levelFader.FadeOut(audioSources, () => SceneManager.LoadScene(NextLevel));
        }
    }

    private void Cheats()
    {
        if(Input.GetKeyDown(KeyCode.M) && Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.I))
        {
            levelFader.FadeOut(audioSources, () => SceneManager.LoadScene(NextLevel));
        }
    }

    private LevelSpawner levelSpawner;
    private PlayerController playerController;
    private Slider mushroomBarUI;
    private List<AudioSource> audioSources;
    private AudioSource mainAudioSource;
    private LevelFader levelFader;
    private ILevelEffects LevelEffects;
}
