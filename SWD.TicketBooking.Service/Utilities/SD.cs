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
           /* public static string PM_VNPAY = "TIỀN MẶT";
            public static string PM_BALANCE = "SỐ DƯ";*/
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

    }
}