using System;
using System.Collections.Generic;
using System.Linq;

namespace UST;

public class UnitAutomaton {
    public UnitRow UnitRow { get; set; }
    public SaveData SaveData { get; set; }

    public void Process(double delta) {
        /*
        Unit automaton

        if enemy unit in current location:
            Attack enemy unit
            Clear any movement progress
        else:
            if movement in progress:
                progress movement
                check if enough progress made to move to next location
            else:
                Find the path to the closest enemy unit
        */

        int currentLocation = this.UnitRow.Location;
        List<UnitRow> enemyUnits = this.SaveData.UnitLocations[currentLocation].Where(x => x.Location != currentLocation).ToList();
        if (enemyUnits.Count > 0) {
            UnitRow enemyUnit = enemyUnits[Random.Shared.Next(enemyUnits.Count)];
            enemyUnit.Health -= delta * this.UnitRow.Attack;
            if (enemyUnit.Health <= 0) {
                enemyUnit.Alive = false;
            }
        } else {
            // Find the path to the nearest enemy
            // Get the first step of that path
            // If this matches the current target, then make progress
            // Else, restart progress towards new target
        }
    }
}
