namespace WinFormsApp1
{

    /*
     * Класс с реализацией логики главного окна приложения.
     */
    public partial class MainForm : Form
    {

        /*
         * Переменная с экземпляром класса, отвечающим за логику приложения.
         */
        private readonly CoreLogic logic;

        /*
         * Конструтор класса.
         * Инициализирует компоненты и заполняет внутренние переменные.
         */
        public MainForm(CoreLogic logic)
        {
            this.logic = logic;

            InitializeComponent();
        }

        /*
         * Метод вызывается при открытии окна приложения.
         * 
         * Загружается список сегодняшних праздников, список ближайших уведомлений, обновляется информация о текущем времени и дне недели.
         * Запускается таймер, который каждую секунду обновляет все вышеперечисленную информацию в окне приложения, 
         * а так же таймер проверяет необходимость отображения уведомления в нужный момент.
         * 
         * Таймер вызывает методы:
         *  LabelCurrentDateTime_Update
         *  TodayHolidayListBox_Update
         *  TodayNotificationListBox_Update
         *  ShowNotification
         */
        private void MainForm_Load(object? sender = null, EventArgs? e = null)
        {
            LabelCurrentDateTime_Update();
            HolidayListBox_Update();
            NotificationListBox_Update();

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(LabelCurrentDateTime_Update);
            timer.Tick += new EventHandler(TodayHolidayListBox_Update);
            timer.Tick += new EventHandler(TodayNotificationListBox_Update);
            timer.Tick += new EventHandler(ShowNotification);
            timer.Start();
        }

        /*
         * Метод ищет и отображает уведомления, время у которых совпадает с текущем временем (равны секунды, минуты, часы и полная дата).
         * 
         * Список уведомлений получается из NotificationManager#GetNotifications().
         */
        private void ShowNotification(object? sender = null, EventArgs? e = null)
        {
            var time = DateTime.Now;
            var notification = logic.Notifications
                .GetNotifications()
                .Find(n => n.ShowAt.Second == time.Second && n.ShowAt.Minute == time.Minute && n.ShowAt.Hour == time.Hour && n.ShowAt.Date == time.Date);
            
            if (notification != null)
            {
                MessageBox.Show(notification.Description, notification.Header, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /*
         * Метод вызывается каждую секунду для обновления текущего времени и дня недели.
         * 
         * День недели получается из: CoreLogic.GetDayOfWeekString().
         * Время получается из: CoreLogic.GetTimeString().
         */
        private void LabelCurrentDateTime_Update(object? sender = null, EventArgs? e = null)
        {
            LabelCurrentDateTime.Text = "Текущее время: " + CoreLogic.GetDayOfWeekString() + ", " + CoreLogic.GetTimeString();
            LabelCurrentDateTime.Refresh();
        }

        /*
         * Метод вызывается каждую секунду для обновления сегодняшних праздников.
         * 
         * Список праздников получается из HolidayManager#GetTodayHolidays().
         */
        private void TodayHolidayListBox_Update(object? sender = null, EventArgs? e = null)
        {
            TodayHolidayListBox.Items.Clear();
            logic.Holidays
                .GetTodayHolidays()
                .ForEach(holiday => TodayHolidayListBox.Items.Add(holiday));
            TodayHolidayListBox.DisplayMember = "name";
            TodayHolidayListBox.Refresh();
        }

        /*
         * Метод вызывается каждую секунду для обновления всех праздников.
         * 
         * Список праздников получается из HolidayManager#GetHolidays() и сортируется по дате.
         */
        private void HolidayListBox_Update(object? sender = null, EventArgs? e = null)
        {
            HolidayListBox.Items.Clear();
            logic.Holidays
                .GetHolidays()
                .OrderBy(holiday => holiday.Date)
                .ToList()
                .ForEach(holiday => HolidayListBox.Items.Add(holiday));
            HolidayListBox.Refresh();
        }

        /*
         * Метод вызывается каждую секунду для обновления ближайших уведомлений.
         * 
         * Список праздников получается из NotificationManager#GetTodayNotifications().
         */
        private void TodayNotificationListBox_Update(object? sender = null, EventArgs? e = null)
        {
            TodayNotificationListBox.Items.Clear();
            logic.Notifications.GetTodayNotifications().ForEach(n => TodayNotificationListBox.Items.Add(n));
            TodayNotificationListBox.Refresh();
        }

        /*
         * Метод вызывается каждую секунду для обновления всех уведомлений.
         * 
         * Список праздников получается из NotificationManager#GetNotifications() и сортируется по времени.
         */
        private void NotificationListBox_Update(object? sender = null, EventArgs? e = null)
        {
            NotificationListBox.Items.Clear();
            logic.Notifications
                .GetNotifications()
                .OrderBy(n => n.ShowAt)
                .ToList()
                .ForEach(n => NotificationListBox.Items.Add(n));
            NotificationListBox.Refresh();
        }

        /*
         * Метод вызывается при нажатии на кнопку создания праздника.
         * 
         * Открывается диалоговое окно HolidayForm и по результатам взаимодействия с ним создается праздник.
         */
        private void CreateHolidayButton_Click(object? sender = null, EventArgs? e = null)
        {
            var form = new HolidayForm();
            form.ShowDialog();
            
            if (form.DialogResult == DialogResult.OK)
            {
                logic.Holidays.CreateHoliday(form.HolidayName, form.HolidayDate);
            }

            this.HolidayListBox_Update();
        }

        /*
         * Метод вызывается при нажатии на кнопку редактирования праздника.
         * 
         * Открывается диалоговое окно HolidayForm и по результатам взамодействия с ним изменяются данные выбранного праздника.
         */
        private void EditHoliayButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.HolidayListBox.SelectedItem is not Holiday holiday)
            {
                MessageBox.Show("Выберите праздник", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var form = new HolidayForm(holiday.Name, holiday.Date, holiday.Uuid);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                logic.Holidays.EditHoliday(holiday.Uuid, new Holiday(form.HolidayName, form.HolidayDate));
                this.HolidayListBox_Update();
            }
        }

        /*
         * Метод вызывается при нажатии на кнопку удаления праздника.
         * 
         * Происходит удаление выбранного праздника.
         */
        private void DeleteHolidayButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.HolidayListBox.SelectedItem is not Holiday selectedHoliday)
            {
                MessageBox.Show("Выберите праздник", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                logic.Holidays.RemoveHoliday(selectedHoliday.Uuid);
                this.HolidayListBox_Update();
            }
        }

        /*
         * Метод вызывается при нажатии на кнопку создания уведомления.
         * 
         * Открывается диалоговое окно NotificationForm и по результатам взаимодействия с ним создается уведомление.
         */
        private void CreateNotificationButton_Click(object? sender = null, EventArgs? e = null)
        {
            var form = new NotificationForm();
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                logic.Notifications.CreateNotification(form.NotificationHeader, form.NotificationShowAt, form.NotificationDescription);
            }

            this.NotificationListBox_Update();
        }

        /*
         * Метод вызывается при нажатии на кнопку редактирования уведомления.
         * 
         * Открывается диалоговое окно NotificationForm и по результатам взамодействия с ним изменяются данные выбранного уведомления.
         */
        private void EditNotificationButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.NotificationListBox.SelectedItem is not Notification notification)
            {
                MessageBox.Show("Выберите уведомление", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var form = new NotificationForm(notification.Header, notification.ShowAt, notification.Description, notification.Uuid);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                logic.Notifications.EditNotification(notification.Uuid, new Notification(form.NotificationHeader, form.NotificationShowAt, form.NotificationDescription));
                this.NotificationListBox_Update();
            }
        }

        /*
         * Метод вызывается при нажатии на кнопку удаления уведомления.
         * 
         * Происходит удаление выбранного уведомления.
         */
        private void DeleteNotificationButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.NotificationListBox.SelectedItem is not Notification notification)
            {
                MessageBox.Show("Выберите уведомление", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                logic.Notifications.RemoveNotification(notification.Uuid);
                this.NotificationListBox_Update();
            }
        }

    }
}