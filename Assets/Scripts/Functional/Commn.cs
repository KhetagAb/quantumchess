using System.Collections;
using System.Collections.Generic;

public class Commn {
    public static void AddLocation<T>(T gridPoint, List<T> locations) {
        if (!locations.Contains(gridPoint))
            locations.Add(gridPoint);
    }
}
