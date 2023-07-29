using UnityEngine;

[CreateAssetMenu(fileName = "New Level Select", menuName = "ScriptableObjects/Cards/Level")]
public class LevelSelectCardScriptableObject : CardScriptableObject
{
    // Needs some sort of difficulty scalar
    public string levelName;
    public string sceneToLoad;
    public Sprite levelThumbnail;
}
