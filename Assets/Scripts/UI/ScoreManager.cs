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
        float scoreLastFrame = 0;

        [HideInInspector]
        public float highScore;
        
        public float multiplier;
        float lastFrameMultiplier = 0;

        const string highScoreKey = "HighScore";

        public Text currentScoreText;
        ColourLoader scoreTextColour;

        public Text multiplierText;
        ColourLoader multiplierTextColour;

        public Text highScoreText;
        ColourLoader highScoreTextColour;

        // Set score texts and resets current score and multiplier
        void Start()
        {
            highScore = SaveDataManager<OrbitalSaveData>.data.highscore;

            currentScore = 0;
            multiplier = 0;

            currentScoreText.text = "Score: " + currentScore.ToString("F0");
            multiplierText.text = "COMBO: " + multiplier.ToString("F0");
            highScoreText.text = "HI: " + highScore.ToString("F0");

            scoreTextColour = currentScoreText.GetComponent<ColourLoader>();
            multiplierTextColour = multiplierText.GetComponent<ColourLoader>();
            highScoreTextColour = highScoreText.GetComponent<ColourLoader>();
        }

        // Updates each text whenever they change in game once the player gains a point
        void Update()
        {
            if (currentScore > scoreLastFrame)
            {
                currentScoreText.text = "S: " + currentScore.ToString("F0");
                scoreTextColour.FlashWhite();
                scoreLastFrame = currentScore;
            }

            // Activates multiplier text if multiplier is over 2
            if (multiplier >= 2 && lastFrameMultiplier < multiplier)
            {
                multiplierText.text = "COMBO X " + multiplier.ToString("F0");
                multiplierTextColour.FlashWhite();
            }
            else if (multiplier < 2)
            {
                multiplierText.text = "";
            }
            lastFrameMultiplier = multiplier;

            if (currentScore >= highScore)
            {
                highScoreText.text = "HI: " + highScore.ToString("F0");
                highScoreTextColour.FlashWhite();
                highScore = currentScore;
                SaveDataManager<OrbitalSaveData>.data.highscore = highScore;
                SaveDataManager<OrbitalSaveData>.SaveData();
            }
        }
    }
}