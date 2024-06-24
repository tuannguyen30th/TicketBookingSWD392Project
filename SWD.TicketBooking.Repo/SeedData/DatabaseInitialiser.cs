//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Swashbuckle.AspNetCore.SwaggerUI;
//using SWD.TicketBooking.Repo.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SWD.TicketBooking.Repo.SeedData
//{
//    public interface IDataaseInitialiser
//    {
//        Task InitialiseAsync();

//        Task SeedAsync();

//        Task TrySeedAsync();
//    }

//    public class DatabaseInitialiser : IDataaseInitialiser
//    {
//        public readonly TicketBookingDbContext _context;

//        public DatabaseInitialiser(TicketBookingDbContext context)
//        {
//            _context = context;
//        }

//        public async Task InitialiseAsync()
//        {
//            try
//            {
//                // Migration Database - Create database if it does not exist
//                await _context.Database.MigrateAsync();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw;
//            }
//        }

//        public async Task SeedAsync()
//        {
//            try
//            {
//                await TrySeedAsync();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw;
//            }
//        }

//        public async Task TrySeedAsync()
//        {
//            if (_context.UserRoles.Any())
//            {
//                return;
//            }
//            var admin = new UserRole { RoleID = Guid.NewGuid(), RoleName = "Admin" };
//            var staff = new UserRole { RoleID = Guid.NewGuid(), RoleName = "Staff" };
//            var customer = new UserRole { RoleID = new Guid("E6E2FCD6-22F0-426B-A3A0-DD0C5D398387"), RoleName = "Customer" };
//            List<UserRole> userRoles = new()
//             {
//                 staff,
//                 admin,
//                 customer
//             };
//            List<User> users = new()
//            {
//                new User
//                {
//                    UserID = Guid.NewGuid(),
//                    UserName = "Nguyen Ngoc Tuan",
//                    FullName = "Nguyen Ngoc Tuan",
//                    Address = "Lam Dong",
//                    Password = SecurityUtil.Hash("12345"),
//                    Email = "tuannnse170112@fpt.edu.vn",
//                    PhoneNumber = "123457891",
//                    Balance = 333,
//                    Status = "ACTIVE",
//                    IsVerified= false,
//                    UserRole = staff,
//                },
//                new User
//                {
//                    UserID = Guid.NewGuid(),
//                    UserName = "Nguyen Ngoc Gia Bao",
//                    FullName = "Nguyen Ngoc Gia Bao",
//                    Address = "TP Ho Chi Minh",
//                    Password = SecurityUtil.Hash("12345"),
//                    Email = "baonngse173536@fpt.edu.vn",
//                    PhoneNumber = "0184785731",
//                    Balance = 334,
//                    Status = "ACTIVE",
//                    IsVerified = false,
//                    UserRole = staff
//                },
//                new User
//                {
//                    UserID = Guid.NewGuid(),
//                    UserName = "Admin",
//                    FullName = "Admin",
//                    Address = "TP Ho Chi Minh",
//                    Password = SecurityUtil.Hash("12345"),
//                    Email = "admin@gmail.com",
//                    PhoneNumber = "01232321232",
//                    Balance = 335,
//                    Status = "ACTIVE",
//                    IsVerified = false,
//                    UserRole = admin
//                },
//                new User
//                {
//                    UserID = Guid.NewGuid(),
//                    UserName = "Staff",
//                    FullName = "Staff",
//                    Address = "Ben Tre",
//                    Password = SecurityUtil.Hash("12345"),
//                    Email = "staff@gmail.com",
//                    PhoneNumber = "09836457382",
//                    Balance = 336,
//                    Status = "ACTIVE",
//                    IsVerified = false,
//                    UserRole = staff
//                },
//                new User
//                {
//                    UserID = Guid.NewGuid(),
//                    UserName = "Customer",
//                    FullName = "Customer",
//                    Address = "Binh Thuan",
//                    Password = SecurityUtil.Hash("12345"),
//                    Email = "customer@gmail.com",
//                    PhoneNumber = "0964765897",
//                    Balance = 337,
//                    Status = "ACTIVE",
//                    IsVerified = false,
//                    UserRole = customer
//                }
//            };
//            List<City> cities = new()
//            {
//                new City { CityID = Guid.NewGuid(), Name = "An Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bà Rịa - Vũng Tàu", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bạc Liêu", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bắc Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bắc Kạn", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bắc Ninh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bến Tre", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bình Định", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bình Dương", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bình Phước", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Bình Thuận", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Cà Mau", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Cần Thơ", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Cao Bằng", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Đà Nẵng", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Đắk Lắk", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Đắk Nông", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Điện Biên", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Đồng Nai", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Đồng Tháp", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Gia Lai", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hà Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hà Nam", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hà Nội", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hà Tĩnh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hải Dương", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hải Phòng", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hậu Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hòa Bình", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Hưng Yên", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Khánh Hòa", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Kiên Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Kon Tum", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Lai Châu", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Lâm Đồng", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Lạng Sơn", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Lào Cai", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Long An", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Nam Định", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Nghệ An", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Ninh Bình", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Ninh Thuận", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Phú Thọ", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Phú Yên", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Quảng Bình", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Quảng Nam", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Quảng Ngãi", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Quảng Ninh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Quảng Trị", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Sóc Trăng", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Sơn La", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Tây Ninh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Thái Bình", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Thái Nguyên", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Thanh Hóa", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Thừa Thiên Huế", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Tiền Giang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "TP. Hồ Chí Minh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Trà Vinh", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Tuyên Quang", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Vĩnh Long", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Vĩnh Phúc", Status = "ACTIVE" },
//                new City { CityID = Guid.NewGuid(), Name = "Yên Bái", Status = "ACTIVE" }
//            };
//            List<Company> companies = new()
//            {
//                new Company
//                {
//                    CompanyID = Guid.NewGuid(),
//                    Name = "Phuong Trang",
//                    Status = "ACTIVE"
//                },
//                new Company
//                {
//                     CompanyID = Guid.NewGuid(),
//                    Name = "Thanh Buoi",
//                    Status = "ACTIVE"
//                },
//                new Company
//                {
//                     CompanyID = Guid.NewGuid(),
//                    Name = "Thanh Huy",
//                    Status = "ACTIVE"
//                },
//                new Company
//                {
//                     CompanyID = Guid.NewGuid(),
//                    Name = "Ngoc Tuan",
//                    Status = "ACTIVE"
//                },
//                new Company
//                {
//                     CompanyID = Guid.NewGuid(),
//                    Name = "My Thuong",
//                    Status = "ACTIVE"
//                }
//            };
//            List<Station> stations = new()
//            {
//                new Station
//                {
//                    StationID = Guid.NewGuid(),
//                    Name = "Trạm dừng chân Hưng Lộc",
//                    Status = "ACTIVE",
//                    City = cities[0],
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trạm dừng chân Lan Rừng",
//                    Status = "ACTIVE",
//                    City = cities[1],
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nhà Xe Hồng Phượng",
//                    Status = "ACTIVE",
//                    City = cities[2],
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trại Bò Sữa Long Thành",
//                    Status = "ACTIVE",
//                    City = cities[3],
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trạm Dừng Chân Sáu Nho",
//                    Status = "ACTIVE",
//                    City = cities[4],
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trạm Dừng Chân Tâm Châu",
//                    Status = "ACTIVE",
//                    City = cities[5],
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trạm dừng chân ở Bảo Lộc",
//                    Status = "ACTIVE",
//                    City = cities[34],
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Khu du lịch Đồng Tâm",
//                    Status = "ACTIVE",
//                    City = cities[5],
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Đồng Tháp Resort",
//                    Status = "ACTIVE",
//                    City = cities[19], // Đồng Tháp
//                    Company = companies[2]
//                },
//                                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "An Giang Waterpark",
//                    Status = "ACTIVE",
//                    City = cities[0], // An Giang
//                    Company = companies[0]
//                },

//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Vũng Tàu Beach Resort",
//                    Status = "ACTIVE",
//                    City = cities[1], // Bà Rịa - Vũng Tàu
//                    Company = companies[1]
//                },

//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Bạc Liêu Nature Reserve",
//                    Status = "ACTIVE",
//                    City = cities[2], // Bạc Liêu
//                    Company = companies[3]
//                },

//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Bắc Giang Eco-Park",
//                    Status = "ACTIVE",
//                    City = cities[3], // Bắc Giang
//                    Company = companies[4]
//                },

//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Bắc Kạn Highlands",
//                    Status = "ACTIVE",
//                    City = cities[4], // Bắc Kạn
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Adventure Park",
//                    Status = "ACTIVE",
//                    City = cities[13], // Gia Lai
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Ethnic Village",
//                    Status = "ACTIVE",
//                    City = cities[13], // Gia Lai
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Scenic Overlook",
//                    Status = "ACTIVE",
//                    City = cities[14], // Hà Giang
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Trekking Trail",
//                    Status = "ACTIVE",
//                    City = cities[14], // Hà Giang
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hải Dương Amusement Park",
//                    Status = "ACTIVE",
//                    City = cities[6], // Hải Dương
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hải Dương Cycling Route",
//                    Status = "ACTIVE",
//                    City = cities[6], // Hải Dương
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hậu Giang Rice Fields",
//                    Status = "ACTIVE",
//                    City = cities[8], // Hậu Giang
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hậu Giang Farmer's Market",
//                    Status = "ACTIVE",
//                    City = cities[8], // Hậu Giang
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hòa Bình Dam",
//                    Status = "ACTIVE",
//                    City = cities[9], // Hòa Bình
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hòa Bình Hiking Trail",
//                    Status = "ACTIVE",
//                    City = cities[9], // Hòa Bình
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Adventure Park",
//                    Status = "ACTIVE",
//                    City = cities[13], // Gia Lai
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Ethnic Village",
//                    Status = "ACTIVE",
//                    City = cities[13], // Gia Lai
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Scenic Overlook",
//                    Status = "ACTIVE",
//                    City = cities[14], // Hà Giang
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Trekking Trail",
//                    Status = "ACTIVE",
//                    City = cities[14], // Hà Giang
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hải Dương Amusement Park",
//                    Status = "ACTIVE",
//                    City = cities[6], // Hải Dương
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hải Dương Cycling Route",
//                    Status = "ACTIVE",
//                    City = cities[6], // Hải Dương
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hậu Giang Rice Fields",
//                    Status = "ACTIVE",
//                    City = cities[8], // Hậu Giang
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hậu Giang Farmer's Market",
//                    Status = "ACTIVE",
//                    City = cities[8], // Hậu Giang
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hòa Bình Dam",
//                    Status = "ACTIVE",
//                    City = cities[9], // Hòa Bình
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hòa Bình Hiking Trail",
//                    Status = "ACTIVE",
//                    City = cities[9], // Hòa Bình
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Khánh Hòa Beach Resort",
//                    Status = "ACTIVE",
//                    City = cities[15], // Khánh Hòa
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Khánh Hòa Diving Center",
//                    Status = "ACTIVE",
//                    City = cities[15], // Khánh Hòa
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kiên Giang Floating Market",
//                    Status = "ACTIVE",
//                    City = cities[16], // Kiên Giang
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kiên Giang Mangrove Tour",
//                    Status = "ACTIVE",
//                    City = cities[16], // Kiên Giang
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kon Tum Tribal Village",
//                    Status = "ACTIVE",
//                    City = cities[17], // Kon Tum
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kon Tum Eco-Lodge",
//                    Status = "ACTIVE",
//                    City = cities[17], // Kon Tum
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lai Châu Trekking Adventures",
//                    Status = "ACTIVE",
//                    City = cities[18], // Lai Châu
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lai Châu Minority Cultural Center",
//                    Status = "ACTIVE",
//                    City = cities[18], // Lai Châu
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lâm Đồng Coffee Plantation Tour",
//                    Status = "ACTIVE",
//                    City = cities[19], // Lâm Đồng
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lâm Đồng Flower Gardens",
//                    Status = "ACTIVE",
//                    City = cities[19], // Lâm Đồng
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lạng Sơn Frontier Market",
//                    Status = "ACTIVE",
//                    City = cities[20], // Lạng Sơn
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lạng Sơn Hiking Trails",
//                    Status = "ACTIVE",
//                    City = cities[20], // Lạng Sơn
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lào Cai Sapa Trekking",
//                    Status = "ACTIVE",
//                    City = cities[21], // Lào Cai
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lào Cai Bắc Hà Market",
//                    Status = "ACTIVE",
//                    City = cities[21], // Lào Cai
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Long An Eco-Resort",
//                    Status = "ACTIVE",
//                    City = cities[22], // Long An
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Long An Nature Reserve",
//                    Status = "ACTIVE",
//                    City = cities[22], // Long An
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nam Định Ancient Citadel",
//                    Status = "ACTIVE",
//                    City = cities[23], // Nam Định
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nam Định Temple Complex",
//                    Status = "ACTIVE",
//                    City = cities[23], // Nam Định
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nghệ An Phong Nha Cave",
//                    Status = "ACTIVE",
//                    City = cities[24], // Nghệ An
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nghệ An Xuan Truong Beach",
//                    Status = "ACTIVE",
//                    City = cities[24], // Nghệ An
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Bình Trang An Grottoes",
//                    Status = "ACTIVE",
//                    City = cities[25], // Ninh Bình
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Bình Tam Cốc Boat Tour",
//                    Status = "ACTIVE",
//                    City = cities[25], // Ninh Bình
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Thuận Sailing Adventures",
//                    Status = "ACTIVE",
//                    City = cities[26], // Ninh Thuận
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Thuận Kê Gà Beach",
//                    Status = "ACTIVE",
//                    City = cities[26], // Ninh Thuận
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Phú Thọ Đình Bảng Festival",
//                    Status = "ACTIVE",
//                    City = cities[27], // Phú Thọ
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Phú Thọ Hùng Temple",
//                    Status = "ACTIVE",
//                    City = cities[27], // Phú Thọ
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Bình Phong Nha-Kẻ Bàng National Park",
//                    Status = "ACTIVE",
//                    City = cities[28], // Quảng Bình
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Bình Grottoes of Mỹ Đức",
//                    Status = "ACTIVE",
//                    City = cities[28], // Quảng Bình
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Nam Hội An Ancient Town",
//                    Status = "ACTIVE",
//                    City = cities[29], // Quảng Nam
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Nam Mỹ Sơn Sanctuary",
//                    Status = "ACTIVE",
//                    City = cities[29], // Quảng Nam
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ninh Hạ Long Bay",
//                    Status = "ACTIVE",
//                    City = cities[30], // Quảng Ninh
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ninh Yên Tử Mountain",
//                    Status = "ACTIVE",
//                    City = cities[30], // Quảng Ninh
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Sóc Trăng Khmer Pagodas",
//                    Status = "ACTIVE",
//                    City = cities[31], // Sóc Trăng
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Sóc Trăng Fruit Gardens",
//                    Status = "ACTIVE",
//                    City = cities[31], // Sóc Trăng
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Sơn La Mộc Châu Plateau",
//                    Status = "ACTIVE",
//                    City = cities[32], // Sơn La
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Sơn La Pha Luông National Park",
//                    Status = "ACTIVE",
//                    City = cities[32], // Sơn La
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tây Ninh Bà Đen Mountain",
//                    Status = "ACTIVE",
//                    City = cities[33], // Tây Ninh
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tây Ninh Cao Đài Temple",
//                    Status = "ACTIVE",
//                    City = cities[33], // Tây Ninh
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thái Bình Ancient Pagodas",
//                    Status = "ACTIVE",
//                    City = cities[34], // Thái Bình
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thái Bình Lộc Vượn Nature Reserve",
//                    Status = "ACTIVE",
//                    City = cities[34], // Thái Bình
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thanh Hóa Hồ Sơn Temple",
//                    Status = "ACTIVE",
//                    City = cities[35], // Thanh Hóa
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thanh Hóa Cửa Lò Beach",
//                    Status = "ACTIVE",
//                    City = cities[35], // Thanh Hóa
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thừa Thiên Huế Perfume River Cruise",
//                    Status = "ACTIVE",
//                    City = cities[36], // Thừa Thiên Huế
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Thừa Thiên Huế Imperial City",
//                    Status = "ACTIVE",
//                    City = cities[36], // Thừa Thiên Huế
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tiền Giang Mekong Delta Floating Market",
//                    Status = "ACTIVE",
//                    City = cities[37], // Tiền Giang
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tiền Giang Gò Công Seafood",
//                    Status = "ACTIVE",
//                    City = cities[37], // Tiền Giang
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trà Vinh Khmer Minority Villages",
//                    Status = "ACTIVE",
//                    City = cities[38], // Trà Vinh
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Trà Vinh Cồn Phụng Ecotourism Site",
//                    Status = "ACTIVE",
//                    City = cities[38], // Trà Vinh
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tuyên Quang Ba Bể National Park",
//                    Status = "ACTIVE",
//                    City = cities[39], // Tuyên Quang
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Tuyên Quang Tây Thiên Pagoda",
//                    Status = "ACTIVE",
//                    City = cities[39], // Tuyên Quang
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Vĩnh Long Cái Bè Floating Market",
//                    Status = "ACTIVE",
//                    City = cities[40], // Vĩnh Long
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Vĩnh Long Vườn Xoài",
//                    Status = "ACTIVE",
//                    City = cities[40], // Vĩnh Long
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Vĩnh Phúc Tam Đảo National Park",
//                    Status = "ACTIVE",
//                    City = cities[41], // Vĩnh Phúc
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Vĩnh Phúc Phượng Hoàng Castle",
//                    Status = "ACTIVE",
//                    City = cities[41], // Vĩnh Phúc
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Yên Bái Tú Lệ Rice Terraces",
//                    Status = "ACTIVE",
//                    City = cities[42], // Yên Bái
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Yên Bái Mù Căng Chải Waterfall",
//                    Status = "ACTIVE",
//                    City = cities[42], // Yên Bái
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Bắc Kạn Ba Bể National Park",
//                    Status = "ACTIVE",
//                    City = cities[43], // Bắc Kạn
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Bắc Kạn Cao Bằng Geopark",
//                    Status = "ACTIVE",
//                    City = cities[43], // Bắc Kạn
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Cao Bằng Cao Bằng Caves",
//                    Status = "ACTIVE",
//                    City = cities[44], // Cao Bằng
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Cao Bằng Đồng Đăng Border Gate",
//                    Status = "ACTIVE",
//                    City = cities[44], // Cao Bằng
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Điện Biên Điện Biên Phu Battlefield",
//                    Status = "ACTIVE",
//                    City = cities[45], // Điện Biên
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Điện Biên Sapa Terraced Fields",
//                    Status = "ACTIVE",
//                    City = cities[45], // Điện Biên
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Đắk Lắk Buôn Mê Thuột Coffee Plantations",
//                    Status = "ACTIVE",
//                    City = cities[46], // Đắk Lắk
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Đắk Lắk Yok Đôn National Park",
//                    Status = "ACTIVE",
//                    City = cities[46], // Đắk Lắk
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Đắk Nông Đăk Nông Geopark",
//                    Status = "ACTIVE",
//                    City = cities[47], // Đắk Nông
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Đắk Nông Gia Nghĩa Town",
//                    Status = "ACTIVE",
//                    City = cities[47], // Đắk Nông
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Kon Tum Ethnic Minority Villages",
//                    Status = "ACTIVE",
//                    City = cities[48], // Gia Lai
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Gia Lai Pleiku War Remnants Museum",
//                    Status = "ACTIVE",
//                    City = cities[48], // Gia Lai
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Mã Pí Lèng Pass",
//                    Status = "ACTIVE",
//                    City = cities[49], // Hà Giang
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Hà Giang Lung Củng Village",
//                    Status = "ACTIVE",
//                    City = cities[49], // Hà Giang
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kon Tum Kon Tum Cathedral",
//                    Status = "ACTIVE",
//                    City = cities[50], // Kon Tum
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Kon Tum Ngọc Linh Mountains",
//                    Status = "ACTIVE",
//                    City = cities[50], // Kon Tum
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lai Châu Sơn La Reservoir",
//                    Status = "ACTIVE",
//                    City = cities[51], // Lai Châu
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lai Châu Hoàng Liên Sơn Nature Reserve",
//                    Status = "ACTIVE",
//                    City = cities[51], // Lai Châu
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lâm Đồng Đà Lạt Flower Gardens",
//                    Status = "ACTIVE",
//                    City = cities[52], // Lâm Đồng
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lâm Đồng Liên Khương Airbase",
//                    Status = "ACTIVE",
//                    City = cities[52], // Lâm Đồng
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lạng Sơn Mẫu Temple",
//                    Status = "ACTIVE",
//                    City = cities[53], // Lạng Sơn
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lạng Sơn Đồng Đăng Border Gate",
//                    Status = "ACTIVE",
//                    City = cities[53], // Lạng Sơn
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lào Cai Sa Pa Cultural Center",
//                    Status = "ACTIVE",
//                    City = cities[54], // Lào Cai
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lào Cai Fansipan Mountain",
//                    Status = "ACTIVE",
//                    City = cities[54], // Lào Cai
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Lào Cai Bát Xát District",
//                    Status = "ACTIVE",
//                    City = cities[54], // Lào Cai
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nghệ An Vinh City",
//                    Status = "ACTIVE",
//                    City = cities[55], // Nghệ An
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Nghệ An Phong Nha-Kẻ Bàng National Park",
//                    Status = "ACTIVE",
//                    City = cities[55], // Nghệ An
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Bình Tam Cốc Bích Động",
//                    Status = "ACTIVE",
//                    City = cities[56], // Ninh Bình
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Bình Cúc Phương National Park",
//                    Status = "ACTIVE",
//                    City = cities[56], // Ninh Bình
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Thuận Phan Rang-Tháp Chàm",
//                    Status = "ACTIVE",
//                    City = cities[57], // Ninh Thuận
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Ninh Thuận Vĩnh Hy Bay",
//                    Status = "ACTIVE",
//                    City = cities[57], // Ninh Thuận
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Bình Phong Nha Caves",
//                    Status = "ACTIVE",
//                    City = cities[58], // Quảng Bình
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Bình Phong Nha-Kẻ Bàng National Park",
//                    Status = "ACTIVE",
//                    City = cities[58], // Quảng Bình
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Nam Hội An Ancient Town",
//                    Status = "ACTIVE",
//                    City = cities[59], // Quảng Nam
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Nam Mỹ Sơn Sanctuary",
//                    Status = "ACTIVE",
//                    City = cities[59], // Quảng Nam
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ngãi Lý Sơn Island",
//                    Status = "ACTIVE",
//                    City = cities[60], // Quảng Ngãi
//                    Company = companies[2]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ngãi Côn Đảo National Park",
//                    Status = "ACTIVE",
//                    City = cities[60], // Quảng Ngãi
//                    Company = companies[3]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ninh Hạ Long Bay",
//                    Status = "ACTIVE",
//                    City = cities[61], // Quảng Ninh
//                    Company = companies[4]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Ninh Yên Tử Mountain",
//                    Status = "ACTIVE",
//                    City = cities[61], // Quảng Ninh
//                    Company = companies[0]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Trị Citadel of Quảng Trị",
//                    Status = "ACTIVE",
//                    City = cities[62], // Quảng Trị
//                    Company = companies[1]
//                },
//                new Station
//                {
//                     StationID = Guid.NewGuid(),
//                    Name = "Quảng Trị Vinh Moc Tunnels",
//                    Status = "ACTIVE",
//                    City = cities[62], // Quảng Trị
//                    Company = companies[2]
//                }
//            };

//            List<Route> routes = new()
//            {
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[55],
//                    StartLocation = "Bưu điện trung tâm TP. Hồ Chí Minh",
//                    EndLocation = " Bến xe Huế",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[3],
//                    ToCity = cities[57],
//                    StartLocation = "Ngã ba Cái Mép",
//                    EndLocation = "Quan 10",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[1],
//                    ToCity = cities[57],
//                    StartLocation = "Bãi Trước",
//                    EndLocation = "Sân bay Tân Sơn Nhất",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[12],
//                    StartLocation = " Bến xe miền tây",
//                    EndLocation = " Bến xe Cần Thơ",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[30],
//                    StartLocation = "Công viên 23/9, Quận 1",
//                    EndLocation = "Bến xe Nha Trang",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[15],
//                    StartLocation = "Bến Xe Miền Đông",
//                    EndLocation = "Bến Xe Đà Nẵng",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[6],
//                    ToCity = cities[57],
//                    StartLocation = " Bến Tre Bus Station",
//                    EndLocation = "Bến Xe Miền Tây",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[34],
//                    StartLocation = "Bến Xe Miền Đông",
//                    EndLocation = "Bến Xe Đà Lạt",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[57],
//                    ToCity = cities[1],
//                    StartLocation = "Bến Xe Miền Đông",
//                    EndLocation = "Bến Xe Vũng Tàu",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[0], // An Giang
//                    ToCity = cities[11], // Cà Mau
//                    StartLocation = "Bến Xe An Giang",
//                    EndLocation = "Bến Xe Cà Mau",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[24], // Hà Nội
//                    ToCity = cities[14], // Đà Nẵng
//                    StartLocation = "Bến Xe Giáp Bát",
//                    EndLocation = "Bến Xe Đà Nẵng",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[54], // TP. Hồ Chí Minh
//                    ToCity = cities[36], // Kiên Giang
//                    StartLocation = "Bến Xe Miền Tây",
//                    EndLocation = "Bến Xe Rạch Giá",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[41], // Lào Cai
//                    ToCity = cities[22], // Hà Giang
//                    StartLocation = "Bến Xe Lào Cai",
//                    EndLocation = "Bến Xe Hà Giang",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[2], // Bạc Liêu
//                    ToCity = cities[13], // Cần Thơ
//                    StartLocation = "Bến Xe Bạc Liêu",
//                    EndLocation = "Bến Xe Cần Thơ",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[6], // Bến Tre
//                    ToCity = cities[53], // Tiền Giang
//                    StartLocation = "Bến Xe Bến Tre",
//                    EndLocation = "Bến Xe Mỹ Tho",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[29], // Hải Phòng
//                    ToCity = cities[15], // Đắk Lắk
//                    StartLocation = "Bến Xe Hải Phòng",
//                    EndLocation = "Bến Xe Buôn Ma Thuột",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[35], // Kon Tum
//                    ToCity = cities[21], // Gia Lai
//                    StartLocation = "Bến Xe Kon Tum",
//                    EndLocation = "Bến Xe Gia Lai",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[46], // Nghệ An
//                    ToCity = cities[25], // Hà Tĩnh
//                    StartLocation = "Bến Xe Vinh",
//                    EndLocation = "Bến Xe Hà Tĩnh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[10], // Cao Bằng
//                    ToCity = cities[41], // Lào Cai
//                    StartLocation = "Bến Xe Cao Bằng",
//                    EndLocation = "Bến Xe Lào Cai",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[32], // Hòa Bình
//                    ToCity = cities[38], // Lai Châu
//                    StartLocation = "Bến Xe Hòa Bình",
//                    EndLocation = "Bến Xe Lai Châu",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[19], // Đắk Nông
//                    ToCity = cities[52], // Tây Ninh
//                    StartLocation = "Bến Xe Gia Nghĩa",
//                    EndLocation = "Bến Xe Tây Ninh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[9], // Cần Thơ
//                    ToCity = cities[36], // Kiên Giang
//                    StartLocation = "Bến Xe Cần Thơ",
//                    EndLocation = "Bến Xe Rạch Giá",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[28], // Hải Dương
//                    ToCity = cities[29], // Hải Phòng
//                    StartLocation = "Bến Xe Hải Dương",
//                    EndLocation = "Bến Xe Hải Phòng",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[43], // Ninh Bình
//                    ToCity = cities[48], // Phú Thọ
//                    StartLocation = "Bến Xe Ninh Bình",
//                    EndLocation = "Bến Xe Việt Trì",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[33], // Hưng Yên
//                    ToCity = cities[26], // Hải Dương
//                    StartLocation = "Bến Xe Hưng Yên",
//                    EndLocation = "Bến Xe Hải Dương",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[18], // Đắk Lắk
//                    ToCity = cities[35], // Kon Tum
//                    StartLocation = "Bến Xe Buôn Ma Thuột",
//                    EndLocation = "Bến Xe Kon Tum",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[20], // Điện Biên
//                    ToCity = cities[45], // Nghệ An
//                    StartLocation = "Bến Xe Điện Biên Phủ",
//                    EndLocation = "Bến Xe Vinh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[23], // Hà Nam
//                    ToCity = cities[43], // Ninh Bình
//                    StartLocation = "Bến Xe Phủ Lý",
//                    EndLocation = "Bến Xe Ninh Bình",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[12], // Đắk Nông
//                    ToCity = cities[31], // Hòa Bình
//                    StartLocation = "Bến Xe Gia Nghĩa",
//                    EndLocation = "Bến Xe Hòa Bình",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[39], // Lâm Đồng
//                    ToCity = cities[54], // TP. Hồ Chí Minh
//                    StartLocation = "Bến Xe Đà Lạt",
//                    EndLocation = "Bến Xe Miền Tây",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[47], // Phú Yên
//                    ToCity = cities[51], // Quảng Bình
//                    StartLocation = "Bến Xe Tuy Hòa",
//                    EndLocation = "Bến Xe Đồng Hới",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[50], // Quảng Nam
//                    ToCity = cities[14], // Đà Nẵng
//                    StartLocation = "Bến Xe Tam Kỳ",
//                    EndLocation = "Bến Xe Đà Nẵng",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[55], // Vĩnh Long
//                    ToCity = cities[6], // Bến Tre
//                    StartLocation = "Bến Xe Vĩnh Long",
//                    EndLocation = "Bến Xe Bến Tre",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[11], // Cà Mau
//                    ToCity = cities[37], // Kiên Giang
//                    StartLocation = "Bến Xe Cà Mau",
//                    EndLocation = "Bến Xe Rạch Giá",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[24], // Hà Tĩnh
//                    ToCity = cities[27], // Hậu Giang
//                    StartLocation = "Bến Xe Hà Tĩnh",
//                    EndLocation = "Bến Xe Vị Thanh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[42], // Nam Định
//                    ToCity = cities[23], // Hà Nam
//                    StartLocation = "Bến Xe Nam Định",
//                    EndLocation = "Bến Xe Phủ Lý",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[49], // Quảng Ngãi
//                    ToCity = cities[50], // Quảng Nam
//                    StartLocation = "Bến Xe Quảng Ngãi",
//                    EndLocation = "Bến Xe Tam Kỳ",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[22], // Gia Lai
//                    ToCity = cities[12], // Đắk Nông
//                    StartLocation = "Bến Xe Gia Lai",
//                    EndLocation = "Bến Xe Gia Nghĩa",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[40], // Lạng Sơn
//                    ToCity = cities[43], // Ninh Bình
//                    StartLocation = "Bến Xe Lạng Sơn",
//                    EndLocation = "Bến Xe Ninh Bình",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[13], // Đồng Nai
//                    ToCity = cities[39], // Lâm Đồng
//                    StartLocation = "Bến Xe Biên Hòa",
//                    EndLocation = "Bến Xe Đà Lạt",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[21], // Đồng Tháp
//                    ToCity = cities[8], // Cà Mau
//                    StartLocation = "Bến Xe Cao Lãnh",
//                    EndLocation = "Bến Xe Cà Mau",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[46], // Phú Thọ
//                    ToCity = cities[20], // Điện Biên
//                    StartLocation = "Bến Xe Việt Trì",
//                    EndLocation = "Bến Xe Điện Biên Phủ",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[44], // Ninh Thuận
//                    ToCity = cities[47], // Phú Yên
//                    StartLocation = "Bến Xe Phan Rang",
//                    EndLocation = "Bến Xe Tuy Hòa",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[28], // Hậu Giang
//                    ToCity = cities[55], // Vĩnh Long
//                    StartLocation = "Bến Xe Vị Thanh",
//                    EndLocation = "Bến Xe Vĩnh Long",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[38], // Lào Cai
//                    ToCity = cities[40], // Lạng Sơn
//                    StartLocation = "Bến Xe Lào Cai",
//                    EndLocation = "Bến Xe Lạng Sơn",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[7], // Bình Định
//                    ToCity = cities[49], // Quảng Ngãi
//                    StartLocation = "Bến Xe Quy Nhơn",
//                    EndLocation = "Bến Xe Quảng Ngãi",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[36], // Lai Châu
//                    ToCity = cities[5], // Bắc Giang
//                    StartLocation = "Bến Xe Lai Châu",
//                    EndLocation = "Bến Xe Bắc Giang",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[30], // Hà Giang
//                    ToCity = cities[19], // Đắk Nông
//                    StartLocation = "Bến Xe Hà Giang",
//                    EndLocation = "Bến Xe Gia Nghĩa",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[15], // Đắk Lắk
//                    ToCity = cities[22], // Gia Lai
//                    StartLocation = "Bến Xe Buôn Ma Thuột",
//                    EndLocation = "Bến Xe Gia Lai",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[26], // Hải Phòng
//                    ToCity = cities[41], // Lạng Sơn
//                    StartLocation = "Bến Xe Hải Phòng",
//                    EndLocation = "Bến Xe Lạng Sơn",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[10], // Bình Thuận
//                    ToCity = cities[44], // Ninh Thuận
//                    StartLocation = "Bến Xe Phan Thiết",
//                    EndLocation = "Bến Xe Phan Rang",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[3], // Bắc Kạn
//                    ToCity = cities[35], // Kon Tum
//                    StartLocation = "Bến Xe Bắc Kạn",
//                    EndLocation = "Bến Xe Kon Tum",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[48], // Quảng Ninh
//                    ToCity = cities[29], // Hải Dương
//                    StartLocation = "Bến Xe Hạ Long",
//                    EndLocation = "Bến Xe Hải Dương",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[1], // Bà Rịa - Vũng Tàu
//                    ToCity = cities[6], // Bắc Ninh
//                    StartLocation = "Bến Xe Vũng Tàu",
//                    EndLocation = "Bến Xe Bắc Ninh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[25], // Hà Nội
//                    ToCity = cities[31], // Hà Nam
//                    StartLocation = "Bến Xe Mỹ Đình",
//                    EndLocation = "Bến Xe Phủ Lý",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[2], // Bắc Giang
//                    ToCity = cities[32], // Hà Tĩnh
//                    StartLocation = "Bến Xe Bắc Giang",
//                    EndLocation = "Bến Xe Hà Tĩnh",
//                    Status = "ACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[45], // Phú Thọ
//                    ToCity = cities[4], // Bắc Ninh
//                    StartLocation = "Bến Xe Việt Trì",
//                    EndLocation = "Bến Xe Bắc Ninh",
//                    Status = "INACTIVE"
//                },
//                new Route
//                {
//                    RouteID = Guid.NewGuid(),
//                    FromCity = cities[18], // Cao Bằng
//                    ToCity = cities[51], // Quảng Trị
//                    StartLocation = "Bến Xe Cao Bằng",
//                    EndLocation = "Bến Xe Đông Hà",
//                    Status = "ACTIVE"
//                }
//            };
//            List<Station_Route> station_Routes = new()
//            {
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[0],
//                    Station = stations[0],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[0],

//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[0],

//                    Station = stations[4],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[1],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[1],
//                    Station = stations[0],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[1],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[1],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[1],
//                    Station = stations[4],
//                    Status = "ACTIVE",
//                    OrderInRoute =5
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[2],
//                    Station = stations[0],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[2],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[2],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute =1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[2],
//                    Station = stations[3],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[3],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[3],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[3],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[3],
//                    Station = stations[7],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[4],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[4],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[4],
//                    Station = stations[3],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[4],
//                    Station = stations[7],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[5],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[5],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[5],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute =1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[0],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[4],
//                    Status = "ACTIVE",
//                    OrderInRoute =5
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[7],
//                    Status = "ACTIVE",
//                    OrderInRoute =6
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[3],
//                    Status = "ACTIVE",
//                    OrderInRoute =7
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[6],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute =8
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[7],
//                    Station = stations[0],
//                    Status = "ACTIVE",
//                    OrderInRoute =3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[7],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute =2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[7],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute =4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[7],
//                    Station = stations[4],
//                    Status = "ACTIVE",
//                    OrderInRoute =1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[7],
//                    Station = stations[7],
//                    Status = "ACTIVE",
//                    OrderInRoute =5
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[13],
//                    Station = stations[20],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[13],
//                    Station = stations[45],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[13],
//                    Station = stations[68],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[13],
//                    Station = stations[92],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[13],
//                    Station = stations[115],
//                    Status = "ACTIVE",
//                    OrderInRoute = 5
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[16],
//                    Station = stations[30],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[16],
//                    Station = stations[48],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[16],
//                    Station = stations[75],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[16],
//                    Station = stations[102],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[19],
//                    Station = stations[11],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[19],
//                    Station = stations[27],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[19],
//                    Station = stations[53],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[19],
//                    Station = stations[89],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[28],
//                    Station = stations[4],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[28],
//                    Station = stations[18],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[28],
//                    Station = stations[41],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[28],
//                    Station = stations[67],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[18],
//                    Station = stations[22],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[18],
//                    Station = stations[49],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[18],
//                    Station = stations[78],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[18],
//                    Station = stations[110],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[14],
//                    Station = stations[8],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[14],
//                    Station = stations[31],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[14],
//                    Station = stations[57],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[14],
//                    Station = stations[94],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[23],
//                    Station = stations[2],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[23],
//                    Station = stations[24],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[23],
//                    Station = stations[46],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[23],
//                    Station = stations[79],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[20],
//                    Station = stations[15],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[20],
//                    Station = stations[36],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[20],
//                    Station = stations[62],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[20],
//                    Station = stations[87],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[11],
//                    Station = stations[5],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[11],
//                    Station = stations[26],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[11],
//                    Station = stations[51],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[11],
//                    Station = stations[81],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[17],
//                    Station = stations[13],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[17],
//                    Station = stations[34],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[17],
//                    Station = stations[59],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[17],
//                    Station = stations[91],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[8],
//                    Station = stations[20],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[8],
//                    Station = stations[42],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[8],
//                    Station = stations[69],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[8],
//                    Station = stations[96],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[15],
//                    Station = stations[9],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[15],
//                    Station = stations[33],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[15],
//                    Station = stations[56],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[15],
//                    Station = stations[89],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[22],
//                    Station = stations[1],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[22],
//                    Station = stations[23],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[22],
//                    Station = stations[45],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[22],
//                    Station = stations[77],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[21],
//                    Station = stations[11],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[21],
//                    Station = stations[32],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[21],
//                    Station = stations[54],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[21],
//                    Station = stations[85],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[24],
//                    Station = stations[16],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[24],
//                    Station = stations[37],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[24],
//                    Station = stations[63],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[24],
//                    Station = stations[88],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[12],
//                    Station = stations[6],
//                    Status = "ACTIVE",
//                    OrderInRoute = 1
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[12],
//                    Station = stations[27],
//                    Status = "ACTIVE",
//                    OrderInRoute = 2
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[12],
//                    Station = stations[52],
//                    Status = "ACTIVE",
//                    OrderInRoute = 3
//                },
//                new Station_Route
//                {
//                    Station_RouteID = Guid.NewGuid(),
//                    Route = routes[12],
//                    Station = stations[82],
//                    Status = "ACTIVE",
//                    OrderInRoute = 4
//                }
//            };
//            List<Route_Company> route_Companies = new()
//            {
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[0],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[1],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[2],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[3],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[17],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[29],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[41],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[8],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[10],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[32],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[22],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[47],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[6],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[19],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[38],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[13],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[52],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[25],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[11],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[24],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[43],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[7],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[31],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[14],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[27],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[5],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[36],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[18],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[9],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[28],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[40],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[52],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[16],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[34],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[21],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[46],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[12],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[35],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[13],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[37],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[23],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[45],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[8],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[29],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[51],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[15],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[39],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[4],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[53],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[17],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[41],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[26],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[6],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[32],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[48],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[11],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[42],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[22],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[55],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[19],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[38],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[50],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[10],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[44],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[30],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[2],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[47],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[25],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[54],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[20],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[43],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[7],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[33],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[14],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[49],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[3],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[27],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[40],
//                    Status = "ACTIVE"
//                },
//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[56],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[24],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[9],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[36],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[5],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[1],
//                    Route = routes[31],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[3],
//                    Route = routes[18],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[0],
//                    Route = routes[47],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[4],
//                    Route = routes[12],
//                    Status = "ACTIVE"
//                },

//                new Route_Company
//                {
//                    Route_CompanyID = Guid.NewGuid(),
//                    Company = companies[2],
//                    Route = routes[41],
//                    Status = "ACTIVE"
//                }
//            };
//            List<Trip> trips1 = new List<Trip>();
//            Trip firstTrip1 = new Trip
//            {
//                TripID = Guid.NewGuid(),
//                TemplateID = Guid.NewGuid(),
//                Route_Company = route_Companies[0],
//                IsTemplate = true,
//                StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                EndTime = new DateTime(2024, 6, 19, 15, 30, 0),
//                Status = "ACTIVE"
//            };
//            Trip firstTrip2 = new Trip
//            {
//                TripID = Guid.NewGuid(),
//                TemplateID = Guid.NewGuid(),
//                Route_Company = route_Companies[0],
//                IsTemplate = true,
//                StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                EndTime = new DateTime(2024, 6, 19, 15, 30, 0),
//                Status = "ACTIVE"
//            };

//            // Add the first trip to the list
//            Trip firstTrip3 = new Trip
//            {
//                TripID = Guid.NewGuid(),
//                TemplateID = Guid.NewGuid(),
//                Route_Company = route_Companies[0],
//                IsTemplate = true,
//                StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                EndTime = new DateTime(2024, 6, 19, 15, 30, 0),
//                Status = "ACTIVE"
//            };

//            // Add the first trip to the list
//            trips1.Add(firstTrip1);
//            // Ad1d the first trip to the list
//            trips1.Add(firstTrip2);
//            trips1.Add(firstTrip3);
//            List<Trip> trips = new()
//            {
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[0],
//                    IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 19, 15, 30, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),                  
//                    Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 1),
//                    EndTime = new DateTime(2024, 7, 6, 18, 45, 1),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                     Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 2),
//                    EndTime = new DateTime(2024, 7, 6, 18, 45, 2),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                     Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 3),
//                    EndTime = new DateTime(2024, 6, 25, 11, 30, 3),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                     Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 4),
//                    EndTime = new DateTime(2024, 7, 7, 17, 45, 4),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
//                    EndTime = new DateTime(2024, 7, 1, 23, 59, 59),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[0],
//                    IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 5),
//                    EndTime = new DateTime(2024, 6, 25, 11, 30, 5),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[0],
//                     IsTemplate = false,
//                    TemplateID = trips1[0].TripID,
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 5),
//                    EndTime = new DateTime(2024, 6, 25, 11, 30, 5),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[1],
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 5),
//                    EndTime = new DateTime(2024, 7, 7, 17, 45, 5),
//                    Status ="ACTIVE",
//                },
//                new Trip
//                {
//                     TripID = Guid.NewGuid(),
//                     Route_Company = route_Companies[2],
//                     StartTime = new DateTime(2024, 6, 15, 11, 30, 5),
//                     EndTime = new DateTime(2024, 6, 15, 23, 59, 59), // Fixed end time
//                     Status ="ACTIVE"
//                },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[2],
//                        StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
//                        EndTime = new DateTime(2024, 7, 2, 23, 59, 59),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[2],
//                        StartTime = new DateTime(2024, 7, 5, 17, 45, 1),
//                        EndTime = new DateTime(2024, 7, 8, 17, 45, 1),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[2],
//                        StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
//                        EndTime = new DateTime(2024, 7, 8, 17, 45, 0),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[3],
//                        StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
//                        EndTime = new DateTime(2024, 7, 13, 14, 20, 0),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[3],
//                        StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                        EndTime = new DateTime(2024, 6, 17, 17, 30, 0),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[24],
//                        StartTime = new DateTime(2024, 7, 12, 14, 20, 1),
//                        EndTime = new DateTime(2024, 7, 13, 14, 20, 1),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[23],
//                        StartTime = new DateTime(2024, 7, 5, 17, 45, 1),
//                        EndTime = new DateTime(2024, 7, 9, 17, 45, 1),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[22],
//                        StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
//                        EndTime = new DateTime(2024, 7, 9, 17, 45, 0),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[3],
//                        StartTime = new DateTime(2024, 6, 15, 11, 30, 1),
//                        EndTime = new DateTime(2024, 6, 17, 17, 30, 1),
//                        Status = "ACTIVE"
//                    },
//                new Trip
//                    {
//                        TripID = Guid.NewGuid(),
//                        Route_Company = route_Companies[20],
//                        StartTime = new DateTime(2024, 6, 15, 11, 30, 1),
//                        EndTime = new DateTime(2024, 6, 15, 18, 30, 1),
//                        Status = "ACTIVE"
//                    },

//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 15, 18, 30, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[27],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 23, 9, 0, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[18],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
//                    EndTime = new DateTime(2024, 7, 14, 14, 20, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[5],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 1),
//                    EndTime = new DateTime(2024, 6, 25, 11, 30, 1),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[8],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 1),
//                    EndTime = new DateTime(2024, 7, 15, 14, 20, 1),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[9],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
//                    EndTime = new DateTime(2024, 7, 15, 14, 20, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[10],
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
//                    EndTime = new DateTime(2024, 7, 10, 17, 45, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 21, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 1),
//                    EndTime = new DateTime(2024, 7, 13, 14, 20, 1),
//                    Status ="ACTIVE"
//                },
//                  new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
//                    EndTime = new DateTime(2024, 7, 14, 14, 20, 0),
//                    Status ="ACTIVE"
//                },
//                   new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 21, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                    new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[15],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 23, 9, 0, 0),
//                    Status ="ACTIVE"
//                },
//                     new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[16],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 18, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                      new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[17],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 1),
//                    EndTime = new DateTime(2024, 6, 18, 11, 30, 1),
//                    Status ="ACTIVE"
//                },
//                       new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[18],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 2),
//                    EndTime = new DateTime(2024, 6, 18, 11, 30, 2),
//                    Status ="ACTIVE"
//                },
//                        new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[19],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 22, 9, 0, 0),
//                    Status ="ACTIVE"
//                },
//                         new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
//                    EndTime = new DateTime(2024, 7, 11, 17, 45, 0),
//                    Status ="ACTIVE"
//                },
//                          new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
//                    Status ="ACTIVE"
//                },
//                 new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[22],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 1),
//                    EndTime = new DateTime(2024, 6, 16, 11, 30, 1),
//                    Status ="ACTIVE"
//                },
//                  new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 16, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                   new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
//                    Status ="ACTIVE"
//                },
//                    new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
//                    EndTime = new DateTime(2024, 6, 16, 11, 30, 0),
//                    Status ="ACTIVE"
//                },
//                    new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[26],
//                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
//                    EndTime = new DateTime(2024, 7, 6, 17, 45, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[27],
//                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
//                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[28],
//                    StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
//                    EndTime = new DateTime(2024, 7, 1, 23, 59, 59),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[28],
//                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
//                    EndTime = new DateTime(2024, 7, 13, 14, 20, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[29],
//                    StartTime = new DateTime(2024, 6, 20, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 20, 15, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[30],
//                    StartTime = new DateTime(2024, 7, 10, 17, 30, 0),
//                    EndTime = new DateTime(2024, 7, 11, 7, 30, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[30],
//                    StartTime = new DateTime(2024, 8, 1, 11, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[31],
//                    StartTime = new DateTime(2024, 6, 25, 14, 0, 0),
//                    EndTime = new DateTime(2024, 6, 26, 2, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[32],
//                    StartTime = new DateTime(2024, 7, 20, 6, 30, 0),
//                    EndTime = new DateTime(2024, 7, 20, 16, 30, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[33],
//                    StartTime = new DateTime(2024, 6, 22, 18, 0, 0),
//                    EndTime = new DateTime(2024, 6, 23, 6, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[34],
//                    StartTime = new DateTime(2024, 7, 15, 10, 30, 0),
//                    EndTime = new DateTime(2024, 7, 16, 0, 30, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[35],
//                    StartTime = new DateTime(2024, 8, 10, 13, 0, 0),
//                    EndTime = new DateTime(2024, 8, 11, 1, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[30],
//                    StartTime = new DateTime(2024, 6, 28, 21, 0, 0),
//                    EndTime = new DateTime(2024, 6, 29, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[32],
//                    StartTime = new DateTime(2024, 7, 25, 7, 0, 0),
//                    EndTime = new DateTime(2024, 7, 25, 19, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[33],
//                    StartTime = new DateTime(2024, 6, 18, 12, 0, 0),
//                    EndTime = new DateTime(2024, 6, 18, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 7, 3, 22, 30, 0),
//                    EndTime = new DateTime(2024, 7, 4, 6, 30, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 8, 5, 15, 0, 0),
//                    EndTime = new DateTime(2024, 8, 6, 3, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 6, 30, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 30, 16, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 7, 18, 19, 0, 0),
//                    EndTime = new DateTime(2024, 7, 19, 3, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[10],
//                    StartTime = new DateTime(2024, 6, 15, 5, 0, 0),
//                    EndTime = new DateTime(2024, 6, 15, 15, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[9],
//                    StartTime = new DateTime(2024, 7, 12, 20, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 4, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[8],
//                    StartTime = new DateTime(2024, 8, 15, 10, 0, 0),
//                    EndTime = new DateTime(2024, 8, 15, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 6, 23, 23, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[6],
//                    StartTime = new DateTime(2024, 7, 27, 14, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[5],
//                    StartTime = new DateTime(2024, 6, 15, 5, 0, 0),
//                    EndTime = new DateTime(2024, 6, 15, 15, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[4],
//                    StartTime = new DateTime(2024, 7, 12, 20, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 4, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[3],
//                    StartTime = new DateTime(2024, 8, 15, 10, 0, 0),
//                    EndTime = new DateTime(2024, 8, 15, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[2],
//                    StartTime = new DateTime(2024, 6, 23, 23, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[1],
//                    StartTime = new DateTime(2024, 7, 27, 14, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 6, 20, 0, 0, 0),
//                    EndTime = new DateTime(2024, 6, 20, 8, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 7, 6, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 6, 20, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[22],
//                    StartTime = new DateTime(2024, 8, 1, 6, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 14, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 6, 26, 18, 0, 0),
//                    EndTime = new DateTime(2024, 6, 27, 2, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 7, 22, 9, 0, 0),
//                    EndTime = new DateTime(2024, 7, 22, 17, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[19],
//                    StartTime = new DateTime(2024, 6, 12, 20, 0, 0),
//                    EndTime = new DateTime(2024, 6, 13, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[18],
//                    StartTime = new DateTime(2024, 7, 3, 11, 0, 0),
//                    EndTime = new DateTime(2024, 7, 3, 17, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[17],
//                    StartTime = new DateTime(2024, 8, 10, 23, 0, 0),
//                    EndTime = new DateTime(2024, 8, 11, 7, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[16],
//                    StartTime = new DateTime(2024, 6, 28, 14, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 22, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[15],
//                    StartTime = new DateTime(2024, 7, 15, 7, 0, 0),
//                    EndTime = new DateTime(2024, 7, 15, 15, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 6, 12, 20, 0, 0),
//                    EndTime = new DateTime(2024, 6, 13, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 7, 3, 11, 0, 0),
//                    EndTime = new DateTime(2024, 7, 3, 17, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 8, 10, 23, 0, 0),
//                    EndTime = new DateTime(2024, 8, 11, 7, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 6, 28, 14, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 22, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[10],
//                    StartTime = new DateTime(2024, 7, 15, 7, 0, 0),
//                    EndTime = new DateTime(2024, 7, 15, 15, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[9],
//                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
//                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[8],
//                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
//                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
//                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[6],
//                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
//                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[5],
//                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
//                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[4],
//                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
//                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[3],
//                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
//                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
//                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[22],
//                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
//                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
//                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[30],
//                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
//                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[27],
//                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
//                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
//                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
//                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
//                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[30],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[31],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[32],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[33],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[34],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[35],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[3],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[1],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[2],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[19],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[19],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[10],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[15],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[16],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[17],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[18],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[19],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[22],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[26],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[27],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[28],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[5],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
//                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[7],
//                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
//                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[8],
//                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
//                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
//                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
//                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[20],
//                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[21],
//                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
//                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[24],
//                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
//                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[14],
//                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
//                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[23],
//                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
//                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[17],
//                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
//                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[15],
//                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
//                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[13],
//                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[12],
//                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
//                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[11],
//                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[26],
//                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
//                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[25],
//                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
//                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[1],
//                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
//                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[0],
//                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
//                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[2],
//                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
//                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
//                    Status = "ACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[3],
//                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
//                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
//                    Status = "INACTIVE"
//                },
//                new Trip
//                {
//                    TripID = Guid.NewGuid(),
//                    Route_Company = route_Companies[4],
//                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
//                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
//                    Status = "ACTIVE"
//                }
//            };
//            List<ServiceType> serviceTypes = new()
//            {
//                new ServiceType
//                {
//                    ServiceTypeID = Guid.NewGuid(),
//                    Name = "Đồ ăn",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                    ServiceTypeID = Guid.NewGuid(),
//                    Name = "Đồ uống",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "Ăn vặt",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "Đồ ăn nhanh",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "ATM",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "Khu vui chơi trẻ em",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "Dịch vụ gửi đồ và hành lý",
//                    Status = "ACTIVE"
//                },
//                new ServiceType
//                {
//                   ServiceTypeID = Guid.NewGuid(),
//                    Name = "Xe máy",
//                    Status = "ACTIVE"
//                }
//            };
//            List<Utility> utilities = new()
//            {
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Nhà vệ sinh",
//                    Description ="Xe được trang bị nhà vệ sinh sạch sẽ.",
//                    Status = "ACTIVE"
//                },
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Chăn",
//                    Description ="Xe cung cấp chăn để hành khách có thể giữ ấm trong suốt hành trình, đặc biệt là vào ban đêm hoặc khi điều hòa nhiệt độ lạnh.",

//                    Status = "ACTIVE"
//                },
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Wi-Fi miễn phí",
//                    Description ="Xe cung cấp dịch vụ Wi-Fi miễn phí để hành khách có thể sử dụng internet trong suốt chuyến đi",
//                    Status = "ACTIVE"
//                },
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Ổ cắm điện và cổng USB",
//                    Description ="Các ổ cắm điện và cổng USB được trang bị để hành khách có thể sạc điện thoại, máy tính bảng hoặc laptop.",
//                    Status = "ACTIVE"
//                },
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Nước uống và khăn lạnh",
//                    Description ="Nhà xe cung cấp nước uống đóng chai và khăn lạnh miễn phí cho hành khách.",
//                    Status = "ACTIVE"
//                },
//                new Utility
//                {
//                    UtilityID = Guid.NewGuid(),
//                    Name="Rèm che riêng tư",
//                    Description ="Trên xe có rèm che riêng tư giúp hành khách có không gian riêng tư hơn.",
//                    Status = "ACTIVE"
//                },
//            };
//            List<Trip_Utility> utilityInTrips = new()
//            {
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[0],
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[1],
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[2],
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[1],
//                    Status = "ACTIVE",
//                    Trip = trips[2]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[1],
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[2],
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                },
//                new Trip_Utility
//                {
//                    Trip_UtilityID = Guid.NewGuid(),
//                    Utility = utilities[0],
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                }
//            };
//            List<Service> services = new()
//            {
//                new Service
//                {
//                    ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[0],
//                    Name = "Mi tom",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[0],
//                    Name = "Pho",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[1],
//                    Name = "Bun bo",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[2],
//                    Name = "Com xeo",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[1],
//                    Name = "Com phan",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[0],
//                    Name = "Mi tom",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[3],
//                    Name = "Hamburger",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[3],
//                    Name = "Sandwich",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[3],
//                    Name = "hotdog",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[2],
//                    Name = "Trái cây",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[1],
//                    Name = "Cà phê",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[1],
//                    Name = "Nước trái cây",
//                    Status = "ACTIVE"
//                },
//                new Service
//                {
//                     ServiceID = Guid.NewGuid(),
//                    ServiceType=serviceTypes[1],
//                    Name = "Trà",
//                    Status = "ACTIVE"
//                },
//            };
//            List<TicketType> types = new()
//            {
//                new TicketType
//                {
//                    TicketTypeID = Guid.NewGuid(),
//                    Name = "Hàng đầu",
//                    Status = "ACTIVE"
//                },
//                new TicketType
//                {
//                    TicketTypeID = Guid.NewGuid(),
//                    Name = "Hàng giữa",
//                    Status = "ACTIVE"
//                },
//                new TicketType
//                {
//                    TicketTypeID = Guid.NewGuid(),
//                    Name = "Hàng sau",
//                    Status = "ACTIVE"
//                },
//            };
//            List<TicketType_Trip> ticketType_Trips = new()
//            {
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0],
//                    Trip = trips[0],
//                    Price = 120000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0],
//                    Trip = trips[1],
//                    Price = 150000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2],
//                    Trip = trips[2],
//                    Price = 130000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0],
//                    Trip = trips[3],
//                    Price = 100000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[10], // Assuming trips[10] is a valid Trip
//                    Price = 85000,
//                    Status = "ACTIVE",
//                    Quantity = 15
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[27], // Assuming trips[27] is a valid Trip
//                    Price = 140000,
//                    Status = "INACTIVE",
//                    Quantity = 22
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[89], // Assuming trips[89] is a valid Trip
//                    Price = 175000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[17], // Assuming trips[17] is a valid Trip
//                    Price = 120000,
//                    Status = "INACTIVE",
//                    Quantity = 28
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[42], // Assuming trips[42] is a valid Trip
//                    Price = 85000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[68], // Assuming trips[68] is a valid Trip
//                    Price = 190000,
//                    Status = "INACTIVE",
//                    Quantity = 18
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[99], // Assuming trips[99] is a valid Trip
//                    Price = 160000,
//                    Status = "ACTIVE",
//                    Quantity = 25
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[51], // Assuming trips[51] is a valid Trip
//                    Price = 155000,
//                    Status = "INACTIVE",
//                    Quantity = 14
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[80], // Assuming trips[80] is a valid Trip
//                    Price = 180000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[7], // Assuming trips[7] is a valid Trip
//                    Price = 90000,
//                    Status = "INACTIVE",
//                    Quantity = 26
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[92], // Assuming trips[92] is a valid Trip
//                    Price = 125000,
//                    Status = "ACTIVE",
//                    Quantity = 19
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[15], // Assuming trips[15] is a valid Trip
//                    Price = 75000,
//                    Status = "ACTIVE",
//                    Quantity = 12
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[38], // Assuming trips[38] is a valid Trip
//                    Price = 120000,
//                    Status = "INACTIVE",
//                    Quantity = 21
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[62], // Assuming trips[62] is a valid Trip
//                    Price = 85000,
//                    Status = "ACTIVE",
//                    Quantity = 16
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[84], // Assuming trips[84] is a valid Trip
//                    Price = 190000,
//                    Status = "INACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[23], // Assuming trips[23] is a valid Trip
//                    Price = 160000,
//                    Status = "ACTIVE",
//                    Quantity = 30
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[65], // Assuming trips[65] is a valid Trip
//                    Price = 70000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[42], // Assuming trips[42] is a valid Trip
//                    Price = 130000,
//                    Status = "INACTIVE",
//                    Quantity = 24
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[91], // Assuming trips[91] is a valid Trip
//                    Price = 180000,
//                    Status = "ACTIVE",
//                    Quantity = 18
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[12], // Assuming trips[12] is a valid Trip
//                    Price = 95000,
//                    Status = "INACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[77], // Assuming trips[77] is a valid Trip
//                    Price = 150000,
//                    Status = "ACTIVE",
//                    Quantity = 27
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[30], // Assuming trips[30] is a valid Trip
//                    Price = 80000,
//                    Status = "ACTIVE",
//                    Quantity = 22
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[58], // Assuming trips[58] is a valid Trip
//                    Price = 115000,
//                    Status = "INACTIVE",
//                    Quantity = 16
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[83], // Assuming trips[83] is a valid Trip
//                    Price = 90000,
//                    Status = "ACTIVE",
//                    Quantity = 14
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[45], // Assuming trips[45] is a valid Trip
//                    Price = 170000,
//                    Status = "INACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[72], // Assuming trips[72] is a valid Trip
//                    Price = 140000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[11], // Assuming trips[11] is a valid Trip
//                    Price = 80000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[34], // Assuming trips[34] is a valid Trip
//                    Price = 125000,
//                    Status = "INACTIVE",
//                    Quantity = 14
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[69], // Assuming trips[69] is a valid Trip
//                    Price = 150000,
//                    Status = "ACTIVE",
//                    Quantity = 17
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[92], // Assuming trips[92] is a valid Trip
//                    Price = 105000,
//                    Status = "INACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[55], // Assuming trips[55] is a valid Trip
//                    Price = 90000,
//                    Status = "ACTIVE",
//                    Quantity = 15
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[18], // Assuming trips[18] is a valid Trip
//                    Price = 82000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[41], // Assuming trips[41] is a valid Trip
//                    Price = 128000,
//                    Status = "INACTIVE",
//                    Quantity = 16
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[74], // Assuming trips[74] is a valid Trip
//                    Price = 155000,
//                    Status = "ACTIVE",
//                    Quantity = 19
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[62], // Assuming trips[62] is a valid Trip
//                    Price = 95000,
//                    Status = "INACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[29], // Assuming trips[29] is a valid Trip
//                    Price = 112000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[3], // Trip with index 3
//                    Price = 95000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[4], // Trip with index 4
//                    Price = 120000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[5], // Trip with index 5
//                    Price = 150000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[6], // Trip with index 6
//                    Price = 90000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[8], // Trip with index 8
//                    Price = 130000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[9], // Trip with index 9
//                    Price = 180000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[13], // Trip with index 13
//                    Price = 85000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[14], // Trip with index 14
//                    Price = 110000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[16], // Trip with index 16
//                    Price = 160000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[19], // Trip with index 19
//                    Price = 100000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[1], // Assuming types[1] is a valid TicketType
//                    Trip = trips[20], // Trip with index 20
//                    Price = 140000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[2], // Assuming types[2] is a valid TicketType
//                    Trip = trips[21], // Trip with index 21
//                    Price = 170000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                },
//                new TicketType_Trip
//                {
//                    TicketType_TripID = Guid.NewGuid(),
//                    TicketType = types[0], // Assuming types[0] is a valid TicketType
//                    Trip = trips[22], // Trip with index 22
//                    Price = 92000,
//                    Status = "ACTIVE",
//                    Quantity = 20
//                }
//            };
//            List<Booking> bookings = new()
//            {
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = DateTime.Now,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[0],
//                    User = users[0],
//                    QRCode = "11111",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = DateTime.Now,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[1],
//                    User = users[1],
//                    QRCode = "22222",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = DateTime.Now,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[2],
//                    User = users[3],
//                    QRCode = "33333",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"

//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = DateTime.Now,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 100000,
//                    Trip = trips[3],
//                    User = users[0],
//                    QRCode = "44444",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = DateTime.Now,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[4],
//                    User = users[3],
//                    QRCode = "55555",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 5, 1),
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[5],
//                    User = users[1],
//                    QRCode = "12345",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 3, 15),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "False",
//                    TotalBill = 120000,
//                    Trip = trips[6],
//                    User = users[2],
//                    QRCode = "67890",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 6, 5),
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 360000,
//                    Trip = trips[7],
//                    User = users[0],
//                    QRCode = "abcde",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 20),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[8],
//                    User = users[3],
//                    QRCode = "fghij",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 5, 10),
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[9],
//                    User = users[1],
//                    QRCode = "qwert",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 3, 1),
//                    Quantity = 1,
//                    Status = "INACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[10],
//                    User = users[2],
//                    QRCode = "yuiop",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 6, 15),
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 360000,
//                    Trip = trips[11],
//                    User = users[0],
//                    QRCode = "asdfg",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 30),
//                    Quantity = 1,
//                    Status = "INACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[12],
//                    User = users[3],
//                    QRCode = "hjkl",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 5, 20),
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[13],
//                    User = users[1],
//                    QRCode = "zxcvb",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 3, 10),
//                    Quantity = 1,
//                    Status = "INACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[14],
//                    User = users[3],
//                    QRCode = "mnbvc",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 6, 5),
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 360000,
//                    Trip = trips[15],
//                    User = users[2],
//                    QRCode = "lkjhg",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 25),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "BALANCE",
//                    PaymentStatus = "True",
//                    TotalBill = 120000,
//                    Trip = trips[16],
//                    User = users[0],
//                    QRCode = "poiuy",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[16],
//                    User = users[1],
//                    QRCode = "poauy",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[17],
//                    User = users[1],
//                    QRCode = "podsf",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[18],
//                    User = users[1],
//                    QRCode = "qweuy",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[19],
//                    User = users[1],
//                    QRCode = "poaqw",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[20],
//                    User = users[1],
//                    QRCode = "poaut",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[21],
//                    User = users[1],
//                    QRCode = "qoauy",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[22],
//                    User = users[1],
//                    QRCode = "poasy",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                },
//                new Booking
//                {
//                    BookingID = Guid.NewGuid(),
//                    BookingTime = new DateTime(2024, 4, 24),
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    PaymentMethod = "CASH",
//                    PaymentStatus = "True",
//                    TotalBill = 240000,
//                    Trip = trips[23],
//                    User = users[1],
//                    QRCode = "poaay",
//                    FullName = "Tuan",
//                    PhoneNumber = "1234567898",
//                    Email ="tuan@gmail.com"
//                }
//            };
//            List<TicketDetail> ticketDetails = new List<TicketDetail>();

//            for (int i = 0; i < bookings.Count; i++)
//            {
//                double price = 0;
//                var ticketTypeTrip = new TicketType_Trip();
//                for (int a = 0; a < ticketType_Trips.Count; a++)
//                {
//                    if (ticketType_Trips[a].Trip.TripID == bookings[i].Trip.TripID
//                        && ticketType_Trips[a].Trip.Route_Company == bookings[i].Trip.Route_Company
//                        && ticketType_Trips[a].Trip.StartTime == bookings[i].Trip.StartTime
//                        && ticketType_Trips[a].Trip.EndTime == bookings[i].Trip.EndTime)
//                    {
//                        price = ticketType_Trips[a].Price;
//                        ticketTypeTrip = ticketType_Trips[a];
//                        break;
//                    }
//                }
//                for (int j = 0; j < bookings[i].Quantity; j++)
//                {
//                    Random random = new Random();
//                    int randomIndex = random.Next(0, 3);
//                    string status = null;
//                    switch (randomIndex)
//                    {
//                        case 0:
//                            status = "UNUSED";
//                            break;

//                        case 1:
//                            status = "CANCEL";
//                            break;

//                        case 2:
//                            status = "USED";
//                            break;

//                        default:
//                            status = "UNUSED";
//                            break;
//                    }
//                    ticketDetails.Add(new TicketDetail
//                    {
//                        TicketDetailID = Guid.NewGuid(),
//                        Booking = bookings[i],
//                        Price = price,
//                        Status = status,
//                        SeatCode = $"A{j + 1}",
//                        TicketType_Trip = ticketTypeTrip,
//                    });
//                }
//            }

//            List<TicketDetail_Service> ticketDetail_Services = new()
//            {
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[1],
//                    Service = services[1],
//                    Price = 120000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[2],
//                    Service = services[2],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[35],
//                    Service = services[0],
//                    Price = 120000,
//                    Quantity = 5,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[0],
//                    Service = services[0],
//                    Price = 120000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[0],
//                    Service = services[3],
//                    Price = 80000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[5],
//                    Service = services[1],
//                    Price = 90000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[10],
//                    Service = services[7],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[15],
//                    Service = services[9],
//                    Price = 200000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[20],
//                    Service = services[4],
//                    Price = 120000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[25],
//                    Service = services[11],
//                    Price = 180000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[7]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[30],
//                    Service = services[6],
//                    Price = 100000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[0],
//                    Service = services[2],
//                    Price = 75000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[10],
//                    Service = services[8],
//                    Price = 160000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[3],
//                    Service = services[5],
//                    Price = 130000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[8],
//                    Service = services[10],
//                    Price = 170000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[13],
//                    Service = services[2],
//                    Price = 85000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[18],
//                    Service = services[8],
//                    Price = 160000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[23],
//                    Service = services[4],
//                    Price = 120000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[28],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[0],
//                    Service = services[6],
//                    Price = 110000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[5],
//                    Service = services[9],
//                    Price = 180000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[10],
//                    Service = services[3],
//                    Price = 95000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[15],
//                    Service = services[7],
//                    Price = 150000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[3]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[1],
//                    Service = services[0],
//                    Price = 120000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[2],
//                    Service = services[4],
//                    Price = 110000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[3],
//                    Service = services[8],
//                    Price = 160000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[4],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[5],
//                    Service = services[6],
//                    Price = 130000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[6],
//                    Service = services[2],
//                    Price = 90000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[7],
//                    Service = services[7],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[8],
//                    Service = services[3],
//                    Price = 100000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[9],
//                    Service = services[9],
//                    Price = 170000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[10],
//                    Service = services[5],
//                    Price = 120000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[11],
//                    Service = services[1],
//                    Price = 125000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[12],
//                    Service = services[5],
//                    Price = 115000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[13],
//                    Service = services[9],
//                    Price = 165000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[14],
//                    Service = services[0],
//                    Price = 195000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[15],
//                    Service = services[7],
//                    Price = 135000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[16],
//                    Service = services[3],
//                    Price = 95000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[17],
//                    Service = services[8],
//                    Price = 155000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[18],
//                    Service = services[4],
//                    Price = 105000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[19],
//                    Service = services[10],
//                    Price = 175000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[20],
//                    Service = services[6],
//                    Price = 125000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[21],
//                    Service = services[2],
//                    Price = 130000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[22],
//                    Service = services[6],
//                    Price = 120000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[23],
//                    Service = services[10],
//                    Price = 180000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[24],
//                    Service = services[1],
//                    Price = 200000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[25],
//                    Service = services[5],
//                    Price = 140000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[26],
//                    Service = services[9],
//                    Price = 100000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[27],
//                    Service = services[3],
//                    Price = 160000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[28],
//                    Service = services[7],
//                    Price = 110000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[29],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[7]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[30],
//                    Service = services[4],
//                    Price = 130000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[7]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[31],
//                    Service = services[8],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[32],
//                    Service = services[0],
//                    Price = 170000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[33],
//                    Service = services[6],
//                    Price = 120000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[3]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[34],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[35],
//                    Service = services[2],
//                    Price = 140000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[20]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[0],
//                    Service = services[7],
//                    Price = 160000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[1],
//                    Service = services[5],
//                    Price = 130000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[2],
//                    Service = services[9],
//                    Price = 180000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[3],
//                    Service = services[1],
//                    Price = 110000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[4],
//                    Service = services[6],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[5],
//                    Service = services[3],
//                    Price = 120000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[6],
//                    Service = services[8],
//                    Price = 160000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[7],
//                    Service = services[4],
//                    Price = 130000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[2]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[8],
//                    Service = services[10],
//                    Price = 180000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[9],
//                    Service = services[2],
//                    Price = 140000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[10],
//                    Service = services[7],
//                    Price = 150000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[4]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[11],
//                    Service = services[1],
//                    Price = 110000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[12],
//                    Service = services[6],
//                    Price = 170000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[13],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[14],
//                    Service = services[5],
//                    Price = 100000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[15],
//                    Service = services[9],
//                    Price = 160000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[16],
//                    Service = services[3],
//                    Price = 130000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[3]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[17],
//                    Service = services[8],
//                    Price = 180000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[18],
//                    Service = services[0],
//                    Price = 120000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[5]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[19],
//                    Service = services[6],
//                    Price = 170000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[0]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[20],
//                    Service = services[4],
//                    Price = 140000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[21],
//                    Service = services[11],
//                    Price = 190000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[22],
//                    Service = services[2],
//                    Price = 110000,
//                    Quantity = 1,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[23],
//                    Service = services[7],
//                    Price = 150000,
//                    Quantity = 2,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                },
//                new TicketDetail_Service
//                {
//                    TicketDetail_ServiceID = Guid.NewGuid(),
//                    TicketDetail = ticketDetails[24],
//                    Service = services[1],
//                    Price = 100000,
//                    Quantity = 3,
//                    Status = "ACTIVE",
//                    Station = stations[1]
//                }
//            };
//            List<Station_Service> station_Services = new()
//            {
//                new Station_Service
//                {
//                    Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[0],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 135000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[1],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 350000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[0],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 25000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[2],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 45000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[3],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 310000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[1],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 250000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[1],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 390000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[2],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 400000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[2],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 350000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[2],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 354000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[3],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 353000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[3],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 351000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[4],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 261000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[4],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 156000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[4],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 789000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[5],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 125000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[5],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 563000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[5],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 123000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg" },
//                    new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[5],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 345000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                    new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[78],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 652000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[23],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 155000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[101],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 312000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[42],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 235000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[89],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 991000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[67],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 901000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[120],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 99000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[32],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 324000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[54],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 345000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[93],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[110],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[29],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[84],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[62],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[97],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[21],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[73],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[116],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[38],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[105],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[53],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[92],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[27],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[115],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[71],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[41],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[126],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[18],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[87],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[64],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[109],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[34],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[79],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[58],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[103],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[25],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[121],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[47],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[90],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[68],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[6],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[19],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[33],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[46],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[59],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[72],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[85],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[98],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[111],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[124],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[10],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[23],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[36],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[49],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[62],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[75],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[88],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[101],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[114],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[127],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[13],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[26],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[39],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[52],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[65],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[78],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[91],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[104],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[117],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[129],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[16],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[29],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[42],
//                    Service = services[6],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[55],
//                    Service = services[0],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[68],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[81],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[94],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[107],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[120],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },

//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[129],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[10],
//                    Service = services[2],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[23],
//                    Service = services[8],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[36],
//                    Service = services[4],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[49],
//                    Service = services[10],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[62],
//                    Service = services[1],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[75],
//                    Service = services[7],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[88],
//                    Service = services[3],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[101],
//                    Service = services[9],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[114],
//                    Service = services[5],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                },
//                new Station_Service
//                {
//                   Station_ServiceID = Guid.NewGuid(),
//                    Station = stations[127],
//                    Service = services[11],
//                    Status = "ACTIVE",
//                    Price = 35000,
//                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
//                }
//            };
//            /*  List<Service_Trip> trip_Services = new()
//              {
//                  new Service_Trip
//                  {
//                      Trip = trips[0],
//                      Service = services[0],
//                      Status = "ACTIVE"
//                  },
//                  new Service_Trip
//                  {
//                      Trip = trips[0],
//                      Service = services[1],
//                      Status = "ACTIVE"
//                  },
//                  new Service_Trip
//                  {
//                      Trip = trips[2],
//                      Service = services[1],
//                      Status = "ACTIVE"
//                  },
//                  new Service_Trip
//                  {
//                      Trip = trips[2],
//                      Service = services[2],
//                      Status = "ACTIVE"
//                  },
//                  new Service_Trip
//                  {
//                      Trip = trips[3],
//                      Service = services[1],
//                      Status = "ACTIVE"
//                  }
//              };*/
//            List<Feedback> feedbacks = new()
//            {
//                new Feedback
//                {
//                    FeedbackID = Guid.NewGuid(),
//                    User = users[0],
//                    Trip = trips[0],
//                    TripTemplate = trips[0],
//                    Rating = 5,
//                    Description = "Dịch vụ tốt",
//                    Status = "ACTIVE"
//                }, new Feedback
//                {
//                    FeedbackID = Guid.NewGuid(),
//                    User = users[0],
//                    Trip = trips[1],
//                    TripTemplate = trips1[1],
//                    Rating = 4,
//                    Description = "Dịch vụ khá tốt",
//                    Status = "ACTIVE"
//                }, new Feedback
//                {
//                    FeedbackID = Guid.NewGuid(),
//                    User = users[0],
//                    Trip = trips[2],
//                    TripTemplate = trips[2],
//                    Rating = 4,
//                    Description = "Dịch vụ rất tốt",
//                    Status = "ACTIVE"
//                }, new Feedback
//                {
//                    FeedbackID = Guid.NewGuid(),
//                    User = users[0],
//                    Trip = trips[3],
//                    TripTemplate = trips[3],
//                    Rating = 3,
//                    Description = "Dịch vụ tạm",
//                    Status = "ACTIVE"
//                }, new Feedback
//                {
//                    FeedbackID = Guid.NewGuid(),
//                    User = users[0],
//                    Trip = trips[4],
//                    TripTemplate = trips[4],
//                    Rating = 5,
//                    Description = "Dịch vụ tốt",
//                    Status = "ACTIVE"
//                },
//            };
//            List<Feedback_Image> images = new()
//            {
//                new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[0],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                },
//                 new Feedback_Image
//                {
//                     Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[0],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }, new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[1],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }, new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[1],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }, new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[2],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }, new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[3],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }, new Feedback_Image
//                {
//                    Feedback_Image_ID = Guid.NewGuid(),
//                    Feedback = feedbacks[4],
//                    ImageUrl = "https://letsenhance.io/static/8f5e523ee6b2479e26ecc91b9c25261e/1015f/MainAfter.jpg",
//                    Status = "ACTIVE"
//                }
//            };

//            List<TripPicture> tripPictures = new()
//            {
//                new TripPicture
//                {
//                    TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://thuexekhach.com/wp-content/uploads/2019/10/xe-giu%CC%9Bo%CC%9B%CC%80ng-na%CC%86%CC%80m-34-giu%CC%9Bo%CC%9B%CC%80ng.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://nhieuxe.vn/upload/images/xe-giuong-nam-co-bao-nhieu-cho--so-ghe-xe-giuong-nam-danh-so-nhu-the-nao2.png",
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://viptrip.vn/public/upload/service/cho-thue-xe-giuong-nam-di-hai-phong4_800x600_714394588.webp",
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://storage.googleapis.com/blogvxr-uploads/2023/03/3.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[0]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://static.vexere.com/production/images/1662198208436.jpeg",
//                    Status = "ACTIVE",
//                    Trip = trips[1]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://static.vexere.com/production/images/1662976460604.jpeg",
//                    Status = "ACTIVE",
//                    Trip = trips[1]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://static.vexere.com/production/images/1662976466001.jpeg",
//                    Status = "ACTIVE",
//                    Trip = trips[1]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://static.vexere.com/production/images/1702817477891.jpeg",
//                    Status = "ACTIVE",
//                    Trip = trips[1]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://cdn.kosshop.vn/wp-content/uploads/2022/11/kinh-nghiem-chon-vi-tri-ghe-xe-giuong-nam.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[2]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSwGHIHBkYG0EkW4B0NNETWKPuMqnvFrDt2YA&s",
//                    Status = "ACTIVE",
//                    Trip = trips[2]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://mia.vn/media/uploads/blog-du-lich/danh-sach-nha-xe-giuong-nam-tuyen-da-nang-quang-binh-ban-can-bo-tui-03-1653402305.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://xekhachhonghai.com/wp-content/uploads/2018/12/dcar-cung-dien-di-dong-21-phong-1.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://i-vnexpress.vnecdn.net/2018/05/08/dcar-2-3889-1525748221.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[3]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSi4Ci_N8RbkPp52dXagxazPW1ylCx1SCINbQ&s",
//                    Status = "ACTIVE",
//                    Trip = trips[4]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://cdn.alongwalk.info/wp-content/uploads/2022/06/17055236/image-top-6-xe-giuong-nam-binh-duong-di-da-lat-xin-so-nhat-165539475673144.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[4]
//                },
//                new TripPicture
//                {
//                     TripPictureID = Guid.NewGuid(),
//                    ImageUrl ="https://bookinglimo.vn/wp-content/uploads/2022/06/Xe-gi%C6%B0%E1%BB%9Dng-%C4%91%C3%B4i-B%C3%ACnh-D%C6%B0%C6%A1ng-%C4%91i-%C4%90%C3%A0-L%E1%BA%A1t-Booking-Limo.jpg",
//                    Status = "ACTIVE",
//                    Trip = trips[4]
//                }
//            };

//            // Add Range training program
//            await _context.Users.AddRangeAsync(users);
//            await _context.UserRoles.AddRangeAsync(userRoles);
//            await _context.City.AddRangeAsync(cities);
//            await _context.Company.AddRangeAsync(companies);
//            await _context.Station.AddRangeAsync(stations);
//            await _context.Route.AddRangeAsync(routes);
//            await _context.Station_Route.AddRangeAsync(station_Routes);
//            await _context.Trip.AddRangeAsync(trips);
//            await _context.Trip.AddRangeAsync(trips1);
//            await _context.ServiceType.AddRangeAsync(serviceTypes);
//            await _context.Service.AddRangeAsync(services);
//            await _context.TicketType.AddRangeAsync(types);
//            await _context.TicketType_Trip.AddRangeAsync(ticketType_Trips);
//            await _context.Booking.AddRangeAsync(bookings);
//            await _context.TicketDetail.AddRangeAsync(ticketDetails);
//            await _context.TicketDetail_Service.AddRangeAsync(ticketDetail_Services);
//            await _context.Station_Service.AddRangeAsync(station_Services);
//            //await _context.Service_Trip.AddRangeAsync(trip_Services);
//            await _context.Route_Company.AddRangeAsync(route_Companies);
//            await _context.Utility.AddRangeAsync(utilities);
//            await _context.UtilityInTrips.AddRangeAsync(utilityInTrips);
//            await _context.Feedback.AddRangeAsync(feedbacks);
//            await _context.TripPictures.AddRangeAsync(tripPictures);
//            // Save to DB
//            await _context.SaveChangesAsync();
//        }
//    }

//    public static class DatabaseInitialiserExtension
//    {
//        public static async Task InitialiseDatabaseAsync(this WebApplication app)
//        {
//            // Create IServiceScope to resolve service scope
//            using var scope = app.Services.CreateScope();
//            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();

//            await initializer.InitialiseAsync();

//            // Try to seeding data
//            await initializer.SeedAsync();
//        }
//    }
//}