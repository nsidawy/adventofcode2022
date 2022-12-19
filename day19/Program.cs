using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var robots = (1, 0, 0, 0);
        var blueprints = File.ReadLines("test.txt")
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
        
        //var result = 0;
        //foreach(var b in blueprints) {
        //    result += Simulate(24, robots, (0, 0, 0, 0), b,
        //        new Dictionary<(int, (int, int, int, int), (int, int, int, int)), int>()) * b.Id;
        //    Console.WriteLine(result);
        //}
        //Console.WriteLine(result);
        var result = 1;
        foreach(var b in blueprints.Take(3)) {
            result *= Simulate(32, robots, (0, 0, 0, 0), b,
                new Dictionary<(int, (int, int, int, int), (int, int, int, int)), int>()) * b.Id;
            Console.WriteLine(result);
        }
        Console.WriteLine(result);
    }

    private static int Simulate(
            int time,
            (int, int, int, int) robots,
            (int, int, int, int) resources,
            Blueprint blueprint,
            Dictionary<(int, (int, int, int, int), (int, int, int, int)), int> memoize) 
        {
        if(time == 0) {
            return resources.Item4;
        }
        if(memoize.ContainsKey((time, robots, resources))) {
            return memoize[(time, robots, resources)];
        }
        var canCreateOreRobot = resources.Item1 >= blueprint.OreRobotCost
            && !(robots.Item1 >= blueprint.MaxOreCost);
        var canCreateClayRobot = resources.Item1 >= blueprint.ClayRobotCost
            && !(robots.Item2 >= blueprint.ObsidianRobotClayCost);
        var canCreateObsidianRobot = resources.Item1 >= blueprint.ObsidianRobotOreCost
            && resources.Item2 >= blueprint.ObsidianRobotClayCost
            && !(robots.Item3 >= blueprint.GeodeRobotObsidianCost);
        var canCreateGeodeRobot = resources.Item1 >= blueprint.GeodeRobotOreCost
            && resources.Item3 >= blueprint.GeodeRobotObsidianCost;
        resources.Item1 += robots.Item1;
        resources.Item2 += robots.Item2;
        resources.Item3 += robots.Item3;
        resources.Item4 += robots.Item4;

        var results = new List<int>();
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
        results.Add(Simulate(time - 1, robots, resources, blueprint, memoize));
        memoize[(time, robots, resources)] = results.Max();
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