namespace WinFormsApp1
{
    
    /*
     * Класс с реализацией логики окна создания/редактирования праздников.
     */
    public partial class HolidayForm : Form
    {

        /*
         * Уникальный идентификатор.
         */
        private readonly string? uuid;

        /*
         * Название праздника.
         */
        private string? holidayName;

        /*
         * Дата праздника.
         */
        private DateTime holidayDate;

        /*
         * Конструктор класса с инициализацией компонентов и заполнением переменных.
         */
        public HolidayForm(string? name = null, DateTime? date = null, string? uuid = null)
        {
            this.holidayName = name;
            this.holidayDate = date ?? DateTime.Now;
            this.uuid = uuid;

            InitializeComponent();
        }

        /*
         * Инкапсулятор поля holidayName.
         */
        public string HolidayName { get => holidayName ?? ""; }

        /*
         * Инкапсулятор поля holidayDate.
         */
        public DateTime HolidayDate { get => holidayDate; }

        /*
         * Метод вызывается при создании окна.
         * 
         * Метод заполняет компоненты. В частности:
         *  в поле выбора даты вносится дата, переданная в конструктор этого класса.
         *  формируется название окна, в зависимости от наличия уникального идентификатора будет либо создание, либо редактирование праздника.
         *  в поле названия праздника проставляется значение, переданное в конструктор этого класса.
         */
        private void HolidayForm_Load(object? sender = null, EventArgs? e = null)
        {
            this.dateTimePicker1.Value = holidayDate;

            if (uuid == null)
            {
                this.DoneButton.Text = "Создать";
                this.Text = "Создание праздника";
            } 
            else
            {
                this.DoneButton.Text = "Изменить";
                this.Text = "Изменение праздника";
            }

            if (holidayName != null)
            {
                this.textBox1.Text = holidayName;
            }
        }

        /*
         * Метод вызывается при нажатии на кнопку готовности.
         * 
         * Метод проверяет наличие названия праздника. Если оно отсутствует - открывает диалоговое окно с ошибкой.
         * Заполняет поля в этом классе исходя из значений, внесенных в компоненты интерфейса. (название и дата)
         * Если все выполнено успешно, то меняет статус этого окна на OK и закрывает его.
         */
        private void DoneButton_Click(object? sender = null, EventArgs? e = null)
        {
            if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("Укажите название праздника", "Некорректные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.holidayName = this.textBox1.Text;
            this.holidayDate = this.dateTimePicker1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
