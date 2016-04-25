public class Rootobject
{
    public string type { get; set; }
    public Crs crs { get; set; }
    public float[][] coordinates { get; set; }
    public Properties1 properties { get; set; }
}

public class Crs
{
    public string type { get; set; }
    public Properties properties { get; set; }
}

public class Properties
{
    public string name { get; set; }
}

public class Properties1
{
    public string distance { get; set; }
    public string description { get; set; }
    public string traveltime { get; set; }
}
