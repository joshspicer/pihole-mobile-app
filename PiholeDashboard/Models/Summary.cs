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
    public string domains_being_blocked { get; set; }
    public string dns_queries_today { get; set; }
    public string ads_blocked_today { get; set; }
    public string ads_percentage_today { get; set; }
    public string unique_domains { get; set; }
    public string queries_forwarded { get; set; }
    public string queries_cached { get; set; }
    public string clients_ever_seen { get; set; }
    public string unique_clients { get; set; }
    public string dns_queries_all_types { get; set; }
    public string reply_NODATA { get; set; }
    public string reply_NXDOMAIN { get; set; }
    public string reply_CNAME { get; set; }
    public string reply_IP { get; set; }
    public string privacy_level { get; set; }
    public string status { get; set; }
    public GravityLastUpdated gravity_last_updated { get; set; }

    public Summary()
    {
        dns_queries_all_types = "...";
    }
}