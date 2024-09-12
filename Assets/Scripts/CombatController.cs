using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour //Placed in the combat scene
{
    private int expReward; //calculated by calling the getExpReward method from enemies' character sheets
    private List<CharacterSheet> enemies;
    
    // Start is called before the first frame update

    void Start()
    {
        //generate enemy list
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Current damage formula:((((((2 * Level) / 5) + 2) * P * (A/D)) / 50 ) + 2) * crit * other modifiers

    //Considerations: How to balance spread moves? lower damage? Higher cost?

    //certain field or status effects may need to applied here in some way

    /*
    private void determineTurnOrder() {
        
    }
    */
}
