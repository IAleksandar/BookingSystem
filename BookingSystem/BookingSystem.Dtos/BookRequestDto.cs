namespace BookingSystem.Dtos
{
    public class BookRequestDto
    {
        public string OptionCode { get; set; } = string.Empty;

        public int SleepTime { get; set; }

        public SearchRequestDto SearchRequest { get; set; }

        public int UserId { get; set; }
    }
}
