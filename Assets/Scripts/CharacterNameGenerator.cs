using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
        Debug.LogWarning("Create Names Called");
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
        Debug.LogWarning("CharacterNameGenerator called, it needs to fill out the names array with unique randomly constructed character names");
        CharacterName[] names = new CharacterName[namesNeeded]; 
        CharacterName _result = new CharacterName(string.Empty, string.Empty, string.Empty, string.Empty);


        for (int i = 0; i < names.Length; i++)
        {
           
            Random _random = new Random();


            //For every name we need to generate, we need to assign a random first name, last name, nickname and descriptor to each.
            //Below is an example of setting the first name of the emptyName variable to the string "Blank".
            _result.firstName = firstNames[_random.Next(0, names.Length)];
            _result.lastName = lastNames[_random.Next(0, names.Length)];
            _result.nickname = nicknames[_random.Next(0, names.Length)];
            _result.descriptor = descriptors[_random.Next(0, names.Length)];

            Debug.Log("RESULT: " + " First Name: " + _result.firstName + " Last Name: " + _result.lastName + " Nick: " + _result.nickname + " Descriptor: " + _result.descriptor);
            names[i] = _result;
        }

        Debug.Log("Generated Names: " + names);

        //Returns an array of names that we just created.
        return names;
    }
}