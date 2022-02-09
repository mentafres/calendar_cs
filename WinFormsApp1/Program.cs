using System.Globalization;
using System.Text.Json;

/*
 * �������:
 * 
 * ���������� "���������" ��� ������� ����������:
 * ������� ����, ���� ������, �����, �������� (���� ����), ����������� (���� ����).
 *
 * �������� ����� ��������, ����������� �� ������������ ����.
 * ����������� �� ������ ����� �������� � ���� (��� ��������), �� � �������� �� ������������ �����.
 * 
 * � ���������� ���������� ��������� ����������:
 * - ������, ��������������� � ������� ����������� ��� ��������.
 * - ����������� ������ ���� ����������� � ����������.
 * 
 * ���������: �������������� �.�.
 */
namespace WinFormsApp1
{

    /*
     * ������� ����� ����������.
     */
    internal static class Program
    {
        /*
         * ����� �����.
         * � ������ ���������� ������������� ������ ���������� � ����.
         */
        [STAThread]
        static void Main()
        {
            var logic = new CoreLogic();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(logic));
        }
    }

    /*
     * ����� � ������� ����������, ����������� � ���� HolidayManager (���������� �����������) � NotificationManager (���������� �������������).
     */
    public class CoreLogic
    {
        
        /*
         * ���������� � ����������� ������, ����������� �� ���������� �����������.
         */
        private readonly HolidayManager holidayManager;

        /*
         * ���������� � ����������� ������, ����������� �� ���������� �������������.
         */
        private readonly NotificationManager notificationManager;

        /*
         * ����������� ������ � ������� ����������.
         */
        public CoreLogic()
        {
            holidayManager = new HolidayManager();
            notificationManager = new NotificationManager();
        }

        // ������������ ����
        public NotificationManager Notifications { get => notificationManager; }

        // ������������ ����
        public HolidayManager Holidays { get => holidayManager; }

        /*
         * ��������� ������� ���� � ���� ������ � ������������ ��������.
         */
        public static string GetDateString() => DateTime.Now.ToString(SerializationUtils.DateFormat);

        /*
         * ��������� �������� ��� ������ � ���� ������ �� ����� ������������.
         */
        public static string GetDayOfWeekString() => DateTimeFormatInfo.CurrentInfo.GetDayName(DateTime.Now.DayOfWeek);

        /*
         * ��������� �������� ������� � ���� ������ � ������������ ��������.
         */
        public static string GetTimeString() => DateTime.Now.ToString(SerializationUtils.TimeFormat);

    }

    /*
     * �����, �������� � ���� ������ ���������� � ���������� �� ���������� ���.
     */
    public class HolidayManager
    {
        /*
         * ���������� � ����������� ��������� ������ ��� ����������.
         */
        private readonly Storage storage;

        /*
         * ���������� �� ������� ����������.
         */
        private readonly List<Holiday> holidays;

        /*
         * ����������� ������.
         * ��������� � ���� ������������� ��������� � ��������� ������ ���������� �� �����.
         */
        public HolidayManager()
        {
            this.storage = new Storage(StorageKey.Holiday);
            this.holidays = this.LoadHolidays();
        }

        /*
         * ��������� ������ ����������.
         */
        public List<Holiday> GetHolidays() => this.holidays;
        
        /*
         * ��������� ������ ���������� �������� ���.
         */
        public List<Holiday> GetTodayHolidays() => this.holidays.FindAll(h => h.Date.Date == DateTime.Now.Date);

        /*
         * �������� ��������� �� �������� � ����.
         * ����� ��������, �������� ����������� � ���������.
         */
        public Holiday CreateHoliday(string name, DateTime date)
        {
            var holiday = new Holiday(name, date);
            this.holidays.Add(holiday);
            this.SaveHolidays();
            return holiday;
        }

        /*
         * �������������� ��������� �� ����������� ��������������.
         * ����� �������� �������� � ���� ���������.
         * 
         * ����� ��������������, ����������� �������� ����������� � ���������.
         */
        public Holiday? EditHoliday(string uuid, Holiday newHoliday)
        {
            foreach (var holiday in this.holidays)
            {
                if (holiday.Uuid == uuid)
                {
                    holiday.Name = newHoliday.Name;
                    holiday.Date = newHoliday.Date;
                    this.SaveHolidays();
                    return holiday;
                }
            }
            return null;
        }

        /*
         * �������� ��������� �� ����������� ��������������.
         * ����� ��������, ����������� ������ ���������� ����������� � ���������.
         */
        public void RemoveHoliday(string uuid)
        {
            this.holidays.RemoveAll(h => h.Uuid == uuid);
            this.SaveHolidays();
        }

        /*
         * ���������� ����� ������ ���������� � ���������.
         */
        private void SaveHolidays() => this.storage.Save(this.holidays);

        /*
         * �������� ������ ���������� �� ���������.
         */
        private List<Holiday> LoadHolidays() => this.storage
                .Load()
                .Select(data => Holiday.CreateFromDictionary(data))
                .ToList();

    }

    /*
     * �����, �������� � ���� ������ ����������� � ���������� �� ���������� ���.
     */
    public class NotificationManager
    {

        /*
         * ���������� � ����������� ��������� ������ ��� �����������.
         */
        private readonly Storage storage;

        /*
         * ���������� �� ������� �����������.
         */
        private readonly List<Notification> notifications;

        /*
         * ����������� ������.
         * ��������� � ���� ������������� ��������� � ��������� ������ ����������� �� �����.
         */
        public NotificationManager()
        {
            this.storage = new Storage(StorageKey.Notification);
            this.notifications = this.LoadNotifications();
        }

        /*
         * ��������� ������ �����������.
         */
        public List<Notification> GetNotifications()
        {
            return this.notifications;
        }

        /*
         * ��������� ������ ����������� �������� ��� ����� �������� �������.
         */
        public List<Notification> GetTodayNotifications()
        {
            return this.notifications
                .OrderBy(n => n.ShowAt)
                .ToList()
                .FindAll(n => n.ShowAt.Date == DateTime.Now.Date && n.ShowAt >= DateTime.Now)
                .ToList();
        }

        /*
         * �������� ����������� �� �������� � ���� � ���������.
         * ����� ��������, ����������� ����������� � ���������.
         */
        public Notification CreateNotification(string header, DateTime showAt, string description)
        {
            var notification = new Notification(header, showAt, description);
            this.notifications.Add(notification);
            this.SaveNotifications();
            return notification;
        }

        /*
         * �������������� ����������� �� ����������� ��������������.
         * ����� �������� ��������, ����� � ��������.
         * 
         * ����� ��������������, ����������� ����������� ����������� � ���������.
         */
        public Notification? EditNotification(string uuid, Notification newNotification)
        {
            foreach (var notification in this.notifications)
            {
                if (notification.Uuid == uuid)
                {
                    notification.Header = newNotification.Header;
                    notification.ShowAt = newNotification.ShowAt;
                    notification.Description = newNotification.Description;
                    this.SaveNotifications();
                    return notification;
                }
            }
            return null;
        }

        /*
         * �������� ����������� �� ����������� ��������������.
         * ����� ��������, ����������� ������ ����������� ����������� � ���������.
         */
        public void RemoveNotification(string uuid)
        {
            this.notifications.RemoveAll(h => h.Uuid == uuid);
            this.SaveNotifications();
        }

        /*
         * ���������� ����� ������ ����������� � ���������.
         */
        private void SaveNotifications() => this.storage.Save(this.notifications);

        /*
         * �������� ������ ����������� �� ���������.
         */
        private List<Notification> LoadNotifications() => this.storage
                .Load()
                .Select(data => Notification.CreateFromDictionary(data))
                .ToList();
    }

    /*
     * �����, ���������� �� �������������� ���������� � ��������� ����� ������ (JSON-������).
     */
    public class Storage
    {

        /*
         * ���������� � ����� �� ����� � ��������� ����� ������.
         */
        private readonly string filePath;

        /*
         * ����������� ������.
         * ��������� � ���� ���������� � ���� �� ����� � ��������� ��.
         */
        public Storage(StorageKey key)
        {
            this.filePath = IOUtils.GetFilePath(key);
        }

        /*
         * ���������� ������ ����������� �������, ����������� ��������� IStorable.
         * 
         * ����� �������� �� ���� ������ ������� � ����������� ������ � ��� � ������� ����� ������ ������ IStorable#ToStringDictionary.
         * ���������� ������ �������� ������������� � ������ ������ ��������� UTF-8 � ������� JsonSerializer.
         * � �����, ����������� � ���� ��������� ��.
         */
        public void Save<T>(List<T> list) where T : IStorable
        {
            var data = list.Select(el => el.ToStringDictionary());
            var bytes = JsonSerializer.SerializeToUtf8Bytes(data)!;
            File.WriteAllBytes(filePath, bytes);
        }

        /*
         * �������� ������ ��������, ������� ������ � ���� "�������� ���������� + ��������" ��� ������ ����������� ��������.
         * 
         * ����� ��������� ������� ����� ��������� �� � �������� ������� � ��������� ������ ���� ��� ���������� ������.
         * ���� ���� ��� ������, �� ����� ��������� �� ���� ������ ������, ����� ������������� ����� � ������ �������� � ������� JsonDeserializer.
         * ���������� ������ �������� ������������ ���������.
         */
        public List<Dictionary<string, string>> Load()
        {
            if (!File.Exists(filePath))
            {
                this.Save(new List<IStorable>(0));
                return new List<Dictionary<string, string>>(0);
            }

            var bytes = File.ReadAllBytes(filePath);
            var utf8Reader = new Utf8JsonReader(bytes);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(ref utf8Reader)!;
            return data ?? new List<Dictionary<string, string>>(0);
        }

    }

    /*
     * ������������ ���������, ������� ����� ����������� � ���������.
     * (��� ������ ��� ����� ������ ��������� ����).
     */
    public enum StorageKey
    {
        // ���������
        Holiday,

        // �����������
        Notification

    }

    /*
     * ������������� ��������� � ���� ������.
     * ����� ��������� ��������� IStorable.
     */
    public class Holiday : IStorable
    {

        /*
         * ���������� �������������.
         */
        private string uuid;

        /*
         * �������� ���������.
         */
        private string name;

        /*
         * ���� ���������.
         */
        private DateTime date;

        /*
         * ����������� ������.
         * 
         * ����������� �������� �������� ���������, ���� � ���������� �������������.
         * ��� ���������� ����������� �������������� - �� ����� ������������ ��������.
         */
        public Holiday(string name, DateTime dateTime, string? uuid = null)
        {
            this.uuid = uuid ?? Guid.NewGuid().ToString();
            this.name = name;
            this.date = dateTime;
        }

        /*
         * ������������ ���� date.
         */
        public DateTime Date { get => date; set => date = value; }

        /*
         * ������������ ���� name.
         */
        public string Name { get => name; set => name = value; }

        /*
         * ������������ ���� uuid.
         */
        public string Uuid { get => uuid; set => uuid = value; }

        /*
         * ���������� ����� �� ���������� IStorable.
         * ����� ����������� ���� � �������� ���������� ������ � ������� ��������� ��������.
         */
        public Dictionary<string, string> ToStringDictionary()
        {
            return new Dictionary<string, string>(3)
            {
                { "uuid", uuid },
                { "name", name },
                { "date", date.ToString(SerializationUtils.DateFormat) }
            };
        }

        /*
         * �������� ���������� ������ ��������� �� ������� ��������� �������� ����� ������.
         */
        public static Holiday CreateFromDictionary(Dictionary<string, string> dictionary)
        {
            var uuid = dictionary["uuid"];
            var name = dictionary["name"];
            var date = DateTime.ParseExact(dictionary["date"], SerializationUtils.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
            return new Holiday(name, date, uuid);
        }

        /*
         * �������������� ���������� ������ � �����.
         */
        override public string ToString()
        {
            return name + " (" + this.date.Date.ToString(SerializationUtils.DateFormat) + ")";
        }

    }

    /*
     * ������������� ����������� � ���� ������.
     * ����� ��������� ��������� IStorable.
     */
    public class Notification : IStorable
    {

        /*
         * ���������� �������������.
         */
        private string uuid;

        /*
         * ��������� �����������.
         */
        private string header;

        /*
         * �������� �����������.
         */
        private string description;

        /*
         * ����� ��������� �����������.
         */
        private DateTime showAt;

        /*
         * ����������� ������.
         * 
         * ����������� �������� �������� �����������, �����, �������� � ���������� �������������.
         * ��� ���������� ����������� �������������� - �� ����� ������������ ��������.
         */
        public Notification(string header, DateTime showAt, string description = "", string uuid = null)
        {
            this.uuid = uuid ?? Guid.NewGuid().ToString();
            this.header = header;
            this.description = description;
            this.showAt = showAt;
        }

        /*
         * ������������ ���� header.
         */
        public string Header { get => header; set => header = value; }

        /*
         * ������������ ���� description.
         */
        public string Description { get => description; set => description = value; }

        /*
         * ������������ ���� showAt.
         */
        public DateTime ShowAt { get => showAt; set => showAt = value; }

        /*
         * ������������ ���� uuid.
         */
        public string Uuid { get => uuid; set => uuid = value; }

        /*
         * ���������� ����� �� ���������� IStorable.
         * ����� ����������� ���� � �������� ���������� ������ � ������� ��������� ��������.
         */
        public Dictionary<string, string> ToStringDictionary()
        {
            return new Dictionary<string, string>(4)
            {
                { "uuid", uuid },
                { "header", header },
                { "description", description },
                { "showAt", showAt.ToString(SerializationUtils.DateTimeFormat) }
            };
        }

        /*
         * �������� ���������� ������ ����������� �� ������� ��������� �������� ����� ������.
         */
        public static Notification CreateFromDictionary(Dictionary<string, string> dictionary)
        {
            var uuid = dictionary["uuid"];
            var name = dictionary["header"];
            var description = dictionary["description"];
            var date = DateTime.ParseExact(dictionary["showAt"], SerializationUtils.DateTimeFormat, CultureInfo.InvariantCulture);
            return new Notification(name, date, description, uuid);
        }

        /*
         * �������������� ���������� ������ � �����.
         */
        override public string ToString()
        {
            return header + " (" + this.showAt.TimeOfDay.ToString() + ")";
        }

    }

    /*
     * ���������, ����������� ������ ��������, ������� ����� ����������� � ��������� Storage.
     */
    public interface IStorable
    {

        /*
         * ���������� ������ ������ ��������������� ������ � ���������� ������ � ������� ��������.
         */
        Dictionary<string, string> ToStringDictionary();

    }

    /*
     * ������� ��� ������������ ������.
     */
    public static class SerializationUtils
    {

        // ������ ���, ����������� � ����������.
        public const string DateFormat = "dd'-'MM'-'yyyy";

        // ������ �������, ����������� � ����������.
        public const string TimeFormat = "HH':'mm':'ss";

        // ������ ���� � �������, ����������� � ����������.
        public const string DateTimeFormat = DateFormat + "'T'" + TimeFormat;

    }

    /*
     * ������� ��� ������ � �������� ��������.
     */
    public static class IOUtils
    {

        // ������� �������� ����� ��������� ��.
        public const string FileName = "storage.json";

        /*
         * ��������� ������ �� ����� ��������� �� � �������� �������.
         * 
         * ����� ����������� �� ���� �� ���������� ������������ � �� � �������� �����, 
         * ���������� �� �������� �����, ����������� � ��������� (�������� 'key') � �������� �������� ����� (��������� 'FileName').
         * 
         * ������. ���:
         *  ��������  key      = "Holiday"
         *  ��������� FileName = "storage.json"
         * ���������:
         *  C:\Users\Admin\holiday_storage.json
         */
        public static string GetFilePath(StorageKey key)
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(directory, key.ToString().ToLower() + "_" + FileName);
        }

    }

}
