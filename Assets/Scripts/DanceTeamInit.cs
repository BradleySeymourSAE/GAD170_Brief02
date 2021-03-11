using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Functions to complete:
/// - Init Teams
/// </summary>
public class DanceTeamInit : MonoBehaviour
{
    public DanceTeam teamA, teamB; // A reference to our teamA and teamB DanceTeam instances.

    public GameObject dancerPrefab; // This is the dancer that gets spawned in for each team.
    public int dancersPerSide = 3; // This is the number of dancers for each team, if you want more, you need to modify this in the inspector.
    public int totalDanceTeams = 2; // Total amount of teams to generate - TODO: Technically we could add more than 2 teams, Will look into implementing in the future.
    public CharacterNameGenerator nameGenerator; // This is a reference to our CharacterNameGenerator instance.
    public DanceTeamNameGenerator danceTeamNameGenerator; // This is a reference to our DanceTeamNameGenerator instance.
    private CharacterName[] teamACharacterNames; // An array to hold all our character names of TeamA.
    private CharacterName[] teamBCharacterNames; // An array to hold all the character names of TeamB
    private DanceTeamName[] danceTeamNames; // An array to hold character names 

    /// <summary>
    /// Called to initialize the dance teams with some dancers :D
    /// </summary>
    public void InitTeams()
    {
       
        Debug.Log("[InitializeTeams]: " + "Initializing Dance Teams with Dancers...");

        // Generate random team names by the amount of dance teams being initialized.
      
        // Check if Dance Team A Exists
        if (teamA != null && teamB != null)
        { 
            // 
            string teamADanceTeamName = teamA.danceTeamName;
            string teamBDanceTeamName = teamB.danceTeamName;

            // Check if Dance Team A or Dance Team B names are empty strings 
            if (teamADanceTeamName == "" || teamBDanceTeamName == "")
			{
                // If the name is empty, we want to generate a random dance team name 
                danceTeamNames = danceTeamNameGenerator.ReturnDanceTeamNames(totalDanceTeams);

                // TODO: I need to clean the following, There is a better way of doing this but my brain 
                // isn't working properly haha.
                int currentIndex = 0;
                for (var i = 0; i < danceTeamNames.Length; i++)
				{
                    currentIndex++;
                    // Check if current index = 1, assign the danceTeamNames
                    if (currentIndex == 1)
					{
                        // Setting Team A Dance name to the first element in the danceTeamNames array 
                        teamADanceTeamName = danceTeamNames[0].GetTeamName();
					}
					
                    // If the current index is 2 
                    if (currentIndex == 2)
				    {
                        // Setting Team B Dance name to the second element in the danceTeamNames array 
                            teamBDanceTeamName = danceTeamNames[1].GetTeamName();
					}
				}
			}

          
            Debug.Log("[DanceTeamInit]: " + "Setting Dance Team A  Team name to " + teamADanceTeamName + " and Dance Team B Team name to " + teamBDanceTeamName);
            
            // Set Dance Team 1's name
            teamA.SetTroupeName(teamADanceTeamName); // Set the troupe name for dance team a 
            teamB.SetTroupeName(teamBDanceTeamName); // Set the troupe name for dance team b 


            // Setting Team ID - Team A = 1, Team B = 2 

            teamA.SetDanceTeamID(1); // Set Dance Team ID for Team A
            teamB.SetDanceTeamID(2); // Set Dance Team ID for Team B 

            // Create an array of team a character names equal to that of the dancers per side 
            teamACharacterNames = nameGenerator.ReturnTeamCharacterNames(dancersPerSide);
           
            // Create an array of team b character names equal to that of the dancers per side 
            teamBCharacterNames = nameGenerator.ReturnTeamCharacterNames(dancersPerSide);

            // Check whether either teams character name lengths are less than the dancers per side
            if (teamACharacterNames.Length < dancersPerSide || teamBCharacterNames.Length < dancersPerSide)
			{
                // Handle the error.
                Debug.LogWarning("[DanceTeamInit]" + " There was an error returning Dance Team A's Character names!");
                return;
			}
            else
            { 
                Debug.Log("[DanceTeamInit]: " + "Initializing Dance Teams from character names!");
                // Everything went okay 
                // Initialize the team using the dancer prefab, the dance teams direction and the character names generated
                teamA.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Left, teamACharacterNames);
                teamB.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Right, teamBCharacterNames);
			}
        }
    }
}
