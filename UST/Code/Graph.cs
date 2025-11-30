using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
    /// Graph contains no islands, except for when there is only one node
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

    public static string MapToString(Dictionary<int, HashSet<int>> map) {
        ArgumentNullException.ThrowIfNull(map);
        int locationsCount = map.Count;
        StringBuilder stringBuilder = new();
        for (int i = 0; i < locationsCount; i++) {
            _ = stringBuilder.Append($"{i}: {string.Join(',', map[i].ToImmutableSortedSet())}");
            _ = stringBuilder.AppendLine();
        }
        return stringBuilder.ToString().Trim();
    }

    public static List<int> GenerateTeamMap(Dictionary<int, HashSet<int>> graph, double proportionOne) {
        ArgumentNullException.ThrowIfNull(graph);
        if (proportionOne is < 0 or > 1) {
            throw new ArgumentOutOfRangeException(nameof(proportionOne), "Must be between 0 and 1");
        }

        int numLocations = graph.Count;
        int teamOneCount = (int)Math.Floor(proportionOne * (double)numLocations);
        List<int> teams = Enumerable.Repeat(-1, numLocations).ToList();
        List<int> shuffledLocations = Enumerable.Range(0, numLocations).ToList();
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(shuffledLocations));

        for (int i = 0; i < shuffledLocations.Count; i++) {
            int location = shuffledLocations[i];
            if (i < teamOneCount) {
                teams[location] = 1;
            } else {
                teams[location] = 2;
            }
        }

        return teams;
    }

    /// <summary>
    /// The returned path does not include startLocation
    /// Returns null if no path found
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="startLocation"></param>
    /// <param name="thisTeam"></param>
    /// <param name="teams"></param>
    /// <returns></returns>
    public static List<int> FindPathToNearestUnalliedLocation(Dictionary<int, HashSet<int>> graph, int startLocation, int thisTeam, List<int> teams) {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(teams);

        Queue<int> queue = new();
        queue.Enqueue(startLocation);
        Dictionary<int, int> parentMap = new() { { startLocation, -1 } };
        int targetLocation = -1;
        while (queue.Count > 0) {
            int currentLocation = queue.Dequeue();
            if (teams[currentLocation] != thisTeam) {
                targetLocation = currentLocation;
                break;
            }

            foreach (int neighbor in graph[currentLocation]) {
                if (!parentMap.ContainsKey(neighbor)) {
                    parentMap[neighbor] = currentLocation;
                    queue.Enqueue(neighbor);
                }
            }
        }

        if (targetLocation == -1) {
            return null;
        } else {
            Stack<int> stack = new();
            int current = targetLocation;
            while (current != -1) {
                stack.Push(current);
                current = parentMap[current];
            }

            _ = stack.Pop();
            return stack.ToList();
        }
    }
}
