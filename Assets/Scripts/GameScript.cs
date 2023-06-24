using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Specialized;

public class GameScript : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI HPCounter;
    private int playerHP, aiHP;
    int[] playerDamageRange = { 1, 2 }, aiDamageRange = { 2, 4 };
    private bool playerTurn, defend, charged;
    private float attackChange = 65f;
    private static bool noAction = false;
    private enum logColors { Red, Magenta, Green, Cyan, Black, Yellow, White };
    Animator anim;


    private void Awake()
    {
        anim = Enemy.GetComponent<Animator>();
    }
    void Start()
    {
        InitializeGame();
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            if (playerTurn)
            {
                // Player's turn
                yield return new WaitForSeconds(3f); // Delay between player turns

                //yield return new WaitForSeconds(2f);
                anim.SetBool("Attacks", false);
            }
            else
            {
                // AI's turn
                yield return new WaitForSeconds(1f); // Delay between AI turns


                anim.SetBool("isHitted", false);
                AiTurn();
            }
        }
    }

    void InitializeGame()
    {
        playerHP = 10;
        aiHP = 10;
        playerTurn = true;
        defend = false;
        charged = false;

    }


    public void PlayerAttack()
    {
        if (!noAction)
        {
            anim.SetBool("isHitted", true);
            int randDamage = Random.Range(playerDamageRange[0], playerDamageRange[1] + 1);
            aiHP -= randDamage;
            playerTurn = false; 
            Print($"You attacked the AI with {randDamage} Damage.", logColors.Yellow, true);
            if (!CheckWinner())
                CurrentHP();
        }

    }

    public void PlayerDefend()
    {
        if (!noAction)
        {
            defend = true;
            playerTurn = false;

            Print("You won't get any Damage the next Round.", logColors.Yellow, true);
        }

    }
    public void PlayerHeal()
    {
        if (!noAction)
        {
            playerHP = playerHP == 10 || playerHP == 9 ? 10 : playerHP + 2;
            playerTurn = false;

            Print("You healed your HP.", logColors.Yellow, true);
            CurrentHP();
        }

    }
    // Handle the AI turn
    void AiTurn()
    {
        if (!noAction)
        {

            if (charged)
            {
                playerHP = defend ? playerHP : 0;
                string text = defend ? "You defend the deadly Attack." : "You couldn't defend the deadly Attack.";
                Print(text, logColors.Yellow, true);
                charged = false;

                if (!CheckWinner())
                    CurrentHP();
            }
            else
            {
                float chance = Random.Range(0f, 100f);
                if (chance < attackChange)
                {
                    // Attack
                    anim.SetBool("Attacks", true);
                    int randDamage = Random.Range(aiDamageRange[0], aiDamageRange[1] + 1);
                    playerHP -= defend ? 0 : randDamage;

                    Print($"The AI attacked You with {randDamage} Damage.", logColors.White, true);

                    if (!CheckWinner())
                        CurrentHP();
                }
                else
                {
                    // Charge
                    charged = true;
                    Print($"The AI is charging, next round the attack will be deadly.", logColors.Red, true);
                }
            }
            defend = false;
            playerTurn = true;
        }
        else
        {
            infoText.text = "";
        }

    }
    public void PlayerRun()
    {
        SceneManager.LoadScene("Game");
    }
    // Print the HP for the Player and Ai after each round
    void CurrentHP()
    {
        HPCounter.text = $"Player HP: {playerHP}";
    }

    // Check if there is a Winner
    bool CheckWinner()
    {
        if (noAction)
        {
            return false;
        }
        if (playerHP <= 0)
        {
            Print("You Lost.", logColors.Red, true);
            noAction = true;
            return true;
        }
        else if (aiHP <= 0)
        {
            Print("You Won.", logColors.Green, true);
            SpriteRenderer[] spriteRenderers = Enemy.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = false;
            }
            Destroy(Enemy);
            noAction = true;
            return true;
        }
        else
            return false;
    }

    void Print(string text, logColors color, bool bold)
    {
        infoText.text = text;
    }
}
