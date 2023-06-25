using System.ComponentModel.DataAnnotations;
using Domain.Interface;

namespace Domain.Model
{
    public class Planet: BaseEntity, IEntity
    {
        public string? PhotoUrl { get; set; }
        public double DistanceFromSunKilometers { get; set; }
        public double MassTonnes { get; set; }
        public double DiameterKilometers { get; set; }
        public double LengthOfDayHours { get; set; }
    }
}