using UST.Objects.Interfaces;
using UST.Spatial;

namespace UST;

enum UnitState {
    Idle,
    Move,
    Attack,
}

public class UnitAutomaton {
    private IUnit Unit;
    private UnitState State;
    private Location Location;

    private void ProcessFrame() {
        if (this.State == UnitState.Idle) {
            // Find the nearest unallied location
        } else if (this.State == UnitState.Move) {
            // If at goal location, then move to attack
            // Else: continue moving to goal location
        } else if (this.State == UnitState.Attack) {
            // If location is allied, then move to idle
            // Else: attack, decapture, and capture
        }
    }
}
