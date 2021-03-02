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


    /// <summary>
    /// Returns a float of the percentage chance to win the fight based on your characters current stats.
    /// </summary>
    /// <param name="p_TeamADancer"></param>
    /// <param name="p_TeamBDancer"></param>
    /// <returns></returns>
    public float SimulateBattle(Character p_TeamADancer, Character p_TeamBDancer)
    {
        int currentTeamADancerPoints = p_TeamADancer.ReturnDancePowerLevel(); // our current powerlevel
        int currentTeamBDancerPoints = p_TeamBDancer.ReturnDancePowerLevel(); // our opponents current power level

        if (currentTeamADancerPoints <= 0 || currentTeamBDancerPoints <= 0)
            Debug.LogWarning("Simulate battle called; but char 1 or char 2 battle points is 0, most likely the logic has not be setup for this yet");
        
        // We need to cast integer values to a float
        float _teamADancerPoints = currentTeamADancerPoints;
        float _teamBDancerPoints = currentTeamBDancerPoints;
        double winningPercentage;

        // calculate the winning percentage by setting it as a normalised (decimal) value

        if (_teamADancerPoints > _teamBDancerPoints)
            winningPercentage = (double)(_teamBDancerPoints / _teamADancerPoints) * 100f;
        else
            winningPercentage = (double)(_teamADancerPoints / _teamBDancerPoints) * 100f;

        // We want to return a float value - as this is what the function REQUIRES to return

        Debug.Log("Chance of winning: " + (float)Math.Round(winningPercentage, 2));        

        // Return winning percentage float as a value for 2 decimal places 
        return (float)Math.Round(winningPercentage, 2);
    }


    //TODO this function is all you need to modify, in this script.
    //You just need determine who wins/loses/draws etc.
    IEnumerator Attack(Character teamADancer, Character teamBDancer)
    {


       // Let's get the dance power levels from our dancers.
        int teamADancerPowerLevel = teamADancer.ReturnDancePowerLevel();
        int teamBDancerPowerLevel = teamBDancer.ReturnDancePowerLevel();
        int winner = 0;

        // by default we set the winner to be character a, for defeated we set it to B.
        var _winningDancer = teamADancer;
        var _defeatedDancer = teamBDancer;


        // Tells each dancer that they are selcted and sets the animation to dance.
        SetUpAttack(teamADancer);
        SetUpAttack(teamBDancer);

        // Tells the system to wait X number of seconds until the fight to begins.
        yield return new WaitForSeconds(fightAnimTime);



        if (teamADancerPowerLevel == teamBDancerPowerLevel)
            winner = 0;
        else if (teamADancerPowerLevel > teamBDancerPowerLevel)
            winner = 1;
        else
            if (teamADancerPowerLevel < teamBDancerPowerLevel)
                winner = -1;
        

        int baseExperience = teamADancer.experienceBase;
        int loserExperience = (int)(baseExperience / lossRatio);
        int currentTeamADancerLevel = teamADancer.level;
        int currentTeamBDancerLevel = teamBDancer.level;

        switch (winner)
        {
            case 0:
                // If there was a draw we can assign base experience to both 
                // add the other dancers experience 

                int teamADancerXP = baseExperience + (currentTeamADancerLevel * 2);
                int teamBDancerXP = baseExperience + (currentTeamBDancerLevel * 2);


                teamADancer.AddXP(teamADancerXP);
                teamBDancer.AddXP(teamBDancerXP);
                break;
            case 1:
                // Team A won
                baseExperience = baseExperience + (int)(currentTeamADancerLevel * Random.Range(1f, 2.5f));

                _winningDancer = teamADancer;
                _defeatedDancer = teamBDancer;

                teamADancer.AddXP(baseExperience);
                teamBDancer.AddXP(loserExperience);
                break;
            case -1:
                // Team B won
                baseExperience = baseExperience + (int)(currentTeamBDancerLevel * Random.Range(1f, 2.5f));

                _winningDancer = teamBDancer;
                _defeatedDancer = teamADancer;

                teamADancer.AddXP(loserExperience);
                teamBDancer.AddXP(baseExperience);
                break;
            default:
                Debug.LogWarning("Error in FightManager (Switch Statement)");
                break;
        }

        float outcome = 0f;
       
        if (winner == 0f)
            outcome = 0;
        else if (winner == 1)
            outcome = (float)(winner * 100f);
        else
            if (winner == -1)
            outcome = (float)(winner * -100f);



        Debug.Log("Winning Outcome Percentage: " + outcome);

        if (outcome <= 0f)
            BattleLog.Log("Fight resulted in a draw!", drawCol);

        // Pass on the winner/loser and the outcome to our fight completed function.
        FightCompleted(_winningDancer, _defeatedDancer, outcome);
        yield return null;
    }

    #region Pre-Existing - No Modes Required
    /// <summary>
    /// Is called when two dancers have been selected and begins a fight!
    /// </summary>
    /// <param name="data"></param>
    public void Fight(Character TeamA, Character TeamB)
    {
        StartCoroutine(Attack(TeamA, TeamB));
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
