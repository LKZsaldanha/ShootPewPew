using UnityEngine;
using UnityEngine.UI;

public enum PlayerHUDState { playing, available, selection, gameOver };

public class PlayerHUD : MonoBehaviour
{
    public PlayerHUDState playerHUDState;

    [Header("Attributes")]
    public int characterID;
    public int playerScore = 0;
    public int gunRank = 0;
    public int playerLives = 5;

    [Header ("HUD States Objects")]
    [SerializeField] private GameObject playingHUD;
    [SerializeField] private GameObject availableHUD;
    [SerializeField] private GameObject gameOverHUD;
    [SerializeField] private GameObject selectionHUD;


    [Header("Linked GameObjects")]
    [SerializeField] private Image portraitObject;
    [SerializeField] private Image gunIconObject;
    [SerializeField] private GameObject[] visualGunRanks;

    [Header("HUD Texts")]
    [SerializeField] private Text scoreValueText;
    [SerializeField] private Text livesValueText;



    [Header("Sprites to use")]
    [SerializeField] private Sprite[] characterPortraitImages;
    [SerializeField] private Sprite[] characterGunIcons;

    private void Start()
    {
        SwitchPlayerHUDState(playerHUDState);
    }

    //altera o estado da HUD 
    public void SwitchPlayerHUDState(PlayerHUDState newState)
    {
        playerHUDState = newState;

        switch (playerHUDState) {
            case PlayerHUDState.available:
                playingHUD.SetActive(false);
                availableHUD.SetActive(true);
                gameOverHUD.SetActive(false);
                selectionHUD.SetActive(false);

                break;

            case PlayerHUDState.playing:
                playingHUD.SetActive(true);
                availableHUD.SetActive(false);
                gameOverHUD.SetActive(false);
                selectionHUD.SetActive(false);

                SetPlayingHUD();

                break;

            case PlayerHUDState.gameOver:
                playingHUD.SetActive(false);
                availableHUD.SetActive(false);
                gameOverHUD.SetActive(true);
                selectionHUD.SetActive(false);

                break;

            case PlayerHUDState.selection:
                playingHUD.SetActive(false);
                availableHUD.SetActive(false);
                gameOverHUD.SetActive(false);
                selectionHUD.SetActive(true);

                break;
        }


    }

    #region SetupFunctions

    //Configura a HUD "playin"
    private void SetPlayingHUD()
    {
        UpdateCharacterHUDVisual();
        UpdateHUDGunRank(gunRank);
        UpdateHUDLives(0);
    }

    public void SetNewCharacterID(int newID)
    {
        characterID = newID;
    }

    //Atualiza visualmente as sprites do char e do icone da arma, baseado no ID de personagem
    public void UpdateCharacterHUDVisual()
    {
        portraitObject.sprite = characterPortraitImages[characterID];
        gunIconObject.sprite = characterGunIcons[characterID];
    }

    #endregion


    #region GameplayFuncions

    //atualiza a pontuação baseada em um valor passado (pode ser negativo)
    public void UpdateHUDScore(int valueVariation)
    {
        playerScore += valueVariation;
        scoreValueText.text = playerScore.ToString("00000");
    }

    //atualiza a visa baseada em um valor passado (pode ser positivo)
    public void UpdateHUDLives(int valueVariation)
    {
        playerLives += valueVariation;
        livesValueText.text = playerLives.ToString("00");

    }

    //atualiza a pontuação para um novo valor passado (copia o valor)
    public void UpdateHUDGunRank (int newlevel)
    {
        gunRank = newlevel;
        for (int i = 0; i < visualGunRanks.Length; i++)
        {
            if (gunRank >= i)
            {
                visualGunRanks[i].SetActive(true);
            }
            else
            {
                visualGunRanks[i].SetActive(false);
            }
        }
    }

    #endregion
}
