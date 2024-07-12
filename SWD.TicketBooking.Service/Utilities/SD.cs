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
        public class TransactionStatus
        {
            public static string TRANSACTION_PAYMENT = "CHI TRẢ";
            public static string TRANSACTION_CANCELLATION = "HỦY BỎ";
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
            public static string Status()
            {
                return $"CHỈ CÓ THỂ ĐỔI TÊN Ở TRẠNG THÁI HOẠT ĐỘNG".ToUpper();
            }
        }
        
        public static class Entity
        {
            public const string ENTITY_CITY = "CITY";
            public const string ENTITY_COMPANY = "COMPANY";
            public const string ENTITY_FEEDBACK = "FEEDBACK";
            public const string ENTITY_FEEDBACK_IMAGE = "FEEDBACK_IMAGE";
            public const string ENTITY_ROUTE = "ROUTE";
            public const string ENTITY_ROUTE_COMPANY = "ROUTE_COMPANY";
            public const string ENTITY_SERVICE = "SERVICE";
            public const string ENTITY_SERVICE_TYPE = "SERVICETYPE";
            public const string ENTITY_STATION = "STATION";
            public const string ENTITY_STATION_ROUTE = "STATION_ROUTE";
            public const string ENTITY_STATION_SERVICE = "STATION_SERVICE";
            public const string ENTITY_TRIP = "TRIP";
            public const string ENTITY_TRIP_PICTURE = "TRIP_PICTURE";
            public const string ENTITY_TRIP_UTILITY = "TRIP_UTILITY";
            public const string ENTITY_USER = "USER";
            public const string ENTITY_USER_ROLE = "USERROLE";
            public const string ENTITY_UTILITY = "UTITILY";
            public const string ENTITY_STATION_COMPANY_ROUTE = "STAION_COMPANY_ROUTE";

        }

        public static class RoleName
        {
            public const string ADMIN = "ADMIN";
            public const string STAFF = "STAFF";
            public const string MANAGER = "MANAGER";
            public const string CUSTOMER = "CUSTOMER";          
        }

    }
}