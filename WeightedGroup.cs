using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedGroup<T> : Dictionary<T, double> where T: class
{
    public T GetItem()
    {
        double totalWeight = 0;
        foreach(var item in Values)
        {
            var absValue = Math.Abs(item);
            totalWeight += absValue;
        }
        if(totalWeight <=0)
        {
            return null;
        }

        double roll = new Random().NextDouble();
        double total = 0;

        foreach(KeyValuePair<T, double> kvp in this.ToList().OrderByDescending(v => v.Value))
        {
            total += kvp.Value /totalWeight;
            if(roll <= total)
            {
                return kvp.Key;
            }
        }

        return Keys.FirstOrDefault();
    }
}

