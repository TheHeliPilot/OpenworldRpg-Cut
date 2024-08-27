using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New PartyMember", menuName = "Fight/New PartyMember")]
    public class PartyMemberScriptableObject : ScriptableObject
    {
        public Speaker speaker;
        public int[] health;
        public StatPair[] baseSkills = new StatPair[6];
        public InventoryItem mainHand;
        public InventoryItem offHand;
        public int criticalFailPercentage;
    }
}
