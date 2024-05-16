namespace BookingSystem.Utils
{
    using BookingSystem.Dtos;
    using Newtonsoft.Json;

    public class HotelResponse
    {
        public int Id { get; set; }

        public int HotelCode { get; set; }

        public int HotelName { get; set; }

        public string DestinationCode { get; set; }

        public string City { get; set; }
    }

    public class FlightResponse
    {
        public int FlightCode { get; set; }

        public string FlightNumber { get; set; }

        public string DepartureAirport { get; set; }

        public string ArrivalAirport { get; set; }
    }

    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<OptionDto>> SearchHotels(SearchRequestDto searchRequestDto)
        {
            string url = $"https://tripx-testfunctions.azurewebsites.net/api/SearchHotels?destinationCode={searchRequestDto.Destination}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<HotelResponse> searchRes = JsonConvert.DeserializeObject<List<HotelResponse>>(jsonResponse);

                var result = searchRes.Select(x => ToOptionHotel(x)).ToList();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve hotels. Status code: {response.StatusCode}");
            }
        }

        public async Task<List<OptionDto>> SearchFlights(SearchRequestDto searchRequestDto)
        {
            string url = $"https://tripx-testfunctions.azurewebsites.net/api/SearchFlights?departureAirport={searchRequestDto.DepartureAirport}&arrivalAirport={searchRequestDto.Destination}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<FlightResponse> searchRes = JsonConvert.DeserializeObject<List<FlightResponse>>(jsonResponse);

                var result = searchRes.Select(x => ToOptionFlight(x)).ToList();

                return result;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve flights. Status code: {response.StatusCode}");
            }
        }


        private static OptionDto ToOptionHotel(HotelResponse hotelResponse)
        {
            return new OptionDto()
            {
                HotelCode = hotelResponse.HotelCode.ToString()
            };
        }

        private static OptionDto ToOptionFlight(FlightResponse flightResponse)
        {
            return new OptionDto()
            {
                ArrivalAirport = flightResponse.ArrivalAirport,
                FlightCode = flightResponse.FlightCode.ToString()
            };

        }
    }
}
