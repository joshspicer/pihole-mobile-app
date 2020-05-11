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

    public Summary()
    {
    }
}