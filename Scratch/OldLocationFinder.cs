        // Move units
        foreach (Unit unit in this.Units) {
            if (unit.PreviousLocation == -1) {
                unit.MoveProgress = 0;
                unit.PreviousLocation = unit.Location;
                unit.TargetLocation = this.Map[unit.Location].ToList().GetRandom();
            }

            double remainingMoveDistance = delta * unit.MoveSpeed;
            while (remainingMoveDistance > 0) {
                double distanceToTarget = EdgeDistance - unit.MoveProgress;
                if (remainingMoveDistance >= distanceToTarget) {
                    unit.MoveProgress = 0;
                    unit.PreviousLocation = unit.Location;
                    List<int> candidateNeighbors = this.Map[unit.Location].Where(x => x != unit.PreviousLocation).ToList();
                    if (candidateNeighbors.Count == 0) {
                        unit.TargetLocation = unit.PreviousLocation;
                    } else {
                        unit.TargetLocation = candidateNeighbors.GetRandom();
                    }
                    unit.Location = unit.TargetLocation;
                    remainingMoveDistance -= distanceToTarget;
                } else {
                    unit.MoveProgress += remainingMoveDistance;
                    remainingMoveDistance = 0;
                }
            }
        }
