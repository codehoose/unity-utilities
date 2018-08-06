using System.IO;
using UnityEngine;

/// <summary>
/// Save and Load the last played position of a sound.
/// </summary>
public class AudioSaveLoad : MonoBehaviour
{
    /// <summary>
    /// Path to the save data.
    /// </summary>
    string pathToSaveData;

    void Awake()
    {
        // Creat the path to the save data.
        pathToSaveData = Path.Combine(Application.persistentDataPath, "sound.json");
    }

    public void Reset()
    {
        // Reset the position on desk to 0. You can always delete this file too
        // if you'd prefer.
        var soundData = new SoundData();
        soundData.soundPosition = 0f;

        // Write to disk
        var json = JsonUtility.ToJson(soundData);
        File.WriteAllText(pathToSaveData, json);
    }

    public void Save()
    {
        // Create new instance of our serializble class
        var soundData = new SoundData();

        // Set the current time of the sound clip
        soundData.soundPosition = GetComponent<AudioSource>().time;

        // Write to disk
        var json = JsonUtility.ToJson(soundData);
        File.WriteAllText(pathToSaveData, json);
    }

	void Start ()
    {
        var audioSource = GetComponent<AudioSource>();

        // Check to see there is save data
        if (File.Exists(pathToSaveData))
        {
            // If there is, we load it into memory (it's gonna be JSON :))
            var json = File.ReadAllText(pathToSaveData);
            var soundData = JsonUtility.FromJson<SoundData>(json);

            // Apply position
            audioSource.time = soundData.soundPosition;
        }
        
        // Play the sound
        audioSource.Play();
	}
}
