using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///		Object struct to hold a complete dance team name.
/// </summary>
[System.Serializable]
public struct DanceTeamName
{
	[SerializeField]
	public string TeamDescriptor; // The Team Descriptor string value
	public string TeamName; // The Team Name string value

	/// <summary>
	///  Returns a dance team name 
	/// </summary>
	/// <param name="p_descriptor"></param>
	/// <param name="p_teamName"></param>
	public DanceTeamName(string p_descriptor, string p_teamName)
	{
		TeamDescriptor = p_descriptor;
		TeamName = p_teamName;
	}

	/// <summary>
	///		Returns a Dance Team's Name 
	/// </summary>
	/// <returns></returns>
	public string GetTeamName()
	{
		return TeamDescriptor + " " + TeamName;
	}
}