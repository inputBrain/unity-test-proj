using System;

namespace Models
{
    [Serializable]
    public class ResourceModel
    {
        public int CoinIncome;

        public static ResourceModel CreateEmpty()
        {
            return new ResourceModel()
            {
                CoinIncome = 0
            };
        }

        public static ResourceModel CreateBeginResourcesForCastle()
        {
            return new ResourceModel()
            {
                CoinIncome = 10
            };
        }
    }
}