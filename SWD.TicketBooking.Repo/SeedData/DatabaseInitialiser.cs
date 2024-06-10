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
                },
                new Station
                {
                    Name = "Trạm dừng chân ở Bảo Lộc",
                    Status = "Active",
                    City = cities[34],
                    Company = companies[2]
                },
                new Station
                {
                    Name = "Khu du lịch Đồng Tâm",
                    Status = "Active",
                    City = cities[5],
                    Company = companies[2]
                }
            };

            List<Route> routes = new()
            {
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[55],
                    StartLocation = "Bưu điện trung tâm TP. Hồ Chí Minh",
                    EndLocation = " Bến xe Huế",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[3],
                    ToCity = cities[57],
                    StartLocation = "Ngã ba Cái Mép",
                    EndLocation = "Quan 10",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[1],
                    ToCity = cities[57],
                    StartLocation = "Bãi Trước",
                    EndLocation = "Sân bay Tân Sơn Nhất",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[12],
                    StartLocation = " Bến xe miền tây",
                    EndLocation = " Bến xe Cần Thơ",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[30],
                    StartLocation = "Công viên 23/9, Quận 1",
                    EndLocation = "Bến xe Nha Trang",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[15],
                    StartLocation = "Bến Xe Miền Đông",
                    EndLocation = "Bến Xe Đà Nẵng",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[6],
                    ToCity = cities[57],
                    StartLocation = " Bến Tre Bus Station",
                    EndLocation = "Bến Xe Miền Tây",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[34],
                    StartLocation = "Bến Xe Miền Đông",
                    EndLocation = "Bến Xe Đà Lạt",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[57],
                    ToCity = cities[1],
                    StartLocation = "Bến Xe Miền Đông",
                    EndLocation = "Bến Xe Vũng Tàu",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[0], // An Giang
                    ToCity = cities[11], // Cà Mau
                    StartLocation = "Bến Xe An Giang",
                    EndLocation = "Bến Xe Cà Mau",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[24], // Hà Nội 
                    ToCity = cities[14], // Đà Nẵng
                    StartLocation = "Bến Xe Giáp Bát",
                    EndLocation = "Bến Xe Đà Nẵng",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[54], // TP. Hồ Chí Minh
                    ToCity = cities[36], // Kiên Giang
                    StartLocation = "Bến Xe Miền Tây",
                    EndLocation = "Bến Xe Rạch Giá",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[41], // Lào Cai
                    ToCity = cities[22], // Hà Giang
                    StartLocation = "Bến Xe Lào Cai",
                    EndLocation = "Bến Xe Hà Giang",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[2], // Bạc Liêu
                    ToCity = cities[13], // Cần Thơ
                    StartLocation = "Bến Xe Bạc Liêu",
                    EndLocation = "Bến Xe Cần Thơ",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[6], // Bến Tre
                    ToCity = cities[53], // Tiền Giang
                    StartLocation = "Bến Xe Bến Tre",
                    EndLocation = "Bến Xe Mỹ Tho",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[29], // Hải Phòng
                    ToCity = cities[15], // Đắk Lắk 
                    StartLocation = "Bến Xe Hải Phòng",
                    EndLocation = "Bến Xe Buôn Ma Thuột",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[35], // Kon Tum
                    ToCity = cities[21], // Gia Lai
                    StartLocation = "Bến Xe Kon Tum",
                    EndLocation = "Bến Xe Gia Lai",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[46], // Nghệ An
                    ToCity = cities[25], // Hà Tĩnh
                    StartLocation = "Bến Xe Vinh",
                    EndLocation = "Bến Xe Hà Tĩnh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[10], // Cao Bằng
                    ToCity = cities[41], // Lào Cai
                    StartLocation = "Bến Xe Cao Bằng",
                    EndLocation = "Bến Xe Lào Cai",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[32], // Hòa Bình
                    ToCity = cities[38], // Lai Châu
                    StartLocation = "Bến Xe Hòa Bình",
                    EndLocation = "Bến Xe Lai Châu",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[19], // Đắk Nông
                    ToCity = cities[52], // Tây Ninh
                    StartLocation = "Bến Xe Gia Nghĩa",
                    EndLocation = "Bến Xe Tây Ninh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[9], // Cần Thơ
                    ToCity = cities[36], // Kiên Giang
                    StartLocation = "Bến Xe Cần Thơ",
                    EndLocation = "Bến Xe Rạch Giá",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[28], // Hải Dương
                    ToCity = cities[29], // Hải Phòng
                    StartLocation = "Bến Xe Hải Dương",
                    EndLocation = "Bến Xe Hải Phòng",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[43], // Ninh Bình
                    ToCity = cities[48], // Phú Thọ
                    StartLocation = "Bến Xe Ninh Bình",
                    EndLocation = "Bến Xe Việt Trì",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[33], // Hưng Yên
                    ToCity = cities[26], // Hải Dương
                    StartLocation = "Bến Xe Hưng Yên",
                    EndLocation = "Bến Xe Hải Dương",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[18], // Đắk Lắk
                    ToCity = cities[35], // Kon Tum
                    StartLocation = "Bến Xe Buôn Ma Thuột",
                    EndLocation = "Bến Xe Kon Tum",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[20], // Điện Biên
                    ToCity = cities[45], // Nghệ An
                    StartLocation = "Bến Xe Điện Biên Phủ",
                    EndLocation = "Bến Xe Vinh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[23], // Hà Nam
                    ToCity = cities[43], // Ninh Bình
                    StartLocation = "Bến Xe Phủ Lý",
                    EndLocation = "Bến Xe Ninh Bình",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[12], // Đắk Nông
                    ToCity = cities[31], // Hòa Bình
                    StartLocation = "Bến Xe Gia Nghĩa",
                    EndLocation = "Bến Xe Hòa Bình",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[39], // Lâm Đồng
                    ToCity = cities[54], // TP. Hồ Chí Minh
                    StartLocation = "Bến Xe Đà Lạt",
                    EndLocation = "Bến Xe Miền Tây",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[47], // Phú Yên
                    ToCity = cities[51], // Quảng Bình
                    StartLocation = "Bến Xe Tuy Hòa",
                    EndLocation = "Bến Xe Đồng Hới",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[50], // Quảng Nam
                    ToCity = cities[14], // Đà Nẵng
                    StartLocation = "Bến Xe Tam Kỳ",
                    EndLocation = "Bến Xe Đà Nẵng",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[55], // Vĩnh Long
                    ToCity = cities[6], // Bến Tre
                    StartLocation = "Bến Xe Vĩnh Long",
                    EndLocation = "Bến Xe Bến Tre",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[11], // Cà Mau
                    ToCity = cities[37], // Kiên Giang
                    StartLocation = "Bến Xe Cà Mau",
                    EndLocation = "Bến Xe Rạch Giá",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[24], // Hà Tĩnh
                    ToCity = cities[27], // Hậu Giang
                    StartLocation = "Bến Xe Hà Tĩnh",
                    EndLocation = "Bến Xe Vị Thanh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[42], // Nam Định
                    ToCity = cities[23], // Hà Nam
                    StartLocation = "Bến Xe Nam Định",
                    EndLocation = "Bến Xe Phủ Lý",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[49], // Quảng Ngãi
                    ToCity = cities[50], // Quảng Nam
                    StartLocation = "Bến Xe Quảng Ngãi",
                    EndLocation = "Bến Xe Tam Kỳ",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[22], // Gia Lai
                    ToCity = cities[12], // Đắk Nông
                    StartLocation = "Bến Xe Gia Lai",
                    EndLocation = "Bến Xe Gia Nghĩa",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[40], // Lạng Sơn
                    ToCity = cities[43], // Ninh Bình
                    StartLocation = "Bến Xe Lạng Sơn",
                    EndLocation = "Bến Xe Ninh Bình",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[13], // Đồng Nai
                    ToCity = cities[39], // Lâm Đồng
                    StartLocation = "Bến Xe Biên Hòa",
                    EndLocation = "Bến Xe Đà Lạt",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[21], // Đồng Tháp
                    ToCity = cities[8], // Cà Mau
                    StartLocation = "Bến Xe Cao Lãnh",
                    EndLocation = "Bến Xe Cà Mau",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[46], // Phú Thọ
                    ToCity = cities[20], // Điện Biên
                    StartLocation = "Bến Xe Việt Trì",
                    EndLocation = "Bến Xe Điện Biên Phủ",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[44], // Ninh Thuận
                    ToCity = cities[47], // Phú Yên
                    StartLocation = "Bến Xe Phan Rang",
                    EndLocation = "Bến Xe Tuy Hòa",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[28], // Hậu Giang
                    ToCity = cities[55], // Vĩnh Long
                    StartLocation = "Bến Xe Vị Thanh",
                    EndLocation = "Bến Xe Vĩnh Long",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[38], // Lào Cai
                    ToCity = cities[40], // Lạng Sơn
                    StartLocation = "Bến Xe Lào Cai",
                    EndLocation = "Bến Xe Lạng Sơn",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[7], // Bình Định
                    ToCity = cities[49], // Quảng Ngãi
                    StartLocation = "Bến Xe Quy Nhơn",
                    EndLocation = "Bến Xe Quảng Ngãi",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[36], // Lai Châu
                    ToCity = cities[5], // Bắc Giang
                    StartLocation = "Bến Xe Lai Châu",
                    EndLocation = "Bến Xe Bắc Giang",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[30], // Hà Giang
                    ToCity = cities[19], // Đắk Nông
                    StartLocation = "Bến Xe Hà Giang",
                    EndLocation = "Bến Xe Gia Nghĩa",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[15], // Đắk Lắk
                    ToCity = cities[22], // Gia Lai
                    StartLocation = "Bến Xe Buôn Ma Thuột",
                    EndLocation = "Bến Xe Gia Lai",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[26], // Hải Phòng
                    ToCity = cities[41], // Lạng Sơn
                    StartLocation = "Bến Xe Hải Phòng",
                    EndLocation = "Bến Xe Lạng Sơn",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[10], // Bình Thuận
                    ToCity = cities[44], // Ninh Thuận
                    StartLocation = "Bến Xe Phan Thiết",
                    EndLocation = "Bến Xe Phan Rang",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[3], // Bắc Kạn
                    ToCity = cities[35], // Kon Tum
                    StartLocation = "Bến Xe Bắc Kạn",
                    EndLocation = "Bến Xe Kon Tum",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[48], // Quảng Ninh
                    ToCity = cities[29], // Hải Dương
                    StartLocation = "Bến Xe Hạ Long",
                    EndLocation = "Bến Xe Hải Dương",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[1], // Bà Rịa - Vũng Tàu
                    ToCity = cities[6], // Bắc Ninh
                    StartLocation = "Bến Xe Vũng Tàu",
                    EndLocation = "Bến Xe Bắc Ninh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[25], // Hà Nội
                    ToCity = cities[31], // Hà Nam
                    StartLocation = "Bến Xe Mỹ Đình",
                    EndLocation = "Bến Xe Phủ Lý",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[2], // Bắc Giang
                    ToCity = cities[32], // Hà Tĩnh
                    StartLocation = "Bến Xe Bắc Giang",
                    EndLocation = "Bến Xe Hà Tĩnh",
                    Status = "Active"
                },
                new Route
                {
                    FromCity = cities[45], // Phú Thọ
                    ToCity = cities[4], // Bắc Ninh
                    StartLocation = "Bến Xe Việt Trì",
                    EndLocation = "Bến Xe Bắc Ninh",
                    Status = "Inactive"
                },
                new Route
                {
                    FromCity = cities[18], // Cao Bằng
                    ToCity = cities[51], // Quảng Trị
                    StartLocation = "Bến Xe Cao Bằng",
                    EndLocation = "Bến Xe Đông Hà",
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
                    Route = routes[0],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute = 2
                },
                new Station_Route
                {
                    Route = routes[0],
                    Station = stations[4],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute = 1
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[0],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[5],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[1],
                    Station = stations[4],
                    Status = "Active",
                    OrderInRoute =5
                },
                new Station_Route
                {
                    Route = routes[2],
                    Station = stations[0],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[2],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[2],
                    Station = stations[5],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[2],
                    Station = stations[3],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[3],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[3],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[3],
                    Station = stations[5],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[3],
                    Station = stations[7],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[4],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[4],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[4],
                    Station = stations[3],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[4],
                    Station = stations[7],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[5],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[5],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[5],
                    Station = stations[5],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[0],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[4],
                    Status = "Active",
                    OrderInRoute =5
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[7],
                    Status = "Active",
                    OrderInRoute =6
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[3],
                    Status = "Active",
                    OrderInRoute =7
                },
                new Station_Route
                {
                    Route = routes[6],
                    Station = stations[5],
                    Status = "Active",
                    OrderInRoute =8
                },
                new Station_Route
                {
                    Route = routes[7],
                    Station = stations[0],
                    Status = "Active",
                    OrderInRoute =3
                },
                new Station_Route
                {
                    Route = routes[7],
                    Station = stations[1],
                    Status = "Active",
                    OrderInRoute =2
                },
                new Station_Route
                {
                    Route = routes[7],
                    Station = stations[2],
                    Status = "Active",
                    OrderInRoute =4
                },
                new Station_Route
                {
                    Route = routes[7],
                    Station = stations[4],
                    Status = "Active",
                    OrderInRoute =1
                },
                new Station_Route
                {
                    Route = routes[7],
                    Station = stations[7],
                    Status = "Active",
                    OrderInRoute =5
                }
            };
            List<Trip> trips = new()
            {
                new Trip
                {
                    Route=routes[0],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 19, 15, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[0],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 6, 18, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[0],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 6, 18, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 7, 17, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
                    EndTime = new DateTime(2024, 7, 1, 23, 59, 59),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[1],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 7, 17, 45, 0),
                    Status ="Active",
                },
                new Trip
                {
                    Route=routes[2],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 15, 0, 0, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[2],
                    StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
                    EndTime = new DateTime(2024, 7, 2, 23, 59, 59),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[2],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 8, 17, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[2],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 8, 17, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 13, 14, 20, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 17, 17, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 13, 14, 20, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 9, 17, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 9, 17, 45, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[3],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 17, 17, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[4],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 15, 18, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[4],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 15, 18, 30, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[4],
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 23, 9, 0, 0),
                    Status ="Active"
                },
                new Trip
                {
                    Route=routes[4],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 14, 14, 20, 0),
                    Status ="Active"
                },
                 new Trip
                {
                    Route=routes[5],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
                    Status ="Active"
                },
                  new Trip
                {
                    Route=routes[5],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 25, 11, 30, 0),
                    Status ="Active"
                },
                   new Trip
                {
                    Route=routes[5],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 15, 14, 20, 0),
                    Status ="Active"
                },
                    new Trip
                {
                    Route=routes[5],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 15, 14, 20, 0),
                    Status ="Active"
                },
                     new Trip
                {
                    Route=routes[5],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 10, 17, 45, 0),
                    Status ="Active"
                },
                      new Trip
                {
                    Route=routes[6],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 21, 11, 30, 0),
                    Status ="Active"
                },
                 new Trip
                {
                    Route=routes[6],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 14, 14, 20, 0),
                    Status ="Active"
                },
                  new Trip
                {
                    Route=routes[6],
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 14, 14, 20, 0),
                    Status ="Active"
                },
                   new Trip
                {
                    Route=routes[6],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 21, 11, 30, 0),
                    Status ="Active"
                },
                    new Trip
                {
                    Route=routes[6],
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 23, 9, 0, 0),
                    Status ="Active"
                },
                     new Trip
                {
                    Route=routes[7],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 18, 11, 30, 0),
                    Status ="Active"

                },
                      new Trip
                {
                    Route=routes[7],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 18, 11, 30, 0),
                    Status ="Active"
                },
                       new Trip
                {
                    Route=routes[7],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 18, 11, 30, 0),
                    Status ="Active"
                },
                        new Trip
                {
                    Route=routes[7],
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 22, 9, 0, 0),
                    Status ="Active"
                },
                         new Trip
                {
                    Route=routes[7],
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 11, 17, 45, 0),
                    Status ="Active"
                },
                          new Trip
                {
                    Route=routes[8],
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
                    Status ="Active"
                },
                 new Trip
                {
                    Route=routes[8],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 16, 11, 30, 0),
                    Status ="Active"
                },
                  new Trip
                {
                    Route=routes[8],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 16, 11, 30, 0),
                    Status ="Active"
                },
                   new Trip
                {
                    Route=routes[8],
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
                    Status ="Active"
                },
                    new Trip
                {
                    Route=routes[8],
                    StartTime = new DateTime(2024, 6, 15, 11, 30, 0),
                    EndTime = new DateTime(2024, 6, 16, 11, 30, 0),
                    Status ="Active"
                },
                    new Trip
                {
                    Route = routes[25], // Assuming routes[25] is a valid route within the 0-50 index range
                    StartTime = new DateTime(2024, 7, 5, 17, 45, 0),
                    EndTime = new DateTime(2024, 7, 6, 17, 45, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route within the 0-50 index range
                    StartTime = new DateTime(2024, 8, 20, 9, 0, 0),
                    EndTime = new DateTime(2024, 8, 21, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route within the 0-50 index range
                    StartTime = new DateTime(2024, 6, 30, 23, 59, 59),
                    EndTime = new DateTime(2024, 7, 1, 23, 59, 59),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[32], // Assuming routes[32] is a valid route within the 0-50 index range
                    StartTime = new DateTime(2024, 7, 12, 14, 20, 0),
                    EndTime = new DateTime(2024, 7, 13, 14, 20, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[7], // Assuming routes[7] is a valid route
                    StartTime = new DateTime(2024, 6, 20, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 20, 15, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[19], // Assuming routes[19] is a valid route
                    StartTime = new DateTime(2024, 7, 10, 17, 30, 0),
                    EndTime = new DateTime(2024, 7, 11, 7, 30, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[34], // Assuming routes[34] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 11, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[42], // Assuming routes[42] is a valid route
                    StartTime = new DateTime(2024, 6, 25, 14, 0, 0),
                    EndTime = new DateTime(2024, 6, 26, 2, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[28], // Assuming routes[28] is a valid route
                    StartTime = new DateTime(2024, 7, 20, 6, 30, 0),
                    EndTime = new DateTime(2024, 7, 20, 16, 30, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[13], // Assuming routes[13] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 18, 0, 0),
                    EndTime = new DateTime(2024, 6, 23, 6, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[31], // Assuming routes[31] is a valid route
                    StartTime = new DateTime(2024, 7, 15, 10, 30, 0),
                    EndTime = new DateTime(2024, 7, 16, 0, 30, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[46], // Assuming routes[46] is a valid route
                    StartTime = new DateTime(2024, 8, 10, 13, 0, 0),
                    EndTime = new DateTime(2024, 8, 11, 1, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[9], // Assuming routes[9] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 21, 0, 0),
                    EndTime = new DateTime(2024, 6, 29, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[23], // Assuming routes[23] is a valid route
                    StartTime = new DateTime(2024, 7, 25, 7, 0, 0),
                    EndTime = new DateTime(2024, 7, 25, 19, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 6, 18, 12, 0, 0),
                    EndTime = new DateTime(2024, 6, 18, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[5], // Assuming routes[5] is a valid route
                    StartTime = new DateTime(2024, 7, 3, 22, 30, 0),
                    EndTime = new DateTime(2024, 7, 4, 6, 30, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[27], // Assuming routes[27] is a valid route
                    StartTime = new DateTime(2024, 8, 5, 15, 0, 0),
                    EndTime = new DateTime(2024, 8, 6, 3, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[16], // Assuming routes[16] is a valid route
                    StartTime = new DateTime(2024, 6, 30, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 30, 16, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[37], // Assuming routes[37] is a valid route
                    StartTime = new DateTime(2024, 7, 18, 19, 0, 0),
                    EndTime = new DateTime(2024, 7, 19, 3, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 6, 15, 5, 0, 0),
                    EndTime = new DateTime(2024, 6, 15, 15, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[20], // Assuming routes[20] is a valid route
                    StartTime = new DateTime(2024, 7, 12, 20, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 4, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 10, 0, 0),
                    EndTime = new DateTime(2024, 8, 15, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 23, 23, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 14, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 6, 15, 5, 0, 0),
                    EndTime = new DateTime(2024, 6, 15, 15, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[20], // Assuming routes[20] is a valid route
                    StartTime = new DateTime(2024, 7, 12, 20, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 4, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 10, 0, 0),
                    EndTime = new DateTime(2024, 8, 15, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 23, 23, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 14, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[11], // Assuming routes[11] is a valid route
                    StartTime = new DateTime(2024, 6, 20, 0, 0, 0),
                    EndTime = new DateTime(2024, 6, 20, 8, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[25], // Assuming routes[25] is a valid route
                    StartTime = new DateTime(2024, 7, 6, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 6, 20, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[34], // Assuming routes[34] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 6, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 14, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[7], // Assuming routes[7] is a valid route
                    StartTime = new DateTime(2024, 6, 26, 18, 0, 0),
                    EndTime = new DateTime(2024, 6, 27, 2, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[19], // Assuming routes[19] is a valid route
                    StartTime = new DateTime(2024, 7, 22, 9, 0, 0),
                    EndTime = new DateTime(2024, 7, 22, 17, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[40], // Assuming routes[40] is a valid route
                    StartTime = new DateTime(2024, 6, 12, 20, 0, 0),
                    EndTime = new DateTime(2024, 6, 13, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[3], // Assuming routes[3] is a valid route
                    StartTime = new DateTime(2024, 7, 3, 11, 0, 0),
                    EndTime = new DateTime(2024, 7, 3, 17, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[32], // Assuming routes[32] is a valid route
                    StartTime = new DateTime(2024, 8, 10, 23, 0, 0),
                    EndTime = new DateTime(2024, 8, 11, 7, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[13], // Assuming routes[13] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 14, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 22, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[23], // Assuming routes[23] is a valid route
                    StartTime = new DateTime(2024, 7, 15, 7, 0, 0),
                    EndTime = new DateTime(2024, 7, 15, 15, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[40], // Assuming routes[40] is a valid route
                    StartTime = new DateTime(2024, 6, 12, 20, 0, 0),
                    EndTime = new DateTime(2024, 6, 13, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[3], // Assuming routes[3] is a valid route
                    StartTime = new DateTime(2024, 7, 3, 11, 0, 0),
                    EndTime = new DateTime(2024, 7, 3, 17, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[32], // Assuming routes[32] is a valid route
                    StartTime = new DateTime(2024, 8, 10, 23, 0, 0),
                    EndTime = new DateTime(2024, 8, 11, 7, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[13], // Assuming routes[13] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 14, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 22, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[23], // Assuming routes[23] is a valid route
                    StartTime = new DateTime(2024, 7, 15, 7, 0, 0),
                    EndTime = new DateTime(2024, 7, 15, 15, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[5], // Assuming routes[5] is a valid route
                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[27], // Assuming routes[27] is a valid route
                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[36], // Assuming routes[36] is a valid route
                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[9], // Assuming routes[9] is a valid route
                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[21], // Assuming routes[21] is a valid route
                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[5], // Assuming routes[5] is a valid route
                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[27], // Assuming routes[27] is a valid route
                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[36], // Assuming routes[36] is a valid route
                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[9], // Assuming routes[9] is a valid route
                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[21], // Assuming routes[21] is a valid route
                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[5], // Assuming routes[5] is a valid route
                    StartTime = new DateTime(2024, 6, 18, 4, 0, 0),
                    EndTime = new DateTime(2024, 6, 18, 12, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[27], // Assuming routes[27] is a valid route
                    StartTime = new DateTime(2024, 7, 9, 18, 0, 0),
                    EndTime = new DateTime(2024, 7, 10, 2, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[36], // Assuming routes[36] is a valid route
                    StartTime = new DateTime(2024, 8, 5, 13, 0, 0),
                    EndTime = new DateTime(2024, 8, 5, 21, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[9], // Assuming routes[9] is a valid route
                    StartTime = new DateTime(2024, 6, 30, 22, 0, 0),
                    EndTime = new DateTime(2024, 7, 1, 6, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[21], // Assuming routes[21] is a valid route
                    StartTime = new DateTime(2024, 7, 19, 15, 0, 0),
                    EndTime = new DateTime(2024, 7, 19, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[14], // Assuming routes[14] is a valid route
                    StartTime = new DateTime(2024, 6, 24, 10, 0, 0),
                    EndTime = new DateTime(2024, 6, 24, 18, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[41], // Assuming routes[41] is a valid route
                    StartTime = new DateTime(2024, 7, 13, 1, 0, 0),
                    EndTime = new DateTime(2024, 7, 13, 9, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[2], // Assuming routes[2] is a valid route
                    StartTime = new DateTime(2024, 8, 15, 20, 0, 0),
                    EndTime = new DateTime(2024, 8, 16, 4, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[33], // Assuming routes[33] is a valid route
                    StartTime = new DateTime(2024, 6, 22, 6, 0, 0),
                    EndTime = new DateTime(2024, 6, 22, 14, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[17], // Assuming routes[17] is a valid route
                    StartTime = new DateTime(2024, 7, 27, 12, 0, 0),
                    EndTime = new DateTime(2024, 7, 27, 20, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[26], // Assuming routes[26] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[11], // Assuming routes[11] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[26], // Assuming routes[26] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[11], // Assuming routes[11] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[26], // Assuming routes[26] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[11], // Assuming routes[11] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[26], // Assuming routes[26] is a valid route
                    StartTime = new DateTime(2024, 6, 28, 8, 0, 0),
                    EndTime = new DateTime(2024, 6, 28, 16, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[39], // Assuming routes[39] is a valid route
                    StartTime = new DateTime(2024, 7, 5, 23, 0, 0),
                    EndTime = new DateTime(2024, 7, 6, 7, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[11], // Assuming routes[11] is a valid route
                    StartTime = new DateTime(2024, 8, 1, 14, 0, 0),
                    EndTime = new DateTime(2024, 8, 1, 22, 0, 0),
                    Status = "Active"
                },
                new Trip
                {
                    Route = routes[8], // Assuming routes[8] is a valid route
                    StartTime = new DateTime(2024, 6, 21, 2, 0, 0),
                    EndTime = new DateTime(2024, 6, 21, 10, 0, 0),
                    Status = "Inactive"
                },
                new Trip
                {
                    Route = routes[29], // Assuming routes[29] is a valid route
                    StartTime = new DateTime(2024, 7, 23, 17, 0, 0),
                    EndTime = new DateTime(2024, 7, 23, 23, 0, 0),
                    Status = "Active"
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
                 
                    ServiceType=serviceTypes[0],
                    Name = "Mi tom",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
            
                    ServiceType=serviceTypes[0],
                    Name = "Pho",
                    Price = 45000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"

                },
                new Service
                {
                
                    ServiceType=serviceTypes[1],
                    Name = "Bun bo",
                    Price = 45000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
             
                    ServiceType=serviceTypes[2],
                    Name = "Com xeo",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
                
                    ServiceType=serviceTypes[1],
                    Name = "Com phan",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
             
                    ServiceType=serviceTypes[0],
                    Name = "Mi tom",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
               
                    ServiceType=serviceTypes[3],
                    Name = "Hamburger",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
                
                    ServiceType=serviceTypes[3],
                    Name = "Sandwich",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
                   
                    ServiceType=serviceTypes[3],
                    Name = "hotdog",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"
                },
                new Service
                {
                   
                    ServiceType=serviceTypes[2],
                    Name = "Trái cây",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"

                },
                new Service
                {
                 
                    ServiceType=serviceTypes[1],
                    Name = "Cà phê",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"

                },
                new Service
                {
               
                    ServiceType=serviceTypes[1],
                    Name = "Nước trái cây",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"

                },
                new Service
                {
                 
                    ServiceType=serviceTypes[1],
                    Name = "Trà",
                    Price = 35000,
                    ImageUrl = "https://cdn.tgdd.vn/Files/2019/10/23/1211482/cac-loai-mi-tom-duoc-ua-chuong-nhat-viet-nam-202206041405267719.jpeg"

                },
            };
            List<TicketType> types = new()
            {
                new TicketType
                {
                    Name = "Hàng đầu",
                    Status = "Active"
                },
                new TicketType
                {
                    Name = "Hàng giữa",
                    Status = "Active"
                },
                new TicketType
                {
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
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[10], // Assuming trips[10] is a valid Trip
                    Price = 85000,
                    Status = "Active",
                    Quantity = 15
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[27], // Assuming trips[27] is a valid Trip
                    Price = 140000,
                    Status = "Inactive",
                    Quantity = 22
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[89], // Assuming trips[89] is a valid Trip
                    Price = 175000,
                    Status = "Active",
                    Quantity = 12
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[2], // Assuming trips[2] is a valid Trip
                    Price = 75000,
                    Status = "Active",
                    Quantity = 20
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[17], // Assuming trips[17] is a valid Trip
                    Price = 120000,
                    Status = "Inactive",
                    Quantity = 28
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[42], // Assuming trips[42] is a valid Trip
                    Price = 85000,
                    Status = "Active",
                    Quantity = 10
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[68], // Assuming trips[68] is a valid Trip
                    Price = 190000,
                    Status = "Inactive",
                    Quantity = 18
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[99], // Assuming trips[99] is a valid Trip
                    Price = 160000,
                    Status = "Active",
                    Quantity = 25
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[51], // Assuming trips[51] is a valid Trip
                    Price = 155000,
                    Status = "Inactive",
                    Quantity = 14
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[80], // Assuming trips[80] is a valid Trip
                    Price = 180000,
                    Status = "Active",
                    Quantity = 9
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[7], // Assuming trips[7] is a valid Trip
                    Price = 90000,
                    Status = "Inactive",
                    Quantity = 26
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[92], // Assuming trips[92] is a valid Trip
                    Price = 125000,
                    Status = "Active",
                    Quantity = 19
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[15], // Assuming trips[15] is a valid Trip
                    Price = 75000,
                    Status = "Active",
                    Quantity = 12
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[38], // Assuming trips[38] is a valid Trip
                    Price = 120000,
                    Status = "Inactive",
                    Quantity = 21
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[62], // Assuming trips[62] is a valid Trip
                    Price = 85000,
                    Status = "Active",
                    Quantity = 16
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[84], // Assuming trips[84] is a valid Trip
                    Price = 190000,
                    Status = "Inactive",
                    Quantity = 5
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[23], // Assuming trips[23] is a valid Trip
                    Price = 160000,
                    Status = "Active",
                    Quantity = 30
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[65], // Assuming trips[65] is a valid Trip
                    Price = 70000,
                    Status = "Active",
                    Quantity = 8
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[42], // Assuming trips[42] is a valid Trip
                    Price = 130000,
                    Status = "Inactive",
                    Quantity = 24
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[91], // Assuming trips[91] is a valid Trip
                    Price = 180000,
                    Status = "Active",
                    Quantity = 18
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[12], // Assuming trips[12] is a valid Trip
                    Price = 95000,
                    Status = "Inactive",
                    Quantity = 11
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[77], // Assuming trips[77] is a valid Trip
                    Price = 150000,
                    Status = "Active",
                    Quantity = 27
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[30], // Assuming trips[30] is a valid Trip
                    Price = 80000,
                    Status = "Active",
                    Quantity = 22
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[58], // Assuming trips[58] is a valid Trip
                    Price = 115000,
                    Status = "Inactive",
                    Quantity = 16
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[83], // Assuming trips[83] is a valid Trip
                    Price = 90000,
                    Status = "Active",
                    Quantity = 14
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[45], // Assuming trips[45] is a valid Trip
                    Price = 170000,
                    Status = "Inactive",
                    Quantity = 9
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[72], // Assuming trips[72] is a valid Trip
                    Price = 140000,
                    Status = "Active",
                    Quantity = 20
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[11], // Assuming trips[11] is a valid Trip
                    Price = 80000,
                    Status = "Active",
                    Quantity = 9
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[34], // Assuming trips[34] is a valid Trip
                    Price = 125000,
                    Status = "Inactive",
                    Quantity = 14
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[69], // Assuming trips[69] is a valid Trip
                    Price = 150000,
                    Status = "Active",
                    Quantity = 17
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[92], // Assuming trips[92] is a valid Trip
                    Price = 105000,
                    Status = "Inactive",
                    Quantity = 11
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[55], // Assuming trips[55] is a valid Trip
                    Price = 90000,
                    Status = "Active",
                    Quantity = 15
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[18], // Assuming trips[18] is a valid Trip
                    Price = 82000,
                    Status = "Active",
                    Quantity = 10
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[41], // Assuming trips[41] is a valid Trip
                    Price = 128000,
                    Status = "Inactive",
                    Quantity = 16
                },
                new TicketType_Trip
                {
                    TicketType = types[2], // Assuming types[2] is a valid TicketType
                    Trip = trips[74], // Assuming trips[74] is a valid Trip
                    Price = 155000,
                    Status = "Active",
                    Quantity = 19
                },
                new TicketType_Trip
                {
                    TicketType = types[0], // Assuming types[0] is a valid TicketType
                    Trip = trips[62], // Assuming trips[62] is a valid Trip
                    Price = 95000,
                    Status = "Inactive",
                    Quantity = 8
                },
                new TicketType_Trip
                {
                    TicketType = types[1], // Assuming types[1] is a valid TicketType
                    Trip = trips[29], // Assuming trips[29] is a valid Trip
                    Price = 112000,
                    Status = "Active",
                    Quantity = 13
                }
            };
            List<Booking> bookings = new()
            {
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[0],
                    User = users[0],
                    QRCode = "11111",
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 2,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[1],
                    User = users[1],
                    QRCode = "22222"

                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 2,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[3],
                    User = users[3],
                    QRCode = "33333"
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 100000,
                    Trip = trips[0],
                    User = users[0],
                    QRCode = "44444"
                },
                new Booking
                {
                    BookingTime = DateTime.Now,
                    Quantity = 1,
                    Status = "ACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[3],
                    User = users[3],
                    QRCode = "55555"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 5, 1),
                    Quantity = 2,
                    Status = "ACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[1],
                    User = users[1],
                    QRCode = "12345"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 3, 15),
                    Quantity = 1,
                    Status = "ACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "False",
                    TotalBill = 120000,
                    Trip = trips[2],
                    User = users[2],
                    QRCode = "67890"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 6, 5),
                    Quantity = 3,
                    Status = "ACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 360000,
                    Trip = trips[0],
                    User = users[0],
                    QRCode = "abcde"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 4, 20),
                    Quantity = 1,
                    Status = "ACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[3],
                    User = users[3],
                    QRCode = "fghij"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 5, 10),
                    Quantity = 2,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[1],
                    User = users[1],
                    QRCode = "qwert"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 3, 1),
                    Quantity = 1,
                    Status = "INACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[2],
                    User = users[2],
                    QRCode = "yuiop"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 6, 15),
                    Quantity = 3,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 360000,
                    Trip = trips[0],
                    User = users[0],
                    QRCode = "asdfg"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 4, 30),
                    Quantity = 1,
                    Status = "INACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[3],
                    User = users[3],
                    QRCode = "hjkl"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 5, 20),
                    Quantity = 2,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 240000,
                    Trip = trips[2],
                    User = users[1],
                    QRCode = "zxcvb"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 3, 10),
                    Quantity = 1,
                    Status = "INACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[0],
                    User = users[3],
                    QRCode = "mnbvc"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 6, 5),
                    Quantity = 3,
                    Status = "ACTIVE",
                    PaymentMethod = "CASH",
                    PaymentStatus = "True",
                    TotalBill = 360000,
                    Trip = trips[1],
                    User = users[2],
                    QRCode = "lkjhg"
                },
                new Booking
                {
                    BookingTime = new DateTime(2024, 4, 25),
                    Quantity = 1,
                    Status = "INACTIVE",
                    PaymentMethod = "BALANCE",
                    PaymentStatus = "True",
                    TotalBill = 120000,
                    Trip = trips[3],
                    User = users[0],
                    QRCode = "poiuy"
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
          /*  List<Service_Trip> trip_Services = new()
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
            };*/
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

            List<TripPicture> tripPictures = new()
            {
                new TripPicture
                {
                    ImageUrl ="https://thuexekhach.com/wp-content/uploads/2019/10/xe-giu%CC%9Bo%CC%9B%CC%80ng-na%CC%86%CC%80m-34-giu%CC%9Bo%CC%9B%CC%80ng.jpg",
                    Status = "Active",
                    Trip = trips[0]
                },
                new TripPicture
                {
                    ImageUrl ="https://nhieuxe.vn/upload/images/xe-giuong-nam-co-bao-nhieu-cho--so-ghe-xe-giuong-nam-danh-so-nhu-the-nao2.png",
                    Status = "Active",
                    Trip = trips[0]
                },
                new TripPicture
                {
                    ImageUrl ="https://viptrip.vn/public/upload/service/cho-thue-xe-giuong-nam-di-hai-phong4_800x600_714394588.webp",
                    Status = "Active",
                    Trip = trips[0]
                },
                new TripPicture
                {
                    ImageUrl ="https://storage.googleapis.com/blogvxr-uploads/2023/03/3.jpg",
                    Status = "Active",
                    Trip = trips[0]
                },
                new TripPicture
                {
                    ImageUrl ="https://static.vexere.com/production/images/1662198208436.jpeg",
                    Status = "Active",
                    Trip = trips[1]
                },
                new TripPicture
                {
                    ImageUrl ="https://static.vexere.com/production/images/1662976460604.jpeg",
                    Status = "Active",
                    Trip = trips[1]
                },
                new TripPicture
                {
                    ImageUrl ="https://static.vexere.com/production/images/1662976466001.jpeg",
                    Status = "Active",
                    Trip = trips[1]
                },
                new TripPicture
                {
                    ImageUrl ="https://static.vexere.com/production/images/1702817477891.jpeg",
                    Status = "Active",
                    Trip = trips[1]
                },
                new TripPicture
                {
                    ImageUrl ="https://cdn.kosshop.vn/wp-content/uploads/2022/11/kinh-nghiem-chon-vi-tri-ghe-xe-giuong-nam.jpg",
                    Status = "Active",
                    Trip = trips[2]
                },
                new TripPicture
                {
                    ImageUrl ="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSwGHIHBkYG0EkW4B0NNETWKPuMqnvFrDt2YA&s",
                    Status = "Active",
                    Trip = trips[2]
                },
                new TripPicture
                {
                    ImageUrl ="https://mia.vn/media/uploads/blog-du-lich/danh-sach-nha-xe-giuong-nam-tuyen-da-nang-quang-binh-ban-can-bo-tui-03-1653402305.jpg",
                    Status = "Active",
                    Trip = trips[3]
                },
                new TripPicture
                {
                    ImageUrl ="https://xekhachhonghai.com/wp-content/uploads/2018/12/dcar-cung-dien-di-dong-21-phong-1.jpg",
                    Status = "Active",
                    Trip = trips[3]
                },
                new TripPicture
                {
                    ImageUrl ="https://i-vnexpress.vnecdn.net/2018/05/08/dcar-2-3889-1525748221.jpg",
                    Status = "Active",
                    Trip = trips[3]
                },
                new TripPicture
                {
                    ImageUrl ="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSi4Ci_N8RbkPp52dXagxazPW1ylCx1SCINbQ&s",
                    Status = "Active",
                    Trip = trips[4]
                },
                new TripPicture
                {
                    ImageUrl ="https://cdn.alongwalk.info/wp-content/uploads/2022/06/17055236/image-top-6-xe-giuong-nam-binh-duong-di-da-lat-xin-so-nhat-165539475673144.jpg",
                    Status = "Active",
                    Trip = trips[4]
                },
                new TripPicture
                {
                    ImageUrl ="https://bookinglimo.vn/wp-content/uploads/2022/06/Xe-gi%C6%B0%E1%BB%9Dng-%C4%91%C3%B4i-B%C3%ACnh-D%C6%B0%C6%A1ng-%C4%91i-%C4%90%C3%A0-L%E1%BA%A1t-Booking-Limo.jpg",
                    Status = "Active",
                    Trip = trips[4]
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
            //await _context.Service_Trip.AddRangeAsync(trip_Services);
            await _context.Route_Company.AddRangeAsync(route_Companies);
            await _context.Utility.AddRangeAsync(utilities);
            await _context.utilityInTrips.AddRangeAsync(utilityInTrips);
            await _context.tripPictures.AddRangeAsync(tripPictures);
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
