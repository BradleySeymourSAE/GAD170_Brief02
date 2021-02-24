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
    public CharacterNameGenerator nameGenerator; // This is a reference to our CharacterNameGenerator instance.
    private CharacterName[] teamACharacterNames; // An array to hold all our character names of TeamA.
    private CharacterName[] teamBCharacterNames; // An array to hold all the character names of TeamB

    /// <summary>
    /// Called to iniatlise the dance teams with some dancers :D
    /// </summary>
    public void InitTeams()
    {
       //  Debug.LogWarning("InitTeams called, needs to generate names for the teams and set them with teamA.SetTroupeName and teamA.InitialiseTeamFromNames");
       
        // Set the name of the team 
        if (teamA != null)
        { 
            string s_DanceTeam1Name = teamA.danceTeamName;
            // If there is no name set for the team, set a default name. 
            if (s_DanceTeam1Name == "")
                s_DanceTeam1Name = "The " + dancersPerSide + " Stars";
            
            // Set Dance Team 1's name
            teamA.SetTroupeName(s_DanceTeam1Name);

            teamACharacterNames = nameGenerator.ReturnTeamCharacterNames(dancersPerSide);

            if (teamACharacterNames.Length < dancersPerSide)
			{
                Debug.LogWarning("[DanceTeamInit]" + " There was an error returning Dance Team A's Character names!");
                return;
			}
            else
			{

                // Everything went okay 
                // Initialize the team using the dancer prefab, the dance teams direction and the character names generated
                teamA.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Left, teamACharacterNames);
			}
        }

        if (teamB != null)
        { 
            string s_DanceTeam2Name = teamB.danceTeamName;
            // If there is no name set for the team, set a default name.
            if (s_DanceTeam2Name == "")
               s_DanceTeam2Name = "The Chosen " + dancersPerSide;

            // Set Dance Team 2's name 
            teamB.SetTroupeName(s_DanceTeam2Name);

            teamBCharacterNames = nameGenerator.ReturnTeamCharacterNames(dancersPerSide);

            if (teamBCharacterNames.Length < dancersPerSide || teamBCharacterNames.Length <= 0)
			{
                Debug.LogWarning("[DanceTeamInit]" + " There was an error returning Dance Team B's Character names!");
                return;
			}
            else
			{
                // Everything is okay 
                // Initialize the team using the dancer prefab, the dance teams direction and the character names generated

                teamB.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Right, teamBCharacterNames);
            }
        }
    }
}
