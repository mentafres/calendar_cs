using System.Globalization;
using System.Text.Json;

/*
 * ЗАДАНИЕ:
 * 
 * Приложение "Календарь" при запуске показывает:
 * текущую дату, день недели, время, праздник (если есть), напоминание (если есть).
 *
 * Праздник имеет название, назначается на определенную дату.
 * Напоминание не только имеет название и дату (как праздник), но и задается на определенное время.
 * 
 * В приложении реализован следующий функционал:
 * - задать, отредактировать и удалить напоминание или праздник.
 * - просмотреть список всех напоминаний и праздников.
 * 
 * ВЫПОЛНИЛА: Красносельская Д.М.
 */
namespace WinFormsApp1
{

    /*
     * Главный класс приложения.
     */
    internal static class Program
    {
        /*
         * Точка входа.
         * В методе происходит инициализация логики приложения и окна.
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
     * Класс с логикой приложения, включащющий в себя HolidayManager (управление праздниками) и NotificationManager (управление уведомлениями).
     */
    public class CoreLogic
    {
        
        /*
         * Переменная с экземпляром класса, отвечающего за управление праздниками.
         */
        private readonly HolidayManager holidayManager;

        /*
         * Переменная с экземпляром класса, отвечающего за управление уведомлениями.
         */
        private readonly NotificationManager notificationManager;

        /*
         * Конструктор класса с логикой приложения.
         */
        public CoreLogic()
        {
            holidayManager = new HolidayManager();
            notificationManager = new NotificationManager();
        }

        // Инкапсулятор поля
        public NotificationManager Notifications { get => notificationManager; }

        // Инкапсулятор поля
        public HolidayManager Holidays { get => holidayManager; }

        /*
         * Получение текущей даты в виде строки с определенным форматом.
         */
        public static string GetDateString() => DateTime.Now.ToString(SerializationUtils.DateFormat);

        /*
         * Получение текущего дня недели в виде строки на языке пользователя.
         */
        public static string GetDayOfWeekString() => DateTimeFormatInfo.CurrentInfo.GetDayName(DateTime.Now.DayOfWeek);

        /*
         * Получение текущего времени в виде строки с определенным форматом.
         */
        public static string GetTimeString() => DateTime.Now.ToString(SerializationUtils.TimeFormat);

    }

    /*
     * Класс, хранящий в себе данные праздников и отвечающий за управление ими.
     */
    public class HolidayManager
    {
        /*
         * Переменная с экземплятом хранилища данных для праздников.
         */
        private readonly Storage storage;

        /*
         * Переменная со списком праздников.
         */
        private readonly List<Holiday> holidays;

        /*
         * Конструктор класса.
         * Выполняет в себе инициализацию хранилища и загружает список праздников из файла.
         */
        public HolidayManager()
        {
            this.storage = new Storage(StorageKey.Holiday);
            this.holidays = this.LoadHolidays();
        }

        /*
         * Получение списка праздников.
         */
        public List<Holiday> GetHolidays() => this.holidays;
        
        /*
         * Получение списка праздников текущего дня.
         */
        public List<Holiday> GetTodayHolidays() => this.holidays.FindAll(h => h.Date.Date == DateTime.Now.Date);

        /*
         * Создание праздника по названию и дате.
         * После создания, праздник сохраняется в хранилище.
         */
        public Holiday CreateHoliday(string name, DateTime date)
        {
            var holiday = new Holiday(name, date);
            this.holidays.Add(holiday);
            this.SaveHolidays();
            return holiday;
        }

        /*
         * Редактирование праздника по уникальному идентификатору.
         * Можно изменять название и дату праздника.
         * 
         * После редактирования, обновленный праздник сохраняется в хранилище.
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
         * Удаление праздника по уникальному идентификатору.
         * После удаления, обновленный список праздников сохраняется в хранилище.
         */
        public void RemoveHoliday(string uuid)
        {
            this.holidays.RemoveAll(h => h.Uuid == uuid);
            this.SaveHolidays();
        }

        /*
         * Сохранение всего списка праздников в хранилище.
         */
        private void SaveHolidays() => this.storage.Save(this.holidays);

        /*
         * Загрузка списка праздников из хранилища.
         */
        private List<Holiday> LoadHolidays() => this.storage
                .Load()
                .Select(data => Holiday.CreateFromDictionary(data))
                .ToList();

    }

    /*
     * Класс, хранящий в себе данные уведомлений и отвечающий за управление ими.
     */
    public class NotificationManager
    {

        /*
         * Переменная с экземплятом хранилища данных для уведомлений.
         */
        private readonly Storage storage;

        /*
         * Переменная со списком уведомлений.
         */
        private readonly List<Notification> notifications;

        /*
         * Конструктор класса.
         * Выполняет в себе инициализацию хранилища и загружает список уведомлений из файла.
         */
        public NotificationManager()
        {
            this.storage = new Storage(StorageKey.Notification);
            this.notifications = this.LoadNotifications();
        }

        /*
         * Получение списка уведомлений.
         */
        public List<Notification> GetNotifications()
        {
            return this.notifications;
        }

        /*
         * Получение списка уведомлений текущего дня после текущего времени.
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
         * Создание уведомления по названию и дате с описанием.
         * После создания, уведомление сохраняется в хранилище.
         */
        public Notification CreateNotification(string header, DateTime showAt, string description)
        {
            var notification = new Notification(header, showAt, description);
            this.notifications.Add(notification);
            this.SaveNotifications();
            return notification;
        }

        /*
         * Редактирование уведомления по уникальному идентификатору.
         * Можно изменять название, время и описание.
         * 
         * После редактирования, обновленное уведомление сохраняется в хранилище.
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
         * Удаление уведомления по уникальному идентификатору.
         * После удаления, обновленный список уведомлений сохраняется в хранилище.
         */
        public void RemoveNotification(string uuid)
        {
            this.notifications.RemoveAll(h => h.Uuid == uuid);
            this.SaveNotifications();
        }

        /*
         * Сохранение всего списка уведомлений в хранилище.
         */
        private void SaveNotifications() => this.storage.Save(this.notifications);

        /*
         * Загрузка списка уведомлений из хранилища.
         */
        private List<Notification> LoadNotifications() => this.storage
                .Load()
                .Select(data => Notification.CreateFromDictionary(data))
                .ToList();
    }

    /*
     * Класс, отвечающий за взаимодействие приложения с локальной базой данных (JSON-файлом).
     */
    public class Storage
    {

        /*
         * Переменная с путем до файла с локальной базой данных.
         */
        private readonly string filePath;

        /*
         * Конструктор класса.
         * Заполняет в себе информацию о пути до файла с локальной БД.
         */
        public Storage(StorageKey key)
        {
            this.filePath = IOUtils.GetFilePath(key);
        }

        /*
         * Сохранение списка экземпляров классов, наследующих интерфейс IStorable.
         * 
         * Метод получает на вход список классов и преобразует классы в нем в словари путем вызова метода IStorable#ToStringDictionary.
         * Полученный список словарей сериализуется в массив байтов кодировки UTF-8 с помощью JsonSerializer.
         * А затем, сохраняется в файл локальной БД.
         */
        public void Save<T>(List<T> list) where T : IStorable
        {
            var data = list.Select(el => el.ToStringDictionary());
            var bytes = JsonSerializer.SerializeToUtf8Bytes(data)!;
            File.WriteAllBytes(filePath, bytes);
        }

        /*
         * Загрузка списка словарей, которые хранят в себе "название переменной + значение" для каждой сохраненной сущности.
         * 
         * Метод проверяет наличие файла локальной БД в файловой системе и сохраняет пустой файл при отсутствии онного.
         * Если файл был найден, то метод считывает из него массив байтов, затем десериализует байты в список словарей с помощью JsonDeserializer.
         * Полученный список является возвращаемым значением.
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
     * Перечисление сущностей, которые могут сохраняться в хранилище.
     * (под каждый тип будет создан отдельный файл).
     */
    public enum StorageKey
    {
        // Праздники
        Holiday,

        // Уведомления
        Notification

    }

    /*
     * Представление праздника в виде класса.
     * Класс наследует интерфейс IStorable.
     */
    public class Holiday : IStorable
    {

        /*
         * Уникальный идентификатор.
         */
        private string uuid;

        /*
         * Название праздника.
         */
        private string name;

        /*
         * Дата праздника.
         */
        private DateTime date;

        /*
         * Конструктор класса.
         * 
         * Аргументами являются название праздника, дата и уникальный идентификатор.
         * При отсутствии уникального идентификатора - он будет сгенерировал случайно.
         */
        public Holiday(string name, DateTime dateTime, string? uuid = null)
        {
            this.uuid = uuid ?? Guid.NewGuid().ToString();
            this.name = name;
            this.date = dateTime;
        }

        /*
         * Инкапсулятор поля date.
         */
        public DateTime Date { get => date; set => date = value; }

        /*
         * Инкапсулятор поля name.
         */
        public string Name { get => name; set => name = value; }

        /*
         * Инкапсулятор поля uuid.
         */
        public string Uuid { get => uuid; set => uuid = value; }

        /*
         * Реализация метод из интерфейса IStorable.
         * Метод преобразует поля и значения экземпляра класса в словарь текстовых значений.
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
         * Создание экземпляра класса праздника из словаря текстовых значений полей класса.
         */
        public static Holiday CreateFromDictionary(Dictionary<string, string> dictionary)
        {
            var uuid = dictionary["uuid"];
            var name = dictionary["name"];
            var date = DateTime.ParseExact(dictionary["date"], SerializationUtils.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
            return new Holiday(name, date, uuid);
        }

        /*
         * Преобразование экземпляра класса в текст.
         */
        override public string ToString()
        {
            return name + " (" + this.date.Date.ToString(SerializationUtils.DateFormat) + ")";
        }

    }

    /*
     * Представление уведомления в виде класса.
     * Класс наследует интерфейс IStorable.
     */
    public class Notification : IStorable
    {

        /*
         * Уникальный идентификатор.
         */
        private string uuid;

        /*
         * Заголовок уведомления.
         */
        private string header;

        /*
         * Описание уведомления.
         */
        private string description;

        /*
         * Время появления уведомления.
         */
        private DateTime showAt;

        /*
         * Конструктор класса.
         * 
         * Аргументами являются название уведмоления, время, описание и уникальный идентификатор.
         * При отсутствии уникального идентификатора - он будет сгенерировал случайно.
         */
        public Notification(string header, DateTime showAt, string description = "", string uuid = null)
        {
            this.uuid = uuid ?? Guid.NewGuid().ToString();
            this.header = header;
            this.description = description;
            this.showAt = showAt;
        }

        /*
         * Инкапсулятор поля header.
         */
        public string Header { get => header; set => header = value; }

        /*
         * Инкапсулятор поля description.
         */
        public string Description { get => description; set => description = value; }

        /*
         * Инкапсулятор поля showAt.
         */
        public DateTime ShowAt { get => showAt; set => showAt = value; }

        /*
         * Инкапсулятор поля uuid.
         */
        public string Uuid { get => uuid; set => uuid = value; }

        /*
         * Реализация метод из интерфейса IStorable.
         * Метод преобразует поля и значения экземпляра класса в словарь текстовых значений.
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
         * Создание экземпляра класса уведомления из словаря текстовых значений полей класса.
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
         * Преобразование экземпляра класса в текст.
         */
        override public string ToString()
        {
            return header + " (" + this.showAt.TimeOfDay.ToString() + ")";
        }

    }

    /*
     * Интерфейс, описывающий методы сущности, которая может сохраняться в хранилище Storage.
     */
    public interface IStorable
    {

        /*
         * Реализация метода должна преобразовывать данные в экземпляре класса в словарь значений.
         */
        Dictionary<string, string> ToStringDictionary();

    }

    /*
     * Утилиты для сериализации данных.
     */
    public static class SerializationUtils
    {

        // Формат дат, приминяемый в приложении.
        public const string DateFormat = "dd'-'MM'-'yyyy";

        // Формат времени, приминяемый в приложении.
        public const string TimeFormat = "HH':'mm':'ss";

        // Формат даты и времени, приминяемый в приложении.
        public const string DateTimeFormat = DateFormat + "'T'" + TimeFormat;

    }

    /*
     * Утилиты для работы с файловой системой.
     */
    public static class IOUtils
    {

        // Базовое название файла локальной БД.
        public const string FileName = "storage.json";

        /*
         * Получение адреса до файла локальной БД в файловой системе.
         * 
         * Адрес формируется из пути до директории пользователя в ОС и названия файла, 
         * состоящего из названия ключа, переданного в аргументе (аргумент 'key') и базового названия файла (константа 'FileName').
         * 
         * Пример. При:
         *  аргумент  key      = "Holiday"
         *  константа FileName = "storage.json"
         * Результат:
         *  C:\Users\Admin\holiday_storage.json
         */
        public static string GetFilePath(StorageKey key)
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(directory, key.ToString().ToLower() + "_" + FileName);
        }

    }

}
