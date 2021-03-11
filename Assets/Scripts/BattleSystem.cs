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
    private const string styled = "---"; // I commonly use this const to style headers.


    [Header(styled + " Battle System Settings " + styled)]
    public FightManager fightManager; // References to our FightManager.
    public AudioSource PlayerDeath;
    public DanceTeam teamA,teamB; //References to TeamA and TeamB

    [Header(styled + " Timers " + styled)]
    public float battlePrepTime = 2;  // the amount of time we need to wait before a battle starts
    public float fightCompletedWaitTime = 2; // the amount of time we need to wait till a fight/round is completed.
    
    /// <summary>
    ///    The ratio for how much experience is lost for the losing character.
    /// </summary>
    private int m_loserExperienceRatio = 35;
    private int m_loserExperience; // The experience for the loser of a round 
    private int m_baseExperienceGained; // The base amount of experience gained 
    private int m_winnerDancerLevel; // The Dancer A's Level 
    private int m_loserDancerLevel; // The Dancer B's Level
    
    [Header(styled + " Modifiers  " + styled)]
    public float damageMinimum = 5f;
    public float damageMaximum = 100f;

    [Header(styled + " Health Modifiers " + styled)]
    public float minimumHealthModifier = 0.25f; // minimum amount of health to add to a character upon winning 
    public float maximumHealthModifier = 0.45f; // maximum amount of health to add to a character upon winning 
    
    [Header(styled + " Debugging " + styled)]
    public int teamACurrentActiveCount;
    public int teamBCurrentActiveCount;
    
    /// <summary>
    /// Function occurs every round or every X number of seconds, is the core battle logic/game loop.
    /// </summary>
    /// <returns></returns>
    IEnumerator DoRound()
    {     
        // Amount of seconds to wait before the round starts 
        yield return new WaitForSeconds(battlePrepTime);

        //Check if all dancers from both Team A or Team B have dancers in them. 
        if (teamA.allDancers.Count == 0 && teamB.allDancers.Count == 0)
        { 
            // If there is no dancers in either list, chances are DanceTeamInit is not filled in, or another error has occurred.
            Debug.LogWarning("[DoRound]: " + "There doesnt seem to be any dancers on either team. Complete DanceTeamInit or check logs for errors.");
        }
        // Otherwise if Team A active dancers count is greater than 0 and Team B active dancer's count is greater than 0 
        else if (teamA.activeDancers.Count > 0 && teamB.activeDancers.Count > 0) 
        {
            // We need to select a random dancer from each team to fight 
            Debug.Log("[DoRound]: " + "Selecting a random dancer from Dance Team A and Dance Team B to fight!");
               
            // Generate a random string int hash code to avoid duplicates.
            System.Random rand = new System.Random(DateTime.Now.ToString().GetHashCode());

            int randomTeamAIndex = rand.Next(teamA.activeDancers.Count);  // Select a random index from Team A active dancers list 
            int randomTeamBIndex = rand.Next(teamB.activeDancers.Count);  // Select a random index from Team B active dancers list 

            Debug.Log("[DoRound]: " + "Selecting Index " + randomTeamAIndex + " from Team A Active Dancers List, Selecting Index " + randomTeamBIndex + " from Team B Active Dancers list.");


            Character teamADancer = teamA.activeDancers[randomTeamAIndex]; // Selected Team A Dancer 
            Character teamBDancer = teamB.activeDancers[randomTeamBIndex]; // Selected Team B Dancer

         
            // Initiate the fight between the two dancers. 

            Debug.Log("[DoRound]: " + "Calling FightManager.Fight for Dancers:  " + teamADancer.character_name + " and " + teamBDancer.character_name);
            fightManager.Fight(teamADancer, teamBDancer);
        }
         else
        {
            DanceTeam winnerDanceTeam; // Store a reference to the winner team.

            // If we do not have any active dancers left in Team A
            if (teamACurrentActiveCount <= 0 && teamBCurrentActiveCount > 0)
            {
                // Winner is Team B 
                winnerDanceTeam = teamB;
            }
              else if (teamBCurrentActiveCount <= 0 && teamACurrentActiveCount > 0)  // Otherwise, If we do not have any active dancers left in Team B 
            {
                // Winner is Team A 
                winnerDanceTeam = teamA;
            }
            else
			{
                winnerDanceTeam = null; // Is the same as saying nothing (Often seen as a null reference in your logs)
            }

            
           
            // Check to see whether DanceTeam winner is not equal to null 
            if (winnerDanceTeam != null)
			{
                // If the winner is not equal to null. 
                Debug.Log("[DoRound]: " +  "Dance Team " + winnerDanceTeam.danceTeamName + " has won the round!"); 
               
                winnerDanceTeam.EnableWinEffects(); // Enable the winning team particle effects 
                BattleLog.Log(winnerDanceTeam.danceTeamName.ToString(), winnerDanceTeam.teamColor); // Custom log the winning dance team and their color.
            }
        }
    }

    /// <summary>
    ///     Handles what happens when the fight has completed
    /// </summary>
    /// <param name="winner">The winner Dance Character</param>
    /// <param name="defeated">The defeated Dance Character</param>
    /// <param name="outcome">The outcome result</param>
    public void FightOver(Character winner, Character defeated, float outcome)
    {
     

        // Check the outcome to determine if a draw occurred
        if (outcome <= 0)
		{
            // Then a draw actually occured -> Do nothing!
            Debug.LogWarning("[FightOver]: " + "There was a Draw " + outcome);
		}
        else
		{
            // Otherwise we do have a winner. 

            // Add Experience to winner & Loser 
             m_baseExperienceGained = winner.experienceBase; // Set the base experience for the winner 
             m_loserExperience = m_baseExperienceGained - m_loserExperienceRatio; // The the losers experience
             m_winnerDancerLevel = winner.level; // The winner players level 
             m_loserDancerLevel = defeated.level; // The loser players level

            int winnerExperience = m_baseExperienceGained + (int)(m_loserDancerLevel * winner.levelScalingFactor);
            int loserExperience = m_loserExperience + m_winnerDancerLevel;


            Debug.Log("[FightOver]: " + "Adding Experience " + winnerExperience + " for Winner Dancer: " + winner.character_name + ", Adding Experience " + loserExperience + " for Defeated Dancer: " + defeated.character_name);
            
            winner.AddXP(winnerExperience); // Add experience for the winning dancer 
            defeated.AddXP(loserExperience); // Add experience for the defeated dancer

            // Local Normalized Damage variable between a minimum damage value and a maximum damage value to deal to the defeated character.
            float damage = Mathf.Round(Random.Range(damageMinimum, damageMaximum)) / 100f;
           
            Debug.Log("[FightOver]: " + "Winning dancer " + winner.character_name + " dealt damage " + damage + " to the defeated dancer " + defeated.character_name);

            // Deal damage to the defeated player and check if they are dead 
            bool dancerHasBeenDefeated = defeated.DealDamage(damage) == true;

            // Check to see whether the other dancer has been defeated
            if (dancerHasBeenDefeated == true)
			{
                // Dancer has been defeated 
                Debug.Log("[FightOver]: " + "Dancer " + winner.character_name + " SLAYED on the D-Floor! " + defeated.character_name + " has been sent to the bench!");
                PlayerDeath.Play(); // Play Roblox OOF Death sound for the defeated character

                // We want to add a randomly generated health value between a minimum & maximum health modifier.
                float randomHealth = Random.Range(minimumHealthModifier, maximumHealthModifier);
                Debug.Log("[FightOver]: " + "Dancer " + winner.character_name + " has GAINED " + randomHealth + " for defeating " + defeated.character_name);

                winner.AddHealth(randomHealth); // Add health to the winning character 
			}
            else
            { 
                // We just want to log how much health each character has so we can keep track 
                Debug.LogWarning("[FightOver]: " + "Winning Dancer " + winner.character_name + " has " + winner.mojoRemaining + " health remaining. Defeated Dancer " + defeated.character_name + " has " + defeated.mojoRemaining + " health remaining!");
            }
           }

        // Calling the coroutine so we can add the animation wait time in
        StartCoroutine(HandleFightOver());
    }

    /// <summary>
    /// Used to Request a round.
    /// </summary>
    public void RequestRound()
    {
        // Calls the Coroutine so we can add a wait time for the animations to play.
        StartCoroutine(DoRound());
    }

    /// <summary>
    /// Handles what happens at the end of a fight 
    /// </summary>
    /// <returns></returns>
    IEnumerator HandleFightOver()
    {
        yield return new WaitForSeconds(fightCompletedWaitTime); // The amount of time to wait after a fight has completed 
        teamA.DisableWinEffects(); // Disable the winning effects for Team A 
        teamB.DisableWinEffects(); // Disable the winning effects for Team B 
        teamACurrentActiveCount = teamA.activeDancers.Count; // The current active dancers for Team A 
        teamBCurrentActiveCount = teamB.activeDancers.Count; // The current active dancers for Team B 
        bool danceFightIsOver = false; // Local variable to store whether a fight is over. 

        Debug.Log("[HandleFightOver]: " + "Team A Current Active Count: " + teamACurrentActiveCount + " Team B Current Active Count: " + teamBCurrentActiveCount);
        // Check if either Team A or Team B Current Active Count is less than or equal to zero 
        if (teamACurrentActiveCount <= 0 || teamBCurrentActiveCount <= 0)
        {
            // The dance fight is over and a team has won. 
            danceFightIsOver = true;
        }


       
        // If the dance fight is over 
        if (danceFightIsOver == true)
		{
            // Clear the active dancers list from both dance teams 
            Debug.Log("[HandleFightOver]: " + "The Dance Battle is over! Clearing the Active Dancers list from both Dance Teams!");
            teamA.activeDancers.Clear(); // Clear Team A Active Dancers List. 
            teamB.activeDancers.Clear(); // Clear Team B Active Dancers List.
		}
        else
		{
            // Otherwise if either team current active count is less than or equal to zero and the dance fight ISNT over 
            if ((teamACurrentActiveCount <= 0 || teamBCurrentActiveCount <= 0) && danceFightIsOver == false)
			{
                // An error could have occured, and we would want to log a warning to the console.
                Debug.LogWarning("[HandleFightOver]: " + "HandleFightOver has been called - However we may need to prepare or clean dancers or teams. We should also do checks before doing GameEvents.RequestFighters()");
			}
		}
  
        // Request a new round 
        RequestRound();
    }
}
