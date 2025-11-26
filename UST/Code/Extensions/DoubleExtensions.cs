using System;
using System.Collections.Generic;
using System.Linq;

namespace UST;

public static class DoubleExtensions {
    public static List<double> Split(this double value, int n) {
        if (n <= 0) { throw new ArgumentException("n must be > 0"); }
        if (value < 0) { throw new ArgumentException("value must be >= 0"); }

        // Generate n−1 random breakpoints
        double[] points = new double[n + 1];
        points[0] = 0;
        points[n] = 1;

        for (int i = 1; i < n; i++) {
            points[i] = Random.Shared.NextDouble();
        }

        Array.Sort(points);

        // Differences give splits that sum to 1
        double[] splits = new double[n];
        for (int i = 0; i < n; i++) {
            splits[i] = points[i + 1] - points[i];
        }

        // Scale to the magnitude of value
        return splits.Select(p => p * value).ToList();
    }
}
