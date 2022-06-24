using System.Collections.Generic;

public class Player { }
public class Gun { }
public class Follower { }

public class Units
{
    public IReadOnlyCollection<Unit> Collection { get; private set; }
}
public class Unit { }