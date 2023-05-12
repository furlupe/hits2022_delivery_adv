namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public abstract class NamedEntity
    {
        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NormalizedName = value.ToUpper().Normalize();
            }
        }
        public string NormalizedName { get; set; }
    }
}
