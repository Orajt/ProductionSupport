using System.Text.Json.Serialization;

namespace Application.OrderPosition
{
    public class PositionDto
    {
        public long Id { get; set; }
        public int Lp { get; set; }
        public int ArticleId{get;set;}
        public int ArticleTypeId{get;set;}
        public string ArticleName { get; set; }
        public int Quanity { get; set; }
        public string Realization { get; set; }
        public string Client { get; set; }
        public int SetId { get; set; }
        [JsonIgnore]
        public bool SetIdFromDB{get;set;}=false;
        [JsonIgnore]
        public int? IndexOfSetList{get;set;}=null;

    }
}