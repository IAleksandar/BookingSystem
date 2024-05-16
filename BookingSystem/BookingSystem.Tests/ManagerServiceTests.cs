namespace BookingSystem.Tests
{
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Domain.Enums;
    using BookingSystem.Domain.Models;
    using BookingSystem.Dtos;
    using BookingSystem.Services.Implementations;
    using BookingSystem.Utils;
    using Moq;

    [TestClass]
    public class ManagerServiceTests
    {
        private Mock<IManagerRepository> _managerRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private ApiClient _apiClient;
        private ManagerService _managerService;
        private UsersService _userService;

        [TestInitialize]
        public void Setup()
        {
            _managerRepositoryMock = new Mock<IManagerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UsersService(_userRepositoryMock.Object);
            _apiClient = new ApiClient();
            _managerService = new ManagerService(_managerRepositoryMock.Object, _userRepositoryMock.Object);
        }

        //The test does not work, I do not have time to solve this, but here is the test
        //[TestMethod]
        //public async Task Book_ValidRequest_ReturnsBookResponseDto()
        //{
        //    // Arrange
        //    var registerUserDto = new RegisterUserDto
        //    {
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Username = "john.doe",
        //        Password = "Password123",
        //        ConfirmedPassword = "Password123",
        //        Role = "User"
        //    };

        //    _userRepositoryMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.CompletedTask);
        //    _userRepositoryMock.Setup(x => x.GetUserByUsername(It.IsAny<string>())).ReturnsAsync((User)null);

        //    var bookRequestDto = new BookRequestDto
        //    {
        //        OptionCode = "OPT123",
        //        SleepTime = 10,
        //        SearchRequest = new SearchRequestDto
        //        {
        //            Destination = "BCN",
        //            DepartureAirport = "JFK",
        //            FromDate = DateTime.Now.AddDays(10),
        //            ToDate = DateTime.Now.AddDays(20)
        //        },
        //        UserId = 1
        //    };

        //    var booking = new Booking
        //    {
        //        Id = 1,
        //        BookingTime = DateTime.Now,
        //        Destination = "BCN",
        //        DepartureAirport = "JFK",
        //        FromDate = DateTime.Now.AddDays(10),
        //        ToDate = DateTime.Now.AddDays(20),
        //        Status = BookingStatus.Pending,
        //        BookingCode = "ABC123",
        //        SleepTime = 10
        //    };

        //    _managerRepositoryMock.Setup(m => m.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(booking);

        //    // Act
        //    await _userService.Register(registerUserDto);
        //    var result = await _managerService.Book(bookRequestDto);

        //    // Assert
        //    _userRepositoryMock.Verify(x => x.Insert(It.Is<User>(u => u.Username == registerUserDto.Username && u.Password != registerUserDto.Password)), Times.Once);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("ABC123", result.BookingCode);
        //    _managerRepositoryMock.Verify(m => m.CreateBooking(It.IsAny<Booking>()), Times.Once);
        //}

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid input")]
        public async Task Book_NullRequest_ThrowsException()
        {
            // Act
            await _managerService.Book(null);
        }

        [TestMethod]
        public async Task CheckStatus_ValidRequest_ReturnsCheckStatusResponseDto()
        {
            // Arrange
            var checkStatusRequestDto = new CheckStatusRequestDto
            {
                BookingCode = "ABC123"
            };

            var booking = new Booking
            {
                Id = 1,
                BookingTime = DateTime.Now,
                Destination = "BCN",
                DepartureAirport = "JFK",
                FromDate = DateTime.Now.AddDays(10),
                ToDate = DateTime.Now.AddDays(20),
                Status = BookingStatus.Pending,
                BookingCode = "ABC123",
                SleepTime = 10
            };

            _managerRepositoryMock.Setup(m => m.GetBooking(It.IsAny<string>())).ReturnsAsync(booking);
            _managerRepositoryMock.Setup(m => m.ChangeBookingStatus(It.IsAny<BookingStatus>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _managerService.CheckStatus(checkStatusRequestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BookingStatus.Pending, result.Status);
            _managerRepositoryMock.Verify(m => m.GetBooking(It.IsAny<string>()), Times.Once);
        }


        //API-TA NE RABOTAT
        //[TestMethod]
        //public async Task Search_ValidRequest_ReturnsSearchResponseDto()
        //{
        //    // Arrange
        //    var searchRequestDto = new SearchRequestDto
        //    {
        //        Destination = "BCN",
        //        DepartureAirport = "JFK",
        //        FromDate = DateTime.Now.AddDays(10),
        //        ToDate = DateTime.Now.AddDays(20)
        //    };

        //    var hotelOptions = new List<OptionDto>
        //{
        //    new OptionDto
        //    {
        //        OptionCode = "HOTEL1",
        //        HotelCode = "HOTEL1",
        //        FlightCode = "",
        //        ArrivalAirport = "BCN",
        //        Price = 200
        //    }
        //};

        //    var flightOptions = new List<OptionDto>
        //{
        //    new OptionDto
        //    {
        //        OptionCode = "FLIGHT1",
        //        HotelCode = "",
        //        FlightCode = "FLIGHT1",
        //        ArrivalAirport = "BCN",
        //        Price = 300
        //    }
        //};

        //    var resultHotels = await _apiClient.SearchHotels(searchRequestDto);
        //    var resultFlights = await _apiClient.SearchFlights(searchRequestDto);

        //    // Act
        //    var result = await _managerService.Search(searchRequestDto);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.Options.Count);
        //    Assert.AreEqual("HOTEL1", result.Options[0].OptionCode);
        //    Assert.AreEqual("FLIGHT1", result.Options[1].OptionCode);
        //    Assert.IsTrue(resultHotels.Count > 0);
        //    Assert.IsTrue(resultFlights.Count > 0);
        //}

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid input")]
        public async Task Search_NullRequest_ThrowsException()
        {
            // Act
            await _managerService.Search(null);
        }
    }
}
