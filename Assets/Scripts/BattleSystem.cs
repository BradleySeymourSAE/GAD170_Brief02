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
    private const string styled = "---";
    [Header(styled + " Manager Settings " + styled)]
    public FightManager fightManager; // References to our FightManager.
    public AudioSource PlayerDeath;
    public DanceTeam teamA,teamB; //References to TeamA and TeamB

    [Header(styled + " Timers " + styled)]
    public float battlePrepTime = 2;  // the amount of time we need to wait before a battle starts
    public float fightCompletedWaitTime = 2; // the amount of time we need to wait till a fight/round is completed.
    
    // Losing a round ratio 
    [Range(0.5f, 3.0f)]
    public float loserExperienceRatio = 1.5f;
    
    [Header(styled + " Damage Range " + styled)]
    public float randomDamageMinimum = 5f;
    public float randomDamageMaximum = 100f;

    [Header(styled + " Health Range " + styled)]
    public float minimumHealthMultiplier = 0.25f;
    public float maximumHealthMultiplier = 0.45f;
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

            if (teamA.activeDancers.Count <= 0)
                winner = teamB;
            else
            {
                if (teamB.activeDancers.Count <= 0)
                winner = teamA;
            }


            // Check to see whether we have a Dance Team that has won!
            if (winner != null)
			{
                // Log to the console 
                // Enable the winning particle effects 
                Debug.Log("Winning Team: " + winner.danceTeamName); 
                winner.EnableWinEffects();
                BattleLog.Log(winner.danceTeamName.ToString(), winner.teamColor); 
            }
        }
    }

    // handles win / lose 
    public void FightOver(Character winner, Character defeated, float outcome)
    {
       // Debug.LogWarning("FightOver called, may need to check for winners and/or notify teams of zero mojo dancers");   

        if (outcome == 0f)
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

            // Get a random damage value within the set minimum and maximum range 
            float damageValue = Mathf.Round(Random.Range(randomDamageMinimum, randomDamageMaximum));
           
            //  Debug.LogWarning(defeated.character_name + " got random damage value " + damageValue);
            // Divide the value by 100f to get our normalized value (0-100f) -> (0.0, 1.0f)
            float damage = damageValue / 100f;
            Debug.Log("Assigning normalized damage value: " + damage);

            // Assign damage to the defeated character 

            Debug.Log(winner.character_name + " dealt damage " + damage + " to player " + defeated.character_name);

            // Deal damage to the defeated player and check if they are dead 
            bool isDefeated = defeated.DealDamage(damage) == true;


            if (isDefeated)
			{
                Debug.Log("Winner: " + winner.character_name + " SLAYED " + defeated.character_name);

                // If the winner defeats the other character (As in the other character 'died') then we want to 
                // add health to the winner
               
                float randomHealth = Random.Range(minimumHealthMultiplier, maximumHealthMultiplier);

                Debug.LogWarning("Adding Health " + randomHealth + " to character " + winner.character_name);

                winner.AddHealth(randomHealth);

                PlayerDeath.Play();
			}
            else
            { 
                Debug.LogWarning(winner.character_name + " has " + winner.mojoRemaining + " health remaining. " + defeated.character_name + " has " + defeated.mojoRemaining + " health remaining.");
            }
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

        bool fightIsOver = false;

        int teamACurrentActiveCount = teamA.activeDancers.Count;
        int teamBCurrentActiveCount = teamB.activeDancers.Count;

        // Simple check to determine whether the fight is over or not 
        if (teamACurrentActiveCount <= 0 || teamBCurrentActiveCount <= 0)
            fightIsOver = true;
        
        // Debugging purposes to find the count of players on each side 
        // Debug.Log("Team A Active Count: " + teamACurrentActiveCount + " Team B Active Count: " + teamBCurrentActiveCount);

        if (fightIsOver)
		{
            Debug.Log("Battle is over! Clearing active dancers list!");
            teamA.activeDancers.Clear();
            teamB.activeDancers.Clear();
		}
       
        if ((teamACurrentActiveCount <= 0 || teamBCurrentActiveCount <= 0) && fightIsOver == false)
		{
            Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        }


        RequestRound();
    }
}
