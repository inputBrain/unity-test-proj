using System;

namespace Models
{
    [Serializable]
    public class IncomeManager
    {
        public int CoinIncome;

        public static IncomeManager CreateEmpty()
        {
            return new IncomeManager()
            {
                CoinIncome = 0
            };
        }

        public static IncomeManager CreateBeginResourcesForCastle()
        {
            return new IncomeManager()
            {
                CoinIncome = 10
            };
        }
    }
}