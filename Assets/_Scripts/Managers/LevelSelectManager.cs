using UnityEngine;

public class LevelSelectManager : StaticInstance<LevelSelectManager>
{
    [SerializeField] private LevelPicker levelSelectUIPrefab;
    private LevelPicker levelSelectInstance = null;

    private Transform mainUIContainer = null;

    protected override void Awake() {
        base.Awake();

        GameManager.OnAfterStateChanged += HandleStateChange;
    }

    private void HandleStateChange(GameState state)
    {
        if(state == GameState.LevelSelect) {
            if(!levelSelectInstance) {
                if(!mainUIContainer) {
                    mainUIContainer = GameObject.FindGameObjectWithTag("UIContainer").transform;
                    if(!mainUIContainer) throw new System.Exception("Could Not Find UI Container");
                }
                levelSelectInstance = Instantiate(levelSelectUIPrefab, mainUIContainer);
            }
        } else {
            if(levelSelectInstance) {
                Object.Destroy(levelSelectInstance.gameObject);
                levelSelectInstance = null;
            }
        }
    }
}
