using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions to complete:
/// - Simulate Battle
/// - Attack
/// </summary>
public class FightManager : MonoBehaviour
{
    public BattleSystem battleSystem; //A reference to our battleSystem script in our scene
    public Color drawCol = Color.gray; // A colour you might want to set the battle log message to if it's a draw.
    private float fightAnimTime = 2; //An amount to wait between initiating the fight, and the fight begining, so we can see some of that sick dancing.

    /// <summary>
    /// Returns a float of the percentage chance to win the fight based on your characters current stats.
    /// </summary>
    /// <param name="charOne"></param>
    /// <param name="charTwo"></param>
    /// <returns></returns>
    public float SimulateBattle(Character charOne, Character charTwo)
    {
        int charOnePoints = charOne.ReturnDancePowerLevel(); // our current powerlevel
        int charTwoPoints = charTwo.ReturnDancePowerLevel(); // our opponents current power level

        if (charOnePoints <= 0 || charTwoPoints <= 0)
        {
            Debug.LogWarning(" Simulate battle called; but char 1 or char 2 battle points is 0, most likely the logic has not be setup for this yet");
        }

        // we probably want to compare our powerlevels...hope they aren't over 9000.
        // we need to return a normalised (decimal) value....how much do you remember about percentages?
        // don't forget that we are returning a float...but diving 2 ints...what happens?

        Debug.LogWarning("Simulate battle called, but the logic hasn't been set up yet, so defaulting to 0");
        return 0;
    }


    //TODO this function is all you need to modify, in this script.
    //You just need determine who wins/loses/draws etc.
    IEnumerator Attack(Character teamACharacter, Character teamBCharacter)
    {

        Character winner = teamACharacter;//defaulting the winner to TeamA.
        Character defeated = teamBCharacter;//defaulting the loser to TeamB.
        float outcome = 0;// the outcome from the fight, i.e. the % that the winner has won by...fractions could help us calculate this, but start with whole numbers i.e. 0 = draw, and 1 = 100% win.
       
        // We want to get some battle points from each of our characters...instead of just 0....is there a function in the Character script that could help us?
        int teamABattlePoints = teamACharacter.ReturnDancePowerLevel();
        int teamBBattlePoints = teamBCharacter.ReturnDancePowerLevel();


        // Tells each dancer that they are selcted and sets the animation to dance.
        SetUpAttack(teamACharacter);
        SetUpAttack(teamBCharacter);

        // Tells the system to wait X number of seconds until the fight to begins.
        yield return new WaitForSeconds(fightAnimTime);

        // We need to do some logic hear to check who wins based on the battle points, we want to handle team A winning, team B winning and draw scenarios.

        // by default we set the winner to be character a, for defeated we set it to B.
        winner = teamACharacter;
        defeated = teamBCharacter;
        outcome = 0;// this really needs to be a fraction of the win vs the loser, but if it's a draw 0 is okay.
        BattleLog.Log("Fight is a draw!", drawCol);

        Debug.LogWarning("Attack called, may want to use the BattleLog.Log() to report the dancers and the outcome of their dance off.");

        // Pass on the winner/loser and the outcome to our fight completed function.
        FightCompleted(winner, defeated, outcome);
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
