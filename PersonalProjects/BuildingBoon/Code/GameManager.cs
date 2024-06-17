using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    IDataService dataService = new JsonDataService();

    public Account account;
    public Character character;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        if (_instance == this)
            DontDestroyOnLoad(this.gameObject);
    }

    public void SaveAccount(Account account)
    {
        if (dataService.SaveData("/account.json", account, false)) { }
        else
            Debug.LogError("Could not save file. There should be an indication on the UI of this failure.");
    }

    public void SaveCharacter(Character character)
    {
        // Get the saved account
        Account account = LoadAccount();

        // Update the character in the account's list of characters, or add the new character
        bool characterExists = false;
        if (account.characters.Count > 0)
        {
            for (int i = account.characters.Count - 1; i >= 0; i--)
            {
                if (account.characters[i].ID == character.ID)
                {
                    account.characters.Remove(account.characters[i]);
                    account.characters.Insert(i, character);
                    characterExists = true; 
                }
            }
        }

        if (!characterExists)
            account.characters.Add(character);

        // Save the account with the new/updated character
        if (dataService.SaveData("/account.json", account, false)) { }
        else
            Debug.LogError("Could not save file. There should be an indication on the UI of this failure.");
    }

    public Account LoadAccount()
    {
        return dataService.LoadData<Account>("/account.json", false);
    }

    public Character LoadCharacter()
    {
        return new Character(); // placeholder, fix this function later

        //return dataService.LoadData<Character>("/characters.json", false);
    }

    public bool FoundFile<T>(string path)
    {
        return dataService.FoundFile<T>(path);
    }
}