using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var robots = (1, 0, 0, 0);
        var blueprints = File.ReadLines("input.txt")
            .Select(l => {
                var values = Regex.Matches(l, @"\d+").Select(r => int.Parse(r.Value)).ToArray();
                return new Blueprint {
                    Id = values[0],
                    OreRobotCost = values[1],
                    ClayRobotCost = values[2],
                    ObsidianRobotOreCost = values[3],
                    ObsidianRobotClayCost = values[4],
                    GeodeRobotOreCost = values[5],
                    GeodeRobotObsidianCost = values[6],
                };
            })
            .ToArray();
        
        var result = 0;
        foreach(var b in blueprints) {
            result += Simulate(24, robots, (0, 0, 0, 0), b,
                new Dictionary<int, (int, int)>()) * b.Id;
            Console.WriteLine(result);
        }
        Console.WriteLine(result);
        result = 1;
        foreach(var b in blueprints.Take(3)) {
            result *= Simulate(32, robots, (0, 0, 0, 0), b,
                new Dictionary<int, (int, int)>());
            Console.WriteLine(result);
        }
        Console.WriteLine(result);
    }

    private static int Simulate(
            int time,
            (int, int, int, int) robots,
            (int, int, int, int) resources,
            Blueprint blueprint,
            Dictionary<int, (int, int)> memoize) 
        {
        if(time == 0) {
            return resources.Item4;
        }
        var value = resources.Item4 * 1000000 + resources.Item3 * 10000 + resources.Item2 * 100 + resources.Item1;
        if(memoize.ContainsKey(time)) {
            var (bestValue, geodeCount) = memoize[time];
            if (value / 1000000 < bestValue / 1000000) {
                return geodeCount;
            }
        }
        var canCreateOreRobot = resources.Item1 >= blueprint.OreRobotCost
            && !(robots.Item1 >= blueprint.MaxOreCost)
            && !(resources.Item1 > 20);
        var canCreateClayRobot = resources.Item1 >= blueprint.ClayRobotCost
            && !(robots.Item2 >= blueprint.ObsidianRobotClayCost)
            && !(resources.Item2 > 20);
        var canCreateObsidianRobot = resources.Item1 >= blueprint.ObsidianRobotOreCost
            && resources.Item2 >= blueprint.ObsidianRobotClayCost
            && !(robots.Item3 >= blueprint.GeodeRobotObsidianCost)
            && !(resources.Item3 > 20);
        var canCreateGeodeRobot = resources.Item1 >= blueprint.GeodeRobotOreCost
            && resources.Item3 >= blueprint.GeodeRobotObsidianCost;
        resources.Item1 += robots.Item1;
        resources.Item2 += robots.Item2;
        resources.Item3 += robots.Item3;
        resources.Item4 += robots.Item4;

        var results = new List<int>();
        if(!(robots.Item2 == 0 && canCreateOreRobot && canCreateClayRobot)
            && !(robots.Item3 == 0 && canCreateOreRobot && canCreateClayRobot && canCreateObsidianRobot)
            && !(canCreateOreRobot && canCreateClayRobot && canCreateObsidianRobot && canCreateGeodeRobot))
        {
            results.Add(Simulate(time - 1, robots, resources, blueprint, memoize));
        }
        if(canCreateGeodeRobot) {
            results.Add(Simulate(
                time -1 ,
                (robots.Item1, robots.Item2, robots.Item3, robots.Item4 + 1),
                (resources.Item1 - blueprint.GeodeRobotOreCost, resources.Item2, resources.Item3 - blueprint.GeodeRobotObsidianCost, resources.Item4),
                blueprint, memoize));
        }
        if(canCreateObsidianRobot) {
            results.Add(Simulate(
                time - 1,
                (robots.Item1, robots.Item2, robots.Item3 + 1, robots.Item4),
                (resources.Item1 - blueprint.ObsidianRobotOreCost, resources.Item2 - blueprint.ObsidianRobotClayCost, resources.Item3, resources.Item4),
                blueprint, memoize));
        }
        if(canCreateClayRobot) {
            results.Add(Simulate(
                time - 1,
                (robots.Item1, robots.Item2 + 1, robots.Item3, robots.Item4),
                (resources.Item1 - blueprint.ClayRobotCost, resources.Item2, resources.Item3, resources.Item4),
                blueprint, memoize));
        }
        if(canCreateOreRobot) {
            results.Add(Simulate(
                time - 1,
                (robots.Item1 + 1, robots.Item2, robots.Item3, robots.Item4),
                (resources.Item1 - blueprint.OreRobotCost, resources.Item2, resources.Item3, resources.Item4),
                blueprint, memoize));
        }
        memoize[time] = (value, results.Max());
        return results.Max();
    }

    private class Blueprint {
        public int Id {get;set;}
        public int OreRobotCost {get;set;}
        public int ClayRobotCost {get;set;}
        public int ObsidianRobotOreCost {get;set;}
        public int ObsidianRobotClayCost {get;set;}
        public int GeodeRobotOreCost {get;set;}
        public int GeodeRobotObsidianCost {get;set;}
        public int MaxOreCost => new List<int> {this.OreRobotCost, this.ClayRobotCost, this.GeodeRobotOreCost, this.ObsidianRobotOreCost}.Max();
    }
}