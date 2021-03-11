using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Functions to complete:
/// Create Names
/// - Generate Names
/// </summary>
public class CharacterNameGenerator : MonoBehaviour
{
 
    [Header("Possible first names")]
    public List<string> firstNames; // These appear in the inspector, you should be assigning names to these in the inspector.
    [Header("Possible last names")]
    public List<string> lastNames; // These appear in the inspector, you should be assigning names to these in the inspector.
    [Header("Possible nicknames")]
    public List<string> nicknames; // These appear in the inspector, you should be assigning names to these in the inspector.
    [Header("Possible adjectives to describe the character")]
    public List<string> descriptors; // These appear in the inspector, you should be assigning names to these in the inspector.


    /// <summary>
    /// Creates a list of names for all our characters to potentiall use.
    /// This get's called in the battle stater, before both teams call initTeams().
    /// </summary>
    public void CreateNames()
    {
       Debug.Log("[CharacterNameGenerator.CreateNames]: " + " Creating names a list of firstNames, lastNames, nicknames and descriptors!");

       // Check to see that list isnt empty.
       if (firstNames.Count <= 0)
            firstNames = new List<string> { 
                "Don", 
                "Leslie", 
                "Enrico", 
                "Vernon",
                "Ciara",
                "Taylor",
                "Dean",
                "Misty",
                "Sara",
                "Mikhail",
                "Sergei",
                "Lucinda"
            };
        // Check to see the last names list isnt empty.
        if (lastNames.Count <= 0)
            lastNames = new List<string> {
                "Deboo",
                "de Weever",
                "Doo",
                "Elssler",
                "Esbrez",
                "Glover",
                "Graham",
                "Hammer",
                "Hawkins",
                "Hines",
                "Horton",
                "Humphrey"
            };

        // Check to see that the nicknames list isnt empty.
        if (nicknames.Count <= 0)
            nicknames = new List<string> {
                "The Queen of Style",
                "The King of Dance",
                "The Talented",
                "The Leader",
                "The Swift",
                "The Coordinated",
                "The Principal Dancer",
                "The Artist",
                "The Dancing Taeguk Warrior",
                "The Dance Mom",
                "The Champion",
                "The Gifted"
            };

        // Check to see that the descriptors list isnt empty.
        if (descriptors.Count <= 0)
            descriptors = new List<string> {
                "wild",
                "religious",
                "light",
                "informal",
                "swift",
                "lovely",
                "frenzied",
                "frantic",
                "fierce",
                "certain",
                "vivacious",
                "unpremeditated"
            };


    }

    /// <summary>
    /// Returns an Array of Character Names based on the number of namesNeeded.
    /// </summary>
    /// <param name="namesNeeded"></param>
    /// <returns></returns>
    public CharacterName[] ReturnTeamCharacterNames(int namesNeeded)
    {
        Debug.Log("[ReturnTeamCharacterNames]: " + "Creating " + namesNeeded + " character names!");
        
        // Create a character names array based on the amount of names needed 
        CharacterName[] names = new CharacterName[namesNeeded]; 


        // For the amount of names needed, we are going to iterate through
        for (int i = 0; i < names.Length; i++)
        {
            // Create a name based on the list of first names, last names, nicknames and descriptors.
            CharacterName _name = new CharacterName
            { 
                firstName = firstNames[Random.Range(0, firstNames.Count)],
                lastName = lastNames[Random.Range(0, lastNames.Count)],
                nickname = nicknames[Random.Range(0, nicknames.Count)],
                descriptor = descriptors[Random.Range(0, descriptors.Count)],
            };

     
            // Add the character name to the character names array 

            // Debugging
            // Debug.Log("[ReturnTeamCharacterNames]: " + "Adding " + _name.firstName + " to the character names array!");
            names[i] = _name;
        }

       
        // Return an array of character names that have been generated
        return names;
    }
}