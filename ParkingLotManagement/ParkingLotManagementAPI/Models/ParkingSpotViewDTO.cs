namespace ParkingLotManagementAPI.Models
{
    public class ParkingSpotViewDTO
    {

        public int TotalSpots { get; set; }
        public int RegularSpots { get; set; }
        public int FreeRegularSpots { get; set; }
        public int OccupiedRegularSpots { get; set; }
        public int ReservedSpots { get; set; }
        public int FreeReservedSpots { get; set; }
        public int OccupiedReservedSpots { get; set; }
    }
}
