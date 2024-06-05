using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.SeedData
{
    public interface IDataaseInitialiser
    {
        Task InitialiseAsync();

        Task SeedAsync();

        Task TrySeedAsync();
    }

    public class DatabaseInitialiser : IDataaseInitialiser
    {
        public readonly TicketBookingDbContext _context;

        public DatabaseInitialiser(TicketBookingDbContext context)
        {
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                // Migration Database - Create database if it does not exist
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (_context.UserRoles.Any())
            {
                return;
            }
            var admin = new UserRole { RoleName = "Admin" };
            var staff = new UserRole { RoleName = "Staff" };
            var customer = new UserRole { RoleName = "Customer" };
            List<UserRole> userRoles = new()
             {
                 staff,
                 admin,
                 customer
             };
            List<User> users = new()
            {
                new User
                {
                    UserName = "Nguyen Ngoc Tuan",
                    Address = "Lam Dong",
                    Password = SecurityUtil.Hash("12345"),
                    Email = "tuannnse170112@fpt.edu.vn",
                    PhoneNumber = "123457891",
                    Balance = 333,
                    Status = "Active",
                    IsVerified= false,
                    UserRole = staff,
                },
                new User
                {
                    UserName = "Nguyen Ngoc Gia Bao",
                    Address = "TP Ho Chi Minh",
                    Password = SecurityUtil.Hash("12345"),
                    Email = "baonngse173536@fpt.edu.vn",
                    PhoneNumber = "0184785731",
                    Balance = 334,
                    Status = "Active",
                    IsVerified = false,
                    UserRole = staff
                },
                new User
                {
                    UserName = "Admin",
                    Address = "TP Ho Chi Minh",
                    Password = SecurityUtil.Hash("12345"),
                    Email = "admin@gmail.com",
                    PhoneNumber = "01232321232",
                    Balance = 335,
                    Status = "Active",
                    IsVerified = false,
                    UserRole = admin
                },
                new User
                {
                    UserName = "Staff",
                    Address = "Ben Tre",
                    Password = SecurityUtil.Hash("12345"),
                    Email = "staff@gmail.com",
                    PhoneNumber = "09836457382",
                    Balance = 336,
                    Status = "Active",
                    IsVerified = false,
                    UserRole = staff
                },
                new User
                {
                    UserName = "Customer",
                    Address = "Binh Thuan",
                    Password = SecurityUtil.Hash("12345"),
                    Email = "customer@gmail.com",
                    PhoneNumber = "0964765897",
                    Balance = 337,
                    Status = "Active",
                    IsVerified = false,
                    UserRole = customer
                }
            };
            List<City> cities = new()
            {
                new City { Name = "An Giang", Status = "Active" },
                new City { Name = "Bà Rịa - Vũng Tàu", Status = "Active" },
                new City { Name = "Bạc Liêu", Status = "Active" },
                new City { Name = "Bắc Giang", Status = "Active" },
                new City { Name = "Bắc Kạn", Status = "Active" },
                new City { Name = "Bắc Ninh", Status = "Active" },
                new City { Name = "Bến Tre", Status = "Active" },
                new City { Name = "Bình Định", Status = "Active" },
                new City { Name = "Bình Dương", Status = "Active" },
                new City { Name = "Bình Phước", Status = "Active" },
                new City { Name = "Bình Thuận", Status = "Active" },
                new City { Name = "Cà Mau", Status = "Active" },
                new City { Name = "Cần Thơ", Status = "Active" },
                new City { Name = "Cao Bằng", Status = "Active" },
                new City { Name = "Đà Nẵng", Status = "Active" },
                new City { Name = "Đắk Lắk", Status = "Active" },
                new City { Name = "Đắk Nông", Status = "Active" },
                new City { Name = "Điện Biên", Status = "Active" },
                new City { Name = "Đồng Nai", Status = "Active" },
                new City { Name = "Đồng Tháp", Status = "Active" },
                new City { Name = "Gia Lai", Status = "Active" },
                new City { Name = "Hà Giang", Status = "Active" },
                new City { Name = "Hà Nam", Status = "Active" },
                new City { Name = "Hà Nội", Status = "Active" },
                new City { Name = "Hà Tĩnh", Status = "Active" },
                new City { Name = "Hải Dương", Status = "Active" },
                new City { Name = "Hải Phòng", Status = "Active" },
                new City { Name = "Hậu Giang", Status = "Active" },
                new City { Name = "Hòa Bình", Status = "Active" },
                new City { Name = "Hưng Yên", Status = "Active" },
                new City { Name = "Khánh Hòa", Status = "Active" },
                new City { Name = "Kiên Giang", Status = "Active" },
                new City { Name = "Kon Tum", Status = "Active" },
                new City { Name = "Lai Châu", Status = "Active" },
                new City { Name = "Lâm Đồng", Status = "Active" },
                new City { Name = "Lạng Sơn", Status = "Active" },
                new City { Name = "Lào Cai", Status = "Active" },
                new City { Name = "Long An", Status = "Active" },
                new City { Name = "Nam Định", Status = "Active" },
                new City { Name = "Nghệ An", Status = "Active" },
                new City { Name = "Ninh Bình", Status = "Active" },
                new City { Name = "Ninh Thuận", Status = "Active" },
                new City { Name = "Phú Thọ", Status = "Active" },
                new City { Name = "Phú Yên", Status = "Active" },
                new City { Name = "Quảng Bình", Status = "Active" },
                new City { Name = "Quảng Nam", Status = "Active" },
                new City { Name = "Quảng Ngãi", Status = "Active" },
                new City { Name = "Quảng Ninh", Status = "Active" },
                new City { Name = "Quảng Trị", Status = "Active" },
                new City { Name = "Sóc Trăng", Status = "Active" },
                new City { Name = "Sơn La", Status = "Active" },
                new City { Name = "Tây Ninh", Status = "Active" },
                new City { Name = "Thái Bình", Status = "Active" },
                new City { Name = "Thái Nguyên", Status = "Active" },
                new City { Name = "Thanh Hóa", Status = "Active" },
                new City { Name = "Thừa Thiên Huế", Status = "Active" },
                new City { Name = "Tiền Giang", Status = "Active" },
                new City { Name = "TP. Hồ Chí Minh", Status = "Active" },
                new City { Name = "Trà Vinh", Status = "Active" },
                new City { Name = "Tuyên Quang", Status = "Active" },
                new City { Name = "Vĩnh Long", Status = "Active" },
                new City { Name = "Vĩnh Phúc", Status = "Active" },
                new City { Name = "Yên Bái", Status = "Active" }
            };
            List<Company> companies = new()
            {
                new Company
                {
                    Name = "Phuong Trang",
                    Status = "Active"
                },
                new Company
                {
                    Name = "Thanh Buoi",
                    Status = "Active"
                },
                new Company
                {
                    Name = "Thanh Huy",
                    Status = "Active"
                },
                new Company
                {
                    Name = "Ngoc Tuan",
                    Status = "Active"
                },
                new Company
                {
                    Name = "My Thuong",
                    Status = "Active"
                }
            };
            List<Station> stations = new()
            {
                new Station
                {
                    Name = "Trạm dừng chân Hưng Lộc",
                    Status = "Active",
                    City = cities[0],
                    Company = companies[0]
                },
                new Station
                {
                    Name = "Trạm dừng chân Lan Rừng",
                    Status = "Active",
                    City = cities[1],
                    Company = companies[1]
                },
                new Station
                {
                    Name = "Nhà Xe Hồng Phượng",
                    Status = "Active",
                    City = cities[2],
                    Company = companies[2]
                },
                new Station
                {
                    Name = "Trại Bò Sữa Long Thành",
                    Status = "Active",
                    City = cities[3],
                    Company = companies[3]
                },
                new Station
                {
                    Name = "Trạm Dừng Chân Sáu Nho",
                    Status = "Active",
                    City = cities[4],
                    Company = companies[4]
                },
                new Station
                {
                    Name = "Trạm Dừng Chân Tâm Châu",
                    Status = "Active",
                    City = cities[5],
                    Company = companies[2]
                }
            };

            List<Route> routes = new()
            {
                new Route
                {
                    FromCity = cities[0],
                    ToCity = cities[6],
                    Company = companies[0],
                    StartLocation = "KCN Mỹ Xuân A",
                    EndLocation = "Quan 2",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[2],
                    ToCity = cities[6],
                    Company = companies[1],
                    StartLocation = "Giáo xứ Chu Hải",
                    EndLocation = "Quan 5",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[3],
                    ToCity = cities[6],
                    Company = companies[2],
                    StartLocation = "Ngã ba Cái Mép",
                    EndLocation = "Quan 10",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[4],
                    ToCity = cities[6],
                    Company = companies[3],
                    StartLocation = "Siêu thị Coop Mart Tân Thành",
                    EndLocation = "Quan 9",
                    Status = "Active"
                }
            };
            List<Station_Route> station_Routes = new()
            {
                new Station_Route
                {
                    Route = routes[0],
                    Station = stations[0],
                    Status = "Active",
                    OrderInRoute = 1
                },
                new Station_Route
                {
                    Route = routes[2],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute = 2
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[4],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute = 2
                },
                new Station_Route
                {
                    Route = routes[3],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =1
                },
            };
            List<Trip> trips = new()
            {
                new Trip
                {
                    Route=routes[0],
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[2],
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status ="Active"
                }
            };
            List<ServiceType> serviceTypes = new()
            {
                new ServiceType
                {
                    Name = "Đồ ăn",
                    Status = "Active"
                },
                new ServiceType
                {
                    Name = "Đồ uống",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "Ăn vặt",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "Đồ ăn nhanh",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "ATM",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "Khu vui chơi trẻ em",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "Dịch vụ gửi đồ và hành lý",
                    Status = "Active"
                },
                new ServiceType
                {
                   Name = "Xe máy",
                    Status = "Active"
                }

            };
            List<Utility> utilities = new()
            {
                new Utility
                {
                    Name="Nhà vệ sinh",
                    Description ="Xe được trang bị nhà vệ sinh sạch sẽ.",
                    Status = "active"
                },
                new Utility
                {
                    Name="Chăn",
                    Description ="Xe cung cấp chăn để hành khách có thể giữ ấm trong suốt hành trình, đặc biệt là vào ban đêm hoặc khi điều hòa nhiệt độ lạnh.",

                    Status = "active"
                },
                new Utility
                {
                    Name="Wi-Fi miễn phí",
                    Description ="Xe cung cấp dịch vụ Wi-Fi miễn phí để hành khách có thể sử dụng internet trong suốt chuyến đi",
                    Status = "active"
                },
                new Utility
                {
                    Name="Ổ cắm điện và cổng USB",
                    Description ="Các ổ cắm điện và cổng USB được trang bị để hành khách có thể sạc điện thoại, máy tính bảng hoặc laptop.",
                    Status = "active"
                },
                new Utility
                {
                    Name="Nước uống và khăn lạnh",
                    Description ="Nhà xe cung cấp nước uống đóng chai và khăn lạnh miễn phí cho hành khách.",
                    Status = "active"
                },
                new Utility
                {
                    Name="Rèm che riêng tư",
                    Description ="Trên xe có rèm che riêng tư giúp hành khách có không gian riêng tư hơn.",
                    Status = "active"
                },

            };
            List<Trip_Utility> utilityInTrips = new()
            {
                new Trip_Utility
                {
                    Utility = utilities[0],
                    Status = "active",
                    Trip = trips[0]
                },
                new Trip_Utility
                {
                    Utility = utilities[1],
                    Status = "active",
                    Trip = trips[0]
                },
                new Trip_Utility
                {
                    Utility = utilities[2],
                    Status = "active",
                    Trip = trips[0]
                },
                new Trip_Utility
                {
                    Utility = utilities[1],
                    Status = "active",
                    Trip = trips[2]
                },
                new Trip_Utility
                {
                    Utility = utilities[1],
                    Status = "active",
                    Trip = trips[3]
                },
                new Trip_Utility
                {
                    Utility = utilities[2],
                    Status = "active",
                    Trip = trips[3]
                },
                new Trip_Utility
                {
                    Utility = utilities[0],
                    Status = "active",
                    Trip = trips[3]
                }
            };
            List<Service> services = new()
            {
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[0],
                    Name = "Mi tom",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[0],
                    Name = "Pho",
                    Price = 45000
                },
                new Service
                {
                    Route=routes[2],
                    ServiceType=serviceTypes[1],
                    Name = "Bun bo",
                    Price = 45000
                },
                new Service
                {
                    Route=routes[3],
                    ServiceType=serviceTypes[2],
                    Name = "Com xeo",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[3],
                    ServiceType=serviceTypes[1],
                    Name = "Com phan",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[0],
                    Name = "Mi tom",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[3],
                    Name = "Hamburger",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[2],
                    ServiceType=serviceTypes[3],
                    Name = "Sandwich",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[2],
                    ServiceType=serviceTypes[3],
                    Name = "hotdog",
                    Price = 35000
                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[2],
                    Name = "Trái cây",
                    Price = 35000

                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[1],
                    Name = "Cà phê",
                    Price = 35000

                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[1],
                    Name = "Nước trái cây",
                    Price = 35000

                },
                new Service
                {
                    Route=routes[1],
                    ServiceType=serviceTypes[1],
                    Name = "Trà",
                    Price = 35000

                },
            };
            List<TicketType> types = new()
            {
                new TicketType
                {
                    Route = routes[1],
                    Name = "Hàng đầu",
                    Status = "Active"
                },
                new TicketType
                {
                    Route = routes[1],
                    Name = "Hàng giữa",
                    Status = "Active"
                },
                new TicketType
                {
                    Route = routes[2],
                    Name = "Hàng sau",
                    Status = "Active"
                },
            };
            List<TicketType_Trip> ticketType_Trips = new()
            {
                new TicketType_Trip
                {
                    TicketType = types[0],
                    Trip = trips[0],
                    Price = 120000,
                    Status = "Active",
                    Quantity = 5
                },
                new TicketType_Trip
                {
                    TicketType = types[0],
                    Trip = trips[1],
                    Price = 150000,
                    Status = "Active",
                    Quantity = 1
                },
                new TicketType_Trip
                {
                    TicketType = types[2],
                    Trip = trips[2],
                    Price = 120000,
                    Status = "Active",
                    Quantity = 5
                },
                new TicketType_Trip
                {
                    TicketType = types[0],
                    Trip = trips[3],
                    Price = 100000,
                    Status = "Active",
                    Quantity = 3
                },
                new TicketType_Trip
                {
                    TicketType = types[1],
                    Trip = trips[0],
                    Price = 120000,
                    Status = "Active",
                    Quantity = 5
                }
            };
            List<Booking> bookings = new()
            {
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "Active",
                    PaymentMethod = "Cash",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[0],
                    User = users[0]
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 2,
                    Status = "Active",
                    PaymentMethod = "Cash",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[1],
                    User = users[1]
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 2,
                    Status = "Active",
                    PaymentMethod = "Cash",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[3],
                    User = users[3]
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "Active",
                    PaymentMethod = "Cash",
                    PaymentStatus = "True",
                    TotalBill = 100000,
                    Trip = trips[0],
                    User = users[0]
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "Active",
                    PaymentMethod = "Cash",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[3],
                    User = users[3]
                }
            };
            List<TicketDetail> ticketDetails = new()
            {
                new TicketDetail
                {
                    Booking = bookings[0],
                    Price = 120000,
                    Status = "Active",
                    SeatCode = "A01",

                    TicketType_Trip = ticketType_Trips[0]
                },
                new TicketDetail
                {
                    Booking = bookings[2],
                    Price = 120000,
                    Status = "Active",
                    SeatCode = "A02",

                    TicketType_Trip = ticketType_Trips[1]
                },
                new TicketDetail
                {
                    Booking = bookings[1],
                    Price = 120000,
                    Status = "Active",
                    SeatCode = "A03",

                    TicketType_Trip = ticketType_Trips[1]
                },
                new TicketDetail
                {
                    Booking = bookings[3],
                    Price = 100000,
                    Status = "Active",
                    SeatCode = "A04",

                    TicketType_Trip = ticketType_Trips[2]
                },
                new TicketDetail
                {
                    Booking = bookings[4],
                    Price = 120000,
                    Status = "Active",
                    SeatCode = "A05",

                    TicketType_Trip = ticketType_Trips[3]
                }
            };
            List<TicketDetail_Service> ticketDetail_Services = new()
            {
                new TicketDetail_Service
                {
                    TicketDetail = ticketDetails[1],
                    Service = services[1],
                    Price = 120000,
                    Quantity = 1,
                    Status = "Active"
                },
                new TicketDetail_Service
                {
                    TicketDetail = ticketDetails[2],
                    Service = services[2],
                    Price = 150000,
                    Quantity = 2,
                    Status = "Active"
                },
                new TicketDetail_Service
                {
                    TicketDetail = ticketDetails[0],
                    Service = services[0],
                    Price = 120000,
                    Quantity = 5,
                    Status = "Active"
                }
            };
            List<Station_Service> station_Services = new()
            {
                new Station_Service
                {
                    Station = stations[0],
                    Service = services[0],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[1],
                    Service = services[1],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[0],
                    Service = services[2],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[2],
                    Service = services[1],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[3],
                    Service = services[3],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[1],
                    Service = services[0],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[1],
                    Service = services[4],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[2],
                    Service = services[0],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[2],
                    Service = services[2],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[2],
                    Service = services[3],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[3],
                    Service = services[2],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[3],
                    Service = services[3],
                    Status = "Active"

                },
                new Station_Service
                {
                    Station = stations[4],
                    Service = services[1],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[4],
                    Service = services[2],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[4],
                    Service = services[0],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[5],
                    Service = services[1],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[5],
                    Service = services[0],
                    Status = "Active"
                },
                new Station_Service
                {
                    Station = stations[5],
                    Service = services[2],
                    Status = "Active" },
                    new Station_Service
                {
                    Station = stations[5],
                    Service = services[3],
                    Status = "Active"
                }

            };

            List<Service_Trip> trip_Services = new()
            {
                new Service_Trip
                {
                    Trip = trips[0],
                    Service = services[0],
                    Status = "Active"
                },
                new Service_Trip
                {
                    Trip = trips[0],
                    Service = services[1],
                    Status = "Active"
                },
                new Service_Trip
                {
                    Trip = trips[2],
                    Service = services[1],
                    Status = "Active"
                },
                new Service_Trip
                {
                    Trip = trips[2],
                    Service = services[2],
                    Status = "Active"
                },
                new Service_Trip
                {
                    Trip = trips[3],
                    Service = services[1],
                    Status = "Active"
                }
            };
            List<Route_Company> route_Companies = new()
            {
                new Route_Company
                {
                    Company = companies[0],
                    Route = routes[0],
                    Status = "Active"
                },
                new Route_Company
                {
                    Company = companies[1],
                    Route = routes[1],
                    Status = "Active"
                },
                new Route_Company
                {
                    Company = companies[2],
                    Route = routes[2],
                    Status = "Active"
                },
                new Route_Company
                {
                    Company = companies[3],
                    Route = routes[3],
                    Status = "Active"
                },
                new Route_Company
                {
                    Company = companies[1],
                    Route = routes[3],
                    Status = "Active"
                },
            };
            List<Feedback> feedbacks = new()
            {
                new Feedback
                {
                    User = users[0],
                    Trip = trips[0],
                    Rating = 5,
                    Description = "Dịch vụ tốt",
                    Status = "Active"
                }, new Feedback
                {
                    User = users[0],
                    Trip = trips[1],
                    Rating = 4,
                    Description = "Dịch vụ khá tốt",
                    Status = "Active"
                }, new Feedback
                {
                    User = users[0],
                    Trip = trips[2],
                    Rating = 4,
                    Description = "Dịch vụ rất tốt",
                    Status = "Active"
                }, new Feedback
                {
                    User = users[0],
                    Trip = trips[3],
                    Rating = 3,
                    Description = "Dịch vụ tạm",
                    Status = "Active"
                }, new Feedback
                {
                    User = users[0],
                    Trip = trips[4],
                    Rating = 5,
                    Description = "Dịch vụ tốt",
                    Status = "Active"
                },
            };
            List<Feedback_Image> images = new()
            {
                new Feedback_Image
                {
                    Feedback = feedbacks[0],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                },
                 new Feedback_Image
                {
                    Feedback = feedbacks[0],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }, new Feedback_Image
                {
                    Feedback = feedbacks[1],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }, new Feedback_Image
                {
                    Feedback = feedbacks[1],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }, new Feedback_Image
                {
                    Feedback = feedbacks[2],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }, new Feedback_Image
                {
                    Feedback = feedbacks[3],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }, new Feedback_Image
                {
                    Feedback = feedbacks[4],
                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
                    Status = "Active"
                }
            };
            // Add Range training program
            await _context.Users.AddRangeAsync(users);
            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.City.AddRangeAsync(cities);
            await _context.Company.AddRangeAsync(companies);
            await _context.Station.AddRangeAsync(stations);
            await _context.Route.AddRangeAsync(routes);
            await _context.Station_Route.AddRangeAsync(station_Routes);
            await _context.Trip.AddRangeAsync(trips);
            await _context.ServiceType.AddRangeAsync(serviceTypes);
            await _context.Service.AddRangeAsync(services);
            await _context.TicketType.AddRangeAsync(types);
            await _context.TicketType_Trip.AddRangeAsync(ticketType_Trips);
            await _context.Booking.AddRangeAsync(bookings);
            await _context.TicketDetail.AddRangeAsync(ticketDetails);
            await _context.TicketDetail_Service.AddRangeAsync(ticketDetail_Services);
            await _context.Station_Service.AddRangeAsync(station_Services);
            await _context.Service_Trip.AddRangeAsync(trip_Services);
            await _context.Route_Company.AddRangeAsync(route_Companies);
            await _context.Utility.AddRangeAsync(utilities);
            await _context.utilityInTrips.AddRangeAsync(utilityInTrips);

            // Save to DB
            await _context.SaveChangesAsync();
        }
    }

    public static class DatabaseInitialiserExtension
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            // Create IServiceScope to resolve service scope
            using var scope = app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();

            await initializer.InitialiseAsync();

            // Try to seeding data
            await initializer.SeedAsync();
        }
    }
}
