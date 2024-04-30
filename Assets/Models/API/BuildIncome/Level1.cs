using System.Collections.Generic;

namespace Models.API.BuildIncome
{
    public class Level1
    {
        public int UpgradeTime;

        public Dictionary<string, int> Cost;
        
        public Dictionary<string, int> IncomePerMinute;
    }
}