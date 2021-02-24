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
    public List<string> lastNames;
    [Header("Possible nicknames")]
    public List<string> nicknames;
    [Header("Possible adjectives to describe the character")]
    public List<string> descriptors;


    /// <summary>
    /// Creates a list of names for all our characters to potentiall use.
    /// This get's called in the battle stater, before both teams call initTeams().
    /// </summary>
    public void CreateNames()
    {
       // Debug.LogWarning("Create Names Called");
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
       //  Debug.LogWarning("CharacterNameGenerator called, it needs to fill out the names array with unique randomly constructed character names");
        CharacterName[] names = new CharacterName[namesNeeded]; 
        
        for (int i = 0; i < names.Length; i++)
        {
            CharacterName _name = new CharacterName
            { 
                firstName = firstNames[Random.Range(0, firstNames.Count)],
                lastName = lastNames[Random.Range(0, lastNames.Count)],
                nickname = nicknames[Random.Range(0, nicknames.Count)],
                descriptor = descriptors[Random.Range(0, descriptors.Count)],
            };

            //For every name we need to generate, we need to assign a random first name, last name, nickname and descriptor to each


            names[i] = _name;
        }

        // Debugging: 
        // foreach (CharacterName _name in names)
        // Debug.Log(_name.firstName);

        //Returns an array of names that we just created.
        return names;
    }
}