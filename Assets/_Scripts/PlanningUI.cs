using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUI : MonoBehaviour
{
    public void BeginDefencePhase() {
        PlayPhaseManager.Instance.ChangePhase(PlayPhase.Defence);
    }
}
