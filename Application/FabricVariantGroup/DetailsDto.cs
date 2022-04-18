namespace Application.FabricVariantGroup
{
    public class DetailsDtoFabricVariant
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public int PlaceInGroup { get; set; }
    }
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DetailsDtoFabricVariant> FabricVariants { get; set; } = new List<DetailsDtoFabricVariant>();
    }
}