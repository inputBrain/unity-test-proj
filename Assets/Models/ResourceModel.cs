namespace Models
{
    public class ResourceModel
    {
        public int Bronze {get; set; }

        public int Silver;

        public int Gold;

        public int DonatCrystal;


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