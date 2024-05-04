using System.Collections.Generic;

namespace Models.API.BuildIncome
{
    public class BuildLevel
    {
        public int Level;
        
        public int UpgradeTime;

        public Dictionary<string, int> Cost;
        
        public Dictionary<string, int> IncomePerMinute;
    }
}