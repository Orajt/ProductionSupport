using System.Text.Json.Serialization;

namespace Application.OrderPosition
{
    public class PositionRealization
    {
        public int Id { get; set; }
        public int FabricId { get; set; }
        public int PlaceInGroup { get; set; }
    }
    public class PositionDto
    {
        public long Id { get; set; }
        public int Lp { get; set; }
        public int ArticleId { get; set; }
        public int ArticleTypeId { get; set; }
        public string ArticleName { get; set; }
        public int Quanity { get; set; }
        public string Realization { get; set; }
        public string Client { get; set; }
        public List<PositionRealization> FabricRealization { get; set; } = new List<PositionRealization>();
        public int SetId { get; set; }
        [JsonIgnore]
        public bool SetIdFromDB { get; set; } = false;
        [JsonIgnore]
        public int? IndexOfSetList { get; set; } = null;

    }
}