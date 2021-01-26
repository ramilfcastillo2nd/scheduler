namespace Core.Dtos.Dashboards.Output
{
    public class GetDashboardStatsOutputDto
    {
        public int Invites { get; set; } = 0;
        public int Views { get; set; } = 0;
        public int Messages { get; set; } = 0;
        public int InMails { get; set; } = 0;
        public int Emails { get; set; } = 0;
        public int Connected { get; set; } = 0;
        public int FollowUps { get; set; } = 0;
        public int Greetings { get; set; } = 0;
        public int Withdrawals { get; set; } = 0;
    }
}
