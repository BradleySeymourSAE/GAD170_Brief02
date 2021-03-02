using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// Functions to complete:
/// - Do Round
/// - Fight Over
/// </summary>
public class BattleSystem : MonoBehaviour
{
    public DanceTeam teamA,teamB; //References to TeamA and TeamB
    public FightManager fightManager; // References to our FightManager.

    public float battlePrepTime = 2;  // the amount of time we need to wait before a battle starts
    public float fightCompletedWaitTime = 2; // the amount of time we need to wait till a fight/round is completed.


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
            Debug.LogWarning("DoRound called, it needs to select a dancer from each team to dance off and put in the FightEventData below");
                
            System.Random rand = new System.Random(DateTime.Now.ToString().GetHashCode());
		
                Character teamADancer = null;
                Character teamBDancer = null;

                // Random dancer from each team 
                int teamADancerIndex = rand.Next(teamA.activeDancers.Count);
                int teamBDancerIndex = rand.Next(teamB.activeDancers.Count);
                int currentFightIndex = 0;

                // For each active dancer in team A
                for (var i = 0; i < teamA.activeDancers.Count; i++)
                { 
                    // Select a random dancer from the list 
                    Character _dancer = teamA.activeDancers[teamADancerIndex];
                    teamADancer = _dancer;
                }
                // For each active dancer in team b 
                for (var j = 0; j < teamB.activeDancers.Count; j++)
                {
                    // Select a random dancer from the list 
                    Character _dancer = teamB.activeDancers[teamBDancerIndex];
                    teamBDancer = _dancer;
                }
            
                // We have a random dancer from each side 
                // <Nathan.Jensen>
                // Characters need to fight 
                // TODO: Currently im only getting one character each side, when I use a list the fight manager throws an error.
                // If this was me i would pass a whole character array through the fight manager (bunch of characters) and handle it that 
                // way but right now im actually fairly stuck and confused 

                // I'm sort of getting stuck on the logical part of this moreso that understanding the coding side? 


      
          
		        fightManager.Fight(teamADancer, teamBDancer);
        }
         else
        {
            // We have a winner a team thats won
            DanceTeam winner = null; // null is the same as saying nothing...often seen as a null reference in your logs.

            Debug.Log("We have a winner: " + winner.danceTeamName);
            // determine a winner
            // look at the previous if statements. 
          

            // Enables win fx, logs it to the console.
            winner.EnableWinEffects();
            BattleLog.Log(winner.danceTeamName.ToString(), winner.teamColor);

            Debug.Log("DoRound called, but we have a winner so Game Over");
          
        }
    }

    // handles win / lose 
    public void FightOver(Character winner, Character defeated, float outcome)
    {
        Debug.LogWarning("FightOver called, may need to check for winners and/or notify teams of zero mojo dancers");   
        
        Debug.Log("Winner: " + winner + " Defeated: " + defeated + " Outcome: " + outcome);
        
       

        // assign damage...or if you want to restore health if they want that's up to you....might involve the character script.

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

       
        if (teamA.activeDancers.Count > 0)
            teamA.activeDancers.Clear();
        
        if (teamB.activeDancers.Count > 0)
            teamB.activeDancers.Clear();

        Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        RequestRound();
    }
}
