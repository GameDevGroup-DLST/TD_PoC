public class LevelSelectCard : Card
{
    private string sceneToLoad;

    public override void Ininitialize() {
        cardImage.sprite = ((LevelSelectCardScriptableObject)cardData).levelThumbnail;
        sceneToLoad = ((LevelSelectCardScriptableObject)cardData).sceneToLoad;
    }

    public override void HandleSelect() {
        GameManager.Instance.ChangeState(GameState.Gameplay);
    }
}
