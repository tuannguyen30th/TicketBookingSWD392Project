namespace SWD.TicketBooking.Service.Utilities
{
    public class SD
    {
        private static SD instance;
        private SD()
        {
        }
        public static SD getInstance()
        {
            if (instance == null) instance = new SD();
            return instance;
        }
        public class Booking_TicketStatus
        {
            public static string UNUSED_TICKET = "CHƯA SỬ DỤNG";
            public static string CANCEL_TICKET = "ĐÃ HỦY";
            public static string USED_TICKET = "ĐÃ SỬ DỤNG";
            public static string NOTPAYING_TICKET = "CHƯA THANH TOÁN";
        }
        public class BookingStatus
        {
            public static string PAYING_BOOKING = "ĐÃ THANH TOÁN";
            public static string NOTPAYING_BOOKING = "CHƯA THANH TOÁN";
            public static string CANCEL_BOOKING = "ĐÃ HỦY";
        }
        public class Booking_ServiceStatus
        {
            public static string NOTPAYING_TICKETSERVICE = "CHƯA THANH TOÁN";
            public static string PAYING_TICKETSERVICE = "ĐÃ THANH TOÁN";
            public static string CANCEL_TICKETSERVICE = "ĐÃ HỦY";
        }
        public class GeneralStatus
        {
            public static string ACTIVE = "HOẠT ĐỘNG";
            public static string INACTIVE = "KHÔNG HOẠT ĐỘNG";
        }
        public class FilterOption
        {
            public const string SEAT_A = "A";
            public const string SEAT_B = "B";
            public const string SEAT_C = "C";
            public const string SEAT_HEAD = "HÀNG ĐẦU";
            public const string SEAT_MIDDLE = "HÀNG GIỮA";
            public const string SEAT_BACK = "HÀNG SAU";
            public const string PRICE_ASC = "GIÁ TĂNG DẦN";
            public const string PRICE_DESC = "GIÁ GIẢM DẦN";
            public const string RATING_ASC = "TỔNG SỐ ĐÁNH GIÁ TĂNG DẦN";
            public const string RATING_DESC = "TỔNG SỐ ĐÁNH GIÁ GIẢM DẦN";
            public const string TIME_SOONER = "THỜI GIAN ĐI SỚM NHẤT";
            public const string TIME_LATER = "THỜI GIAN ĐI MUỘN NHẤT";
        }
        public class Notification
        {
            public static string NotFound(string entity)
            {
                return $"KHÔNG TÌM THẤY {entity}!".ToUpper();
            }
            public static string Existed(string entity, string fieldName)
            {
                return $"ĐÃ TỒN TẠI {fieldName} TRONG {entity}!".ToUpper();
            }
            public static string NotFoundByField( string entity, string fieldName)
            {
                return $"KHÔNG TÌM THẤY {fieldName} TRONG {entity}!".ToUpper();
            }
            public static string Internal(string entity, string issue)
            {
                return $"VẤN ĐỀ XẢY RA VỚI {entity} - {issue}!".ToUpper();
            }
        }
        
        public static class Entity
        {
            public enum EntityType
            {
                City,
                Company,
                Feedback,
                Feedback_Image,
                Route,
                Route_Company,
                Service,
                ServiceType,
                Station, 
                Station_Route,
                Station_Service,
                Trip,
                Trip_Utility,
                User,
                UserRole,
                Utitily
            }
        }

    }
}