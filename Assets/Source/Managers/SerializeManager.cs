using System;
using UnityEditor;
using UnityEngine;

public static class SerializeManager
{
    private static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private static float GetFloat(string key, float value)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        else
        {
            PlayerPrefs.SetFloat(key, value);
            return value;
        }
    }

    private static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    private static int GetInt(string key, int value)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
        {
            PlayerPrefs.SetInt(key, value);
            return value;
        }
    }

    private static bool GetBool(string key, bool value)
    {
        if (PlayerPrefs.HasKey(key))
            return Convert.ToBoolean(PlayerPrefs.GetInt(key));
        else
        {
            PlayerPrefs.SetInt(key, Convert.ToInt32(value));
            return value;
        }
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    private static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    private static string GetString(string key, string value)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        else
        {
            PlayerPrefs.SetString(key, value);
            return value;
        }
    }

    public static bool GetNewGameState()
    {
        return GetBool("new_game", true);
    }

    public static void SetNewGameState(bool value)
    {
        SetBool("new_game", value);
    }

    public static bool GetStructureLockedState(string ID)
    {
        return GetBool("structure_locked_state_ID_" + ID, true);
    }

    public static void SetStructureLockedState(string ID, bool value)
    {
        SetBool("structure_locked_state_ID_" + ID, value);
    }

    public static float GetTileGenOffset()
    {
        return GetFloat("tile_gen_offset", 0f);
    }

    public static void SetTileGenOffset(float value)
    {
        SetFloat("tile_gen_offset", value);
    }

    public static float GetCurrencyUnits()
    {
        return GetFloat("currency_points", 0f);
    }

    public static void SetCurrencyUnits(float value)
    {
        SetFloat("currency_points", value);
    }

    public static float GetResearchUnits()
    {
        return GetFloat("research_points", 0f);
    }

    public static void SetResearchUnits(float value)
    {
        SetFloat("research_points", value);
    }

    public static float GetMasterVolume()
    {
        return GetFloat("master_volume", 1f);
    }

    public static void SetMasterVolume(float value)
    {
        SetFloat("master_volume", value);
    }

    public static float GetMusicVolume()
    {
        return GetFloat("music_volume", 1f);
    }

    public static void SetMusicVolume(float value)
    {
        SetFloat("music_volume", value);
    }

    public static float GetSFXVolume()
    {
        return GetFloat("SFX_volume", 1f);
    }

    public static void SetSFXVolume(float value)
    {
        SetFloat("SFX_volume", value);
    }

    public static string GetStructureID(Vector2 gridPosition)
    {
        return GetString("structure_ID_at_grid_position_x" + gridPosition.x + "_y" + gridPosition.y, "none");
    }

    public static void SetStructureID(Vector2 gridPosition, string ID)
    {
        SetString("structure_ID_at_grid_position_x" + gridPosition.x + "_y" + gridPosition.y, ID);
    }
}