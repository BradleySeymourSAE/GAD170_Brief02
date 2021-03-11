using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
///  Generates names for a Dance Team
/// </summary>
public class DanceTeamNameGenerator : MonoBehaviour
{
  
	public const string styled = "---";
	
	[Header(styled + " Possible Team Names " + styled)]
	public List<string> teamNamesList; // list of team names 
	[Header(styled + " Possible Team Adjectives" + styled)]
	public List<string> teamDescriptorsList; // list of descriptors
	
	public void CreateTeamNames()
	{
		if (teamNamesList.Count <= 0)
		{ 
			// Then we want to create a list of names 

			teamNamesList = new List<string>
			{
				"Avant Garde",
				"Immunity",
				"Liquid",
				"Vitality",
				"Global",
				"G2 Esports",
				"FaZe Clan",
				"Cloud9",
				"Team SoloMid",
				"NRG eSports",
				"100 Thieves",
				"Rogue",
				"OpTic",
				"Astralis",
				"Luminosity",
				"Evil Geniuses",
				"Ninjas In Pyjamas"
			};
		}

		if (teamDescriptorsList.Count <= 0)
		{

			teamDescriptorsList = new List<string>
			{
				"The",
				"Team",
				"The Worldwide",
				"The Secret",
				"Envious",
				"The OG",
			};
		}
	}

	/// <summary>
	///  Creates an array of Dance Team Names 
	/// </summary>
	/// <param name="p_teamNamesNeeded">The amount of dance team names needed</param>
	/// <returns></returns>
	public DanceTeamName[] ReturnDanceTeamNames(int p_teamNamesNeeded)
	{
		Debug.Log("[CreateDanceTeamNames]: " + "Creating " + p_teamNamesNeeded + " Dance Team Names!");
		
		// Create a dance team names array based on the amount of team names needed 
		DanceTeamName[] danceTeamNames = new DanceTeamName[p_teamNamesNeeded];

		// Iterate through the amount of danceTeamNames length. 
		for (int i = 0; i < danceTeamNames.Length; i++)
		{
			// Create a new based on the teamNames list and the teamDescriptorsList.
			DanceTeamName s_name = new DanceTeamName
			{
				TeamDescriptor = teamDescriptorsList[Random.Range(0, teamDescriptorsList.Count)],
				TeamName = teamNamesList[Random.Range(0, teamNamesList.Count)]
			};

			// -- Debugging --
			// Debug.Log("[CreateDanceTeamNames]: " + "Created Dance Team Name " + s_name.TeamDescriptor + " " + s_name.TeamName);
			
			
			// Add the team name to the dance team names array 
			danceTeamNames[i] = s_name;
		}


		// Return an array of dance team names that have been generated 
		return danceTeamNames;
	}
}
