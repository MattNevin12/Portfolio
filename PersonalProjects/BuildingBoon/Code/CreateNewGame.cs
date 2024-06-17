using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNewGame : ScriptableObject
{
    public void Create(Character character)
    {
        // First, find the account information for the player
        Account account;

        // Get the account or create one
        if (!GameManager.Instance.FoundFile<Account>("/account.json"))
        {
            Debug.Log("Making New Account");

            account = new Account();
            account.id = "1";

            GameManager.Instance.SaveAccount(account);
        }
        else
        {
            account = GameManager.Instance.LoadAccount();
            Debug.Log("Account exists");
        }

        // Get the list of character IDs from the account so we can create a new character without overriding previous characters
        List<uint> characterIds = new List<uint>();
        bool idExists = false;

        if (account.characters.Count > 0)
        {
            foreach (Character c in account.characters)
            {
                characterIds.Add(c.ID);
            }           

            do
            {
                character.ID = CreateCharacterID();

                foreach (uint id in characterIds)
                {
                    if (character.ID == id)
                        idExists = true;
                }
            } while (idExists == true);
        }
        else
            character.ID = CreateCharacterID();

        GameManager.Instance.SaveCharacter(character);
        GameManager.Instance.SaveAccount(account);

        SceneManager.LoadScene("Farm");
    }

    uint CreateCharacterID()
    {
        uint id = 100000;
        uint random = (uint)Random.Range(11111, 99999);
        id += random;

        return id;
    }
}
