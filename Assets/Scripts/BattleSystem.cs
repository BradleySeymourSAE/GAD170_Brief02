using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



/// <summary>
/// Functions to complete:
/// - Do Round
/// - Fight Over
/// </summary>
public class BattleSystem : MonoBehaviour
{
    public DanceTeam teamA,teamB; //References to TeamA and TeamB
    public FightManager fightManager; // References to our FightManager.
    public AudioSource PlayerDeath;

    public float battlePrepTime = 2;  // the amount of time we need to wait before a battle starts
    public float fightCompletedWaitTime = 2; // the amount of time we need to wait till a fight/round is completed.
    
    // Losing a round ratio 
    [Range(0.5f, 3.0f)]
    public float loserExperienceRatio = 1.5f;
    
    public float randomDamageMinimum = 5.0f;
    public float randomDamageMaximum = 100.0f;

    /// <summary>
    /// This occurs every round or every X number of seconds, is the core battle logic/game loop.
    /// </summary>
    /// <returns></returns>
    IEnumerator DoRound()
    {     
        // waits for a float number of seconds....
        yield return new WaitForSeconds(battlePrepTime);

        //checking for no dancers on either team
        if (teamA.allDancers.Count == 0 && teamB.allDancers.Count == 0)
        { 
            Debug.LogWarning("DoRound called, but there are no dancers on either team. DanceTeamInit needs to be completed");
        }
        else if (teamA.activeDancers.Count > 0 && teamB.activeDancers.Count > 0) 
        {
           // Debug.LogWarning("DoRound called, it needs to select a dancer from each team to dance off and put in the FightEventData below");
                
            System.Random rand = new System.Random(DateTime.Now.ToString().GetHashCode());

            int randomTeamAIndex = rand.Next(teamA.activeDancers.Count);
            int randomTeamBIndex = rand.Next(teamB.activeDancers.Count);

            // Get a random dancer from Team A's Active Dancer List 
            // Get a random dancer from Team B's Active Dancer List 
           Character teamADancer = teamA.activeDancers[randomTeamAIndex];
           Character teamBDancer = teamB.activeDancers[randomTeamBIndex];

         
            // Start the fight sequence 
            fightManager.Fight(teamADancer, teamBDancer);
        }
         else
        {
            // We have a winner - team thats won
            DanceTeam winner = null; // null is the same as saying nothing...often seen as a null reference in your logs.

            if (teamA.activeDancers.Count <= 0 && teamB.activeDancers.Count > 0)
                winner = teamB;
            else
                if (teamB.activeDancers.Count <= 0 && teamA.activeDancers.Count > 0)
                winner = teamA;


            Debug.Log("We have a winner: " + winner.danceTeamName);
            // determine a winner
            // look at the previous if statements. 
          

            // Enables win fx, logs it to the console.
            winner.EnableWinEffects();
            BattleLog.Log(winner.danceTeamName.ToString(), winner.teamColor);
            // Debug.Log("DoRound called, but we have a winner so Game Over");
        }
    }

    // handles win / lose 
    public void FightOver(Character winner, Character defeated, float outcome)
    {
       // Debug.LogWarning("FightOver called, may need to check for winners and/or notify teams of zero mojo dancers");   

        if (outcome == 0.0f)
		{
            // Then a draw actually occured -> Do nothing!
            Debug.LogWarning("There was a draw " + outcome);
		}
        else
		{


            // Add Experience to winner & Loser 
            int baseExperience = winner.experienceBase;
            int loserExperience = (int)(baseExperience / loserExperienceRatio);

            int winnerDancerLevel = winner.level;
            int loserDancerLevel = defeated.level;

            int winnerXP = baseExperience + (int)(loserDancerLevel * winner.levelScaling);
            int loserXP = loserExperience + winnerDancerLevel;

            winner.AddXP(winnerXP);
            defeated.AddXP(loserXP);

            // Assign damage to the defeated character 
            float damage = Random.Range(randomDamageMinimum, randomDamageMaximum);

            Debug.Log("Dealing damage " + damage + " to player " + defeated.character_name + " from " + winner.character_name);
            bool isDefeated = defeated.DealDamage(damage);


            if (isDefeated)
			{
                Debug.Log("Winner: " + winner.character_name + " SLAYED " + defeated.character_name);
                PlayerDeath.Play();
			}
            else
                Debug.LogWarning("Winner of this round " + winner.character_name + " with HP " + winner.mojoRemaining + " hasnt yet killed " + defeated.character_name + " with HP " + defeated.mojoRemaining);
            // Check mojo of the defeated dancer is not equal to 0 
        }





        //calling the coroutine so we can put waits in for anims to play
        StartCoroutine(HandleFightOver());
    }

    /// <summary>
    /// Used to Request A round.
    /// </summary>
    public void RequestRound()
    {
        //calling the coroutine so we can put waits in for anims to play
        StartCoroutine(DoRound());
    }

    /// <summary>
    /// Handles the end of a fight and waits to start the next round.
    /// </summary>
    /// <returns></returns>
    IEnumerator HandleFightOver()
    {
        yield return new WaitForSeconds(fightCompletedWaitTime);
        teamA.DisableWinEffects();
        teamB.DisableWinEffects();


        Debug.Log("Team A Active List Count: " + teamA.activeDancers.Count);

        Debug.Log("Team B Active List Count: " + teamB.activeDancers.Count);

        if (teamA.activeDancers.Count <= 0)
            teamA.activeDancers.Clear();

        if (teamB.activeDancers.Count <= 0)
            teamB.activeDancers.Clear();

     
        Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        RequestRound();
    }
}
