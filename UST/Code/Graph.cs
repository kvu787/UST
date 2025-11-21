using System;
using System.Collections.Generic;

namespace UST;

/*
Graph generation approaches
- 2 islands and then add random paths between them
- Rectangular grid graph with dimensions x by y
- Hex grid graph with side length of n
- Straight line
- Circle
- Concentric circles
- Fully connected graph
*/

public static class Graph {
    /// <summary>
    /// Generates a randomly connected graph (vertices and edges)
    /// The graph is undirected
    /// Graph contains no islands
    /// The edges from a vertex to other vertices are randomly chosen
    /// Vertices are not allowed to have edges to self
    /// Vertices are not allowed to have more than one edge to another vertex
    /// </summary>
    /// <param name="numLocations"></param>
    /// <param name="probabilityAddEdge"></param>
    /// <returns></returns>
    public static Dictionary<int, HashSet<int>> GenerateMap(int numLocations, double probabilityAddEdge) {
        if (numLocations < 1) {
            throw new ArgumentOutOfRangeException(nameof(numLocations), "Must have at least 1 location");
        }

        if (probabilityAddEdge is < 0 or > 1) {
            throw new ArgumentOutOfRangeException(nameof(probabilityAddEdge), "Must be between 0 and 1");
        }

        Random random = Random.Shared;
        HashSet<(int, int)> edges = [];

        for (int i = 1; i < numLocations; i++) {
            int connectedLocation = random.Next(i);
            (int, int) newEdge = (connectedLocation, i);
            _ = edges.Add(newEdge);
        }

        for (int i = 0; i < numLocations; i++) {
            for (int j = i + 1; j < numLocations; j++) {
                if (random.NextDouble() < probabilityAddEdge) {
                    _ = edges.Add((i, j));
                }
            }
        }

        Dictionary<int, HashSet<int>> graph = [];
        for (int i = 0; i < numLocations; i++) {
            graph[i] = [];
        }

        foreach ((int from, int to) in edges) {
            _ = graph[from].Add(to);
            _ = graph[to].Add(from);
        }

        return graph;
    }
}
