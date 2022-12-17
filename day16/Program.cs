using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var valves = File.ReadLines("input.txt")
            .Select(l => {
                var matches = Regex.Match(l, @"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? (.+)")
                    .Groups;
                var flowRate = int.Parse(matches[2].Value);
                return new Valve {
                    Name = matches[1].Value,
                    FlowRate = flowRate,
                    Neighbors = matches[3].Value.Split(", ").ToList(),
                    IsOn = flowRate == 0
                }; 
            })
            .ToDictionary(v => v.Name, v => v);

        Traverse(valves["AA"], valves, new HashSet<string> { "AA" }, 0, 30);
        Console.WriteLine(currentMax);
        currentMax = 0;
        Traverse2(valves["AA"], valves["AA"], valves, new HashSet<string> { "AA" }, 0, 26);
        Console.WriteLine(currentMax);
    }

    public static int currentMax = 0;

    public static void Traverse(
        Valve current,
        Dictionary<string, Valve> valves,
        HashSet<string> visited,
        int flow,
        int time) {
        if (time == 0 || valves.Values.All(v => v.IsOn)) {
            currentMax = Math.Max(flow, currentMax);
            return;
        }
        var bestPossible = flow + valves.Values
            .Where(v => !v.IsOn)
            .OrderByDescending(v => v.FlowRate)
            .Select((v, i) => (v, i))
            .Sum(x => Math.Max((time - 1 - x.i * 2) * x.v.FlowRate, 0));
        if(bestPossible < currentMax) {
            return;
        }
        var priorityNeighbors = current.Neighbors
            .OrderByDescending(n => visited.Contains(n) 
                ? -1 : (valves[n].IsOn ? 0 : valves[n].FlowRate))
            .ToList();
        if(!current.IsOn && time > 1) {
            current.IsOn = true;
            var addedFlow = current.FlowRate * (time - 1);
            Traverse(current, valves, visited, flow + addedFlow, time - 1);
            current.IsOn = false;
        }
        foreach(var neighbor in priorityNeighbors) {
            visited.Add(neighbor);
            Traverse(valves[neighbor], valves, visited, flow, time - 1);
            visited.Remove(neighbor);
        }
    }

    public static void Traverse2(
        Valve current,
        Valve elephant,
        Dictionary<string, Valve> valves,
        HashSet<string> visited,
        int flow,
        int time) {
        if (time == 0 || valves.Values.All(v => v.IsOn)) {
            currentMax = Math.Max(flow, currentMax);
            return;
        }
        var bestPossible = flow + valves.Values
            .Where(v => !v.IsOn)
            .OrderByDescending(v => v.FlowRate)
            .Select((v, i) => (v, i))
            .Sum(x => Math.Max((time - 1 - x.i * 2) * x.v.FlowRate, 0));
        if(bestPossible < currentMax) {
            return;
        }
        var priorityNeighbors = current.Neighbors
            .OrderByDescending(n => visited.Contains(n) 
                ? -1 : (valves[n].IsOn ? 0 : valves[n].FlowRate))
            .ToList();
        var priorityNeighborsElephant = elephant.Neighbors
            .OrderByDescending(n => visited.Contains(n) 
                ? -1 : (valves[n].IsOn ? 0 : valves[n].FlowRate))
            .ToList();
        if(!current.IsOn && time > 1) {
            current.IsOn = true;
            var addedFlow = current.FlowRate * (time - 1);
            if(!elephant.IsOn) {
                elephant.IsOn = true;
                addedFlow += elephant.FlowRate * (time - 1);
                Traverse2(current, elephant, valves, visited, flow + addedFlow, time - 1);
                elephant.IsOn = false;
            } else {
                foreach(var elephantNeighbor in priorityNeighborsElephant) {
                    visited.Add(elephantNeighbor);
                    Traverse2(current, valves[elephantNeighbor], valves, visited, flow + addedFlow, time - 1);
                    visited.Remove(elephantNeighbor);
                }
            }
            current.IsOn = false;
        }
        foreach(var neighbor in priorityNeighbors) {
            visited.Add(neighbor);
            if(!elephant.IsOn) {
                elephant.IsOn = true;
                var addedFlow = elephant.FlowRate * (time - 1);
                Traverse2(valves[neighbor], elephant, valves, visited, flow + addedFlow, time - 1);
                elephant.IsOn = false;
            } else {
                foreach(var elephantNeighbor in priorityNeighborsElephant) {
                    visited.Add(elephantNeighbor);
                    Traverse2(valves[neighbor], valves[elephantNeighbor], valves, visited, flow, time - 1);
                    visited.Remove(elephantNeighbor);
                }
            }
            visited.Remove(neighbor);
        }
    }

    public class Valve {
        public int FlowRate {get;set;}
        public List<string> Neighbors {get;set;} = default!;
        public string Name {get;set;} = default!;
        public bool IsOn {get; set;} = false;
    }
}