public class UpgradeCard : Card
{
    public override void Ininitialize() {

    }

    public override void HandleSelect() {
        PlayPhaseManager.Instance.ChangePhase(PlayPhase.None);
        GameManager.Instance.ChangeState(GameState.LevelSelect);
    }
}
