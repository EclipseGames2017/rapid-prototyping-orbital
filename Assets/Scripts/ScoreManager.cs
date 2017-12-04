using FallingSloth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class ScoreManager : MonoBehaviour
    {
        [HideInInspector]
        public float currentScore;

        [HideInInspector]
        public float highScore;
        
        public float multiplier;

        const string highScoreKey = "HighScore";

        public Text currentScoreText;
        public Text multiplierText;
        public Text highScoreText;

        // Set score texts and resets current score and multiplier
        void Start()
        {
            highScore = SaveDataManager<OrbitalSaveData>.data.highscore;

            currentScore = 0;
            multiplier = 1;

            currentScoreText.text = "Score: " + currentScore.ToString("F0");
            multiplierText.text = "Multiplier: " + multiplier.ToString("F0");
            highScoreText.text = "High Score: " + highScore.ToString("F0");
        }

        // Updates each text whenever they change in game once the player gains a point
        void Update()
        {
            if (currentScore >= 0.1f)
            {
                currentScoreText.text = "S: " + currentScore.ToString("F0");
                multiplierText.text = "M: " + multiplier.ToString("F0");
                highScoreText.text = "HS: " + highScore.ToString("F0");
            }


            if (currentScore >= highScore)
            {
                highScore = currentScore;
                SaveDataManager<OrbitalSaveData>.data.highscore = highScore;
                SaveDataManager<OrbitalSaveData>.SaveData();
            }
        }
    }
}