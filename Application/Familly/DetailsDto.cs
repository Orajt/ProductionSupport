namespace Application.Familly
{
    public class DetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ReactSelectInt> FabricGroupVariants { get; set; } = new List<ReactSelectInt>();
        public List<ReactSelectInt> Articles { get; set; } = new List<ReactSelectInt>();
        public bool AbleToDelete { get; set; }=false;
    }
}