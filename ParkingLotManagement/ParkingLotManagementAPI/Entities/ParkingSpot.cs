namespace ParkingLotManagementAPI.Entities
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int TotalSpots { get; set; }
        public int FreeSpots { get; set; }
        public int ReservedSpots { get; set; }
        
    }
}
