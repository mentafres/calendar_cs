namespace WinFormsApp1
{
    /*
     * Класс с реализацией логики окна создания/редактирования уведмолений.
     */
    public partial class NotificationForm : Form
    {

        /*
         * Уникальный идентификатор.
         */
        private readonly string? uuid;

        /*
         * Заголовок уведмоления.
         */
        private string? header;

        /*
         * Описание уведомления.
         */
        private string? description;

        /*
         * Время отображения уведомления.
         */
        private DateTime showAt;

        /*
         * Конструктор класса с инициализацией компонентов и заполнением переменных.
         */
        public NotificationForm(string? header = null, DateTime? showAt = null, string? description = null, string? uuid = null)
        {
            this.header = header;
            this.description = description;
            this.showAt = showAt ?? DateTime.Now;
            this.uuid = uuid;

            InitializeComponent();
        }

        /*
         * Инкапсулятор поля header.
         */
        public string NotificationHeader { get => header ?? ""; }

        /*
         * Инкапсулятор поля showAt.
         */
        public DateTime NotificationShowAt { get => showAt; }

        /*
         * Инкапсулятор поля description.
         */
        public string NotificationDescription { get => description ?? ""; }

        /*
         * Метод вызывается при создании окна.
         * 
         * Метод заполняет компоненты. В частности:
         *  в поле выбора времени вносится время, переданное в конструктор этого класса.
         *  формируется название окна, в зависимости от наличия уникального идентификатора будет либо создание, либо редактирование уведомления.
         *  в поле заголовка уведмоления проставляется значение, переданное в конструктор этого класса.
         *  в поле описания уведмоления проставляется значение, переданное в конструктор этого класса.
         */
        private void NotificationForm_Load(object? sender = null, EventArgs? e = null)
        {
            this.dateTimePicker1.Value = showAt;

            if (uuid == null)
            {
                this.DoneButton.Text = "Создать";
                this.Text = "Создание уведомления";
            }
            else
            {
                this.DoneButton.Text = "Изменить";
                this.Text = "Изменение уведомления";
            }

            if (header != null)
            {
                this.textBox1.Text = header;
            }
            
            if (description != null)
            {
                this.textBox2.Text = description;
            }
        }

        /*
         * Метод вызывается при нажатии на кнопку готовности.
         * 
         * Метод проверяет наличие заголовка уведомления. Если оно отсутствует - открывает диалоговое окно с ошибкой.
         * Заполняет поля в этом классе исходя из значений, внесенных в компоненты интерфейса. (заголовок, время и описание)
         * Если все выполнено успешно, то меняет статус этого окна на OK и закрывает его.
         */
        private void DoneButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("Укажите название уведомления", "Некорректные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.header = this.textBox1.Text;
            this.description = this.textBox2.Text;
            this.showAt = this.dateTimePicker1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
