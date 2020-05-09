using System.Collections.Generic;

public class Relative
{
    public string days { get; set; }
    public string hours { get; set; }
    public string minutes { get; set; }
}

public class GravityLastUpdated
{
    public bool file_exists { get; set; }
    public int absolute { get; set; }
    public Relative relative { get; set; }
}

public class Summary
{
    public int domains_being_blocked { get; set; }
    public int dns_queries_today { get; set; }
    public int ads_blocked_today { get; set; }
    public double ads_percentage_today { get; set; }
    public int unique_domains { get; set; }
    public int queries_forwarded { get; set; }
    public int queries_cached { get; set; }
    public int clients_ever_seen { get; set; }
    public int unique_clients { get; set; }
    public int dns_queries_all_types { get; set; }
    public int reply_NODATA { get; set; }
    public int reply_NXDOMAIN { get; set; }
    public int reply_CNAME { get; set; }
    public int reply_IP { get; set; }
    public int privacy_level { get; set; }
    public string status { get; set; }
    public GravityLastUpdated gravity_last_updated { get; set; }
    public List<string> top_sources { get; set; }
    public List<string> top_sources_blocked { get; set; }

    public Summary()
    {
    }
}