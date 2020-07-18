using Core.Game;
using Core.Health;
using TowerDefense.Game;
using TowerDefense.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    /// <summary>
    /// UI to display the game over screen
    /// </summary>
    public class EndGameInfinite : MonoBehaviour
    {
        /// <summary>
        /// AudioClip to play when victorious
        /// </summary>
        public AudioClip endSound;

        /// <summary>
        /// AudioSource that plays the sound
        /// </summary>
        public AudioSource audioSource;

        public WaveManager waveManager;

        /// <summary>
        /// The containing panel of the End Game UI
        /// </summary>
        public Canvas endGameCanvas;

        /// <summary>
        /// Reference to the Text object that displays the result message
        /// </summary>
        public Text endGameMessageText;

        /// <summary>
        /// Panel that shows final star rating
        /// </summary>
        public ScorePanel scorePanel;

        /// <summary>
        /// Name of level select screen
        /// </summary>
        public string menuSceneName = "MainMenu";

        /// <summary>
        /// Background image
        /// </summary>
        public Image background;

        /// <summary>
        /// Color to set background
        /// </summary>
        public Color winBackgroundColor;

        /// <summary>
        /// Reference to the <see cref="LevelManager" />
        /// </summary>
        protected LevelManager m_LevelManager;

        private int numeroOleadas;

        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// Go back to the main menu scene
        /// </summary>
        public void GoToMainMenu()
        {
            SafelyUnsubscribe();
            SceneManager.LoadScene(menuSceneName);
        }

        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// Reloads the active scene
        /// </summary>
        public void RestartLevel()
        {
            SafelyUnsubscribe();
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        /// <summary>
        /// Hide the panel if it is active at the start.
        /// Subscribe to the <see cref="LevelManager" /> completed/failed events.
        /// </summary>
        protected void Start()
        {
            LazyLoad();
            endGameCanvas.enabled = false;

            m_LevelManager.levelFailed += Defeat;
        }

        /// <summary>
        /// Shows the end game screen
        /// </summary>
        protected void OpenEndGameScreen()
        {
            endGameCanvas.enabled = true;

            int score = CalculateScore();
            scorePanel.SetStars(score);

           // if (level != null)
          //  {
                if (Application.systemLanguage.Equals(SystemLanguage.Spanish))
                {
                    endGameMessageText.text = string.Format("HAS SUPERADO {0} OLEADAS", numeroOleadas);
                }
                else
                {
                    endGameMessageText.text = string.Format("YOU'VE DEFEAT {0} WAVES", numeroOleadas);
                }
                GameManager.instance.CompleteLevel("infiniteMode", score);
         //   }
         //   else
        //    {
         //       // If the level is not in LevelList, we should just use the name of the scene. This will not store the level's score.
       //         string levelName = SceneManager.GetActiveScene().name;
       //         endGameMessageText.text = string.Format("PATATA");
       //     }


            if (!HUD.GameUI.instanceExists)
            {
                return;
            }
            if (HUD.GameUI.instance.state == HUD.GameUI.State.Building)
            {
                HUD.GameUI.instance.CancelGhostPlacement();
            }
            HUD.GameUI.instance.GameOver();
        }

        /// <summary>
        /// Occurs when level is finish
        /// </summary>
        protected void Defeat()
        {
            OpenEndGameScreen();

            if ((endSound != null) && (audioSource != null))
            {
                audioSource.PlayOneShot(endSound);
            }
            background.color = winBackgroundColor;
        }

        /// <summary>
        /// Safely unsubscribes from <see cref="LevelManager" /> events.
        /// </summary>
        protected void OnDestroy()
        {
            SafelyUnsubscribe();
            if (HUD.GameUI.instanceExists)
            {
                HUD.GameUI.instance.Unpause();
            }
        }

        /// <summary>
        /// Ensure that <see cref="LevelManager" /> events are unsubscribed from when necessary
        /// </summary>
        protected void SafelyUnsubscribe()
        {
            LazyLoad();
            m_LevelManager.levelFailed -= Defeat;
        }

        /// <summary>
        /// Ensure <see cref="m_LevelManager" /> is not null
        /// </summary>
        protected void LazyLoad()
        {
            if ((m_LevelManager == null) && LevelManager.instanceExists)
            {
                m_LevelManager = LevelManager.instance;
            }
        }

        /// <summary>
        /// Take the final remaining health of all bases and rates them
        /// </summary>
        /// <param name="remainingHealth">the total remaining health of all home bases</param>
        /// <param name="maxHealth">the total maximum health of all home bases</param>
        /// <returns>0 to 3 depending on how much waves defeat</returns>
        protected int CalculateScore()
        {
            numeroOleadas = waveManager.WaveNumber;

            if (numeroOleadas >= 30)
            {
                return 3;
            }
            if (numeroOleadas >= 20 && numeroOleadas <= 29)
            {
                return 2;
            }
            if (numeroOleadas >= 10 && numeroOleadas <= 19)
            {
                return 1;
            }
            return 0;
        }
    }
}