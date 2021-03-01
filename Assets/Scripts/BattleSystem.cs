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
        if (
            teamA.allDancers.Count == 0 && 
            teamB.allDancers.Count == 0
          )
        { 
            Debug.LogWarning("DoRound called, but there are no dancers on either team. DanceTeamInit needs to be completed");
        }
        else if (
            teamA.activeDancers.Count > 0 && 
            teamB.activeDancers.Count > 0
            )
        {
            Debug.LogWarning("DoRound called, it needs to select a dancer from each team to dance off and put in the FightEventData below");
                
            System.Random rand = new System.Random(DateTime.Now.ToString().GetHashCode());
            var team1Dancers = new List<Character>();
            var team2Dancers = new List<Character>();

            for (var i = 0; i < teamA.activeDancers.Count; i++)
            { 
               Character _character = teamA.activeDancers[i];
               team1Dancers.Add(_character);
            }
                
           for (var k = 0; k < teamB.activeDancers.Count; k++)
			{
                Character _character = teamB.activeDancers[k];
                team2Dancers.Add(_character);
			}

           // While team 1's amount of dancers is greater than 0 
           // We want to assign a random character from our team
           // To a character of the other team 
			while (team1Dancers.Count > 0)
			{
				int currentIndex = rand.Next(0, team1Dancers.Count);
				// Debug.Log("Team 1 Character: " + team1Dancers[currentIndex].character_name);

				// Current Character
				Character currentTeam1Dancer = team1Dancers[currentIndex];
                Character currentTeam2Dancer = team2Dancers[currentIndex];

                Debug.Log("Team 1 Dancer: " + currentTeam1Dancer.character_name + " is going to fight Team 2 Dancer: " + currentTeam2Dancer.character_name);


                team1Dancers.RemoveAt(currentIndex);
			}
            
            // select two random characters to fight (one from each team)
            // pass in the selected dancers into fightManager.Fight(CharacterA,CharacterB);
            // simulate battle first if we wanted to.
            // fightManager.Fight(charA, charB);
        }
         else
        {
            // We have a winner a team thats won
            DanceTeam winner = null; // null is the same as saying nothing...often seen as a null reference in your logs.

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
        Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        RequestRound();
    }
}
