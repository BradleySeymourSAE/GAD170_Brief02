using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Functions to complete:
/// - Simulate Battle
/// - Attack
/// </summary>
public class FightManager : MonoBehaviour
{
    private const string styled = "---";
    [Header(styled + " Fight Manager Settings " + styled)]
    [Tooltip("Reference to the BattleSystem script in our scene")]
    public BattleSystem battleSystem; //A reference to our battleSystem script in our scene
    [Tooltip("Sets the battle log message's color")]
    public Color drawCol = Color.gray; // A colour you might want to set the battle log message to if it's a draw.
    [SerializeField]
    [Tooltip("The amount of time to wait between initiaing the fight")]
    private float fightAnimTime = 2;  //An amount to wait between initiating the fight, and the fight begining, so we can see some of that sick dancing.
    public float lossRatio = 1.5f;
    public float powerLevelScalingFactor = 3.0f;

    /// <summary>
    /// Returns a float of the percentage chance to win the fight based on your characters current stats.
    /// </summary>
    /// <param name="p_TeamADancer">A dancer from Dance Team A</param>
    /// <param name="p_TeamBDancer">A dancer from Dance Team B</param>
    /// <returns></returns>
    public float SimulateBattle(Character p_TeamADancer, Character p_TeamBDancer)
    {
        float teamADancePoints = p_TeamADancer.ReturnDancePowerLevel(); // Team A Dancers Power Level
        float teamBDancePoints = p_TeamBDancer.ReturnDancePowerLevel(); // Team B Dancers Power Level
        double winningPercentage; // The percent chance the character will win the fight 

        if (teamADancePoints <= 0 || teamBDancePoints <= 0)
        { 
            Debug.LogWarning("[SimulateBattle]: " + "Simulate battle has been called; Dance character 1 or Dance character 2's battle points is returned a value of 0");
        }

        // If Team A Dance Power Level points are greater than Team B Dance Power Level points 
        if (teamADancePoints > teamBDancePoints)
        { 
            // The winning percentage is calculated (250 / 300) * 100f;
            winningPercentage = (double)(teamBDancePoints / teamADancePoints) * 100f;
        }
          else
        {
            // The winning percentage is calculated eg: (250 / 300) * 100f;
            winningPercentage = (double)(teamADancePoints / teamBDancePoints) * 100f;
        }

        // We do this to avoid running into a value such as (300 / 250) * 100f; -> Which is incorrect.
        
        Debug.Log("[SimulateBattle]: " + "Chance of winning: " + (float)Math.Round(winningPercentage, 2));

        // Return the chance of winning percentage as a float (rounded to 2 decimal places) 
        return (float)Math.Round(winningPercentage, 2);
    }


    /// <summary>
    ///   Handles the Fight between a dancer from each team 
    /// </summary>
    /// <param name="teamADancer">A dancer from Team A </param>
    /// <param name="teamBDancer">A dancer from Team B</param>
    /// <returns></returns>
    IEnumerator Attack(Character teamADancer, Character teamBDancer)
    {


       // Let's get the dance power levels from our dancers.
        float dancerAPowerLevel = teamADancer.ReturnDancePowerLevel(); // Dancer A Power Level 
        float dancerBPowerLevel = teamBDancer.ReturnDancePowerLevel(); // Dancer B Power Level 

        Debug.Log("[Attack]: " + "Before randomising dance power level for Dancer A: " + dancerAPowerLevel + ", Dancer B: " + dancerBPowerLevel);

        dancerAPowerLevel = Random.Range(30f, dancerAPowerLevel);
        dancerBPowerLevel = Random.Range(30f, dancerBPowerLevel);



        Debug.Log("[Attack]: " + "Team A Dancer Power Level " + dancerAPowerLevel + " VS " + " Team B Dancer Power Level: " + dancerBPowerLevel);
        // Local variable to store the winner of a round 
        var winner = 0; // 0 = draw, 1 = Team A Dancer, -1 = Team B Dancer.
        var winnerDancer = teamADancer; // set the default winner to be character a 
        var defeatedDancer = teamBDancer; // set default defeated to be character b 

        SetUpAttack(teamADancer); // Setup Team A Dancers Dance Animation
        SetUpAttack(teamBDancer); // Setup Team B Dancers Dance Animation

        // Wait X number of seconds until the fight begins!
        yield return new WaitForSeconds(fightAnimTime);
   
        
       // If Dancer A's Power Level is greater than Dancer B's Power Level 
        if (dancerAPowerLevel > dancerBPowerLevel)
		{
            // Dancer from Team A won 
            Debug.Log("Team A Dancer " + teamADancer.character_name + "(" + dancerAPowerLevel + ")" + " WON against Team B Dancer " + teamBDancer.character_name + "(" + dancerBPowerLevel + ")");
            winnerDancer = teamADancer;
            defeatedDancer = teamBDancer;
            winner = 1;
		}
        // Otherwise if Dancer A Power Level is Less than Dancer B Power Level 
        else if (dancerAPowerLevel < dancerBPowerLevel)
		{
            // Dancer from Team B Won
            Debug.Log("Team B Dancer " + teamBDancer.character_name + "(" + dancerBPowerLevel + ")" + " WON against Team A Dancer " + teamADancer.character_name + "(" + dancerAPowerLevel + ")");
            winner = -1;
            winnerDancer = teamBDancer;
            defeatedDancer = teamADancer;
		}
        else
		{
            // Check if the power levels are the same on the off chance it was a draw 
            if (dancerAPowerLevel == dancerBPowerLevel)
			{
                // It was a draw 
                winner = 0;
                Debug.LogWarning("Dancer " + teamADancer.character_name + "(" + dancerAPowerLevel + ")" + " DRAWED with " + teamBDancer.character_name + "(" + dancerBPowerLevel + ")");
			}
		}


        float outcome;
        // Fight completed outcome takes in a float parameter, so we need to convert the winner from an integer to a float either here or below.
        if (winner == 0)
		{
            outcome = 0f;
		}
        else if (winner == -1)
		{
            outcome = (float)(-winner * 100f);
		}
        else
            outcome = (float)(winner * 100f);



        Debug.Log("[Attack]: " + "Calling FightCompleted with winner " + winnerDancer.character_name + " Defeated Dancer: " + defeatedDancer.character_name + " with the fight outcome " + outcome);   
        FightCompleted(winnerDancer, defeatedDancer, outcome); // Pass on the winner/loser and the outcome to our fight completed function.
        yield return null; // Otherwise returns null.
    }

    #region Pre-Existing - No Modes Required
    /// <summary>
    /// Is called when two dancers have been selected and begins a fight!
    /// </summary>
    /// <param name="p_TeamADancer">A character from Dance Team A</param>
    /// <param name="p_TeamBDancer">A character from Dance Team B</param>
    public void Fight(Character p_TeamADancer, Character p_TeamBDancer)
    {
        // Starts the the attack between a dancer from both teams.
        StartCoroutine(Attack(p_TeamADancer, p_TeamBDancer));
    }

    /// <summary>
    /// Passes in a winning character, and a defeated character, as well as the outcome 
    /// </summary>
    /// <param name="winner"></param>
    /// <param name="defeated"></param>
    /// <param name="outcome"></param>
    private void FightCompleted(Character winner, Character defeated, float outcome)
    {
        var results = new FightResultData(winner, defeated, outcome);

        winner.isSelected = false;
        defeated.isSelected = false;

        battleSystem.FightOver(winner,defeated,outcome);
        winner.animController.BattleResult(winner,defeated,outcome);
        defeated.animController.BattleResult(winner, defeated, outcome);
    }

    /// <summary>
    /// Sets up a dancer to be selected and the animation to start dancing.
    /// </summary>
    /// <param name="dancer"></param>
    private void SetUpAttack(Character dancer)
    {
        dancer.isSelected = true;
        dancer.GetComponent<AnimationController>().Dance();
        BattleLog.Log(dancer.charName.GetFullCharacterName() + " Selected", dancer.myTeam.teamColor);
    }
    #endregion  
}
