using System;

namespace UST;

public static class RandomExtensions {
    public static double NextDouble(this Random random, double min, double max) {
        ArgumentNullException.ThrowIfNull(random);
        return random.NextDouble() * (max - min) + min;
    }
}
