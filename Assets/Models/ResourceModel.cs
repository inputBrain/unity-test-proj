namespace Models
{
    public class ResourceModel
    {
        public int Bronze {get; set; }

        public int Silver {get; set; }

        public int Gold {get; set; }

        public int DonatCrystal {get; set; }


        public static ResourceModel CreateEmpty()
        {
            return new ResourceModel()
            {
                Bronze = 0,
                Silver = 0,
                Gold = 0,
                DonatCrystal = 0
            };
        }
    }
}