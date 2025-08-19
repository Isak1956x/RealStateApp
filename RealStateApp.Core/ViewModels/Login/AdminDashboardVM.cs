namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class AdminDashboardVM
    {
        public int AvailableProperties { get; set; }
        public int SoldProperties { get; set; }

        public int ActiveAgents { get; set; }
        public int InactiveAgents { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
        public int ActiveDevelopers { get; set; }
        public int InactiveDevelopers { get; set; }

    }
}
