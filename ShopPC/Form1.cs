using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlTypes;

namespace ShopPC
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=ShopPc.db;";

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            LoadCategories();
            LoadSuppliers();
            LoadUsers();
            LoadCategoryNames();
            LoadSupplierNames();
            LoadProducts();
            LoadCategoryNamesSearch();
            LoadSales();
            LoadUsersComboBox();
            LoadRolesComboBox();
        }

        private void InitializeDataGridView()
        {
            // Добавляем столбцы в DataGridView
            dataGridViewCategories.Columns.Add("Id", "ID");
            dataGridViewCategories.Columns.Add("Name", "Название");
            dataGridViewCategories.Columns.Add("Description", "Описание");

            // Добавляем столбцы в DataGridView для поставщиков
            dataGridViewSuppliers.Columns.Add("Id", "ID");
            dataGridViewSuppliers.Columns.Add("Title", "Название");
            dataGridViewSuppliers.Columns.Add("Name", "Имя");
            dataGridViewSuppliers.Columns.Add("Phone", "Телефон");
            dataGridViewSuppliers.Columns.Add("Address", "Адрес");

            // Добавляем столбцы в DataGridView для пользователей
            dataGridViewUsers.Columns.Add("Id", "ID");
            dataGridViewUsers.Columns.Add("Name", "Имя");
            dataGridViewUsers.Columns.Add("Role", "Роль");

            // Добавляем столбцы в DataGridView для товаров
            dataGridViewProducts.Columns.Add("Id", "ID");
            dataGridViewProducts.Columns.Add("Title", "Название");
            dataGridViewProducts.Columns.Add("Characteristic", "Характеристика");
            dataGridViewProducts.Columns.Add("Quantity", "Количество");
            dataGridViewProducts.Columns.Add("Cost", "Стоимость");
            dataGridViewProducts.Columns.Add("Category", "Категория");
            dataGridViewProducts.Columns.Add("Supplier", "Поставщик");

            // Добавляем столбцы в DataGridView для продаж
            dataGridViewSales.Columns.Add("Id", "ID");
            dataGridViewSales.Columns.Add("Product", "Товар");
            dataGridViewSales.Columns.Add("Quantity", "Количество");
            dataGridViewSales.Columns.Add("TotalCost", "Общая стоимость");
            dataGridViewSales.Columns.Add("Seller", "Продавец");
            dataGridViewSales.Columns.Add("SaleDate", "Дата продажи");
        }

        private void LoadCategories()
        {
            dataGridViewCategories.Rows.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Description FROM Categories";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);

                        dataGridViewCategories.Rows.Add(id, name, description);
                    }
                }
            }
        }

        private void LoadSuppliers()
        {
            dataGridViewSuppliers.Rows.Clear();

            List<Supplier> suppliers = Supplier.GetAllSuppliers(connectionString);

            foreach (Supplier supplier in suppliers)
            {
                dataGridViewSuppliers.Rows.Add(supplier.Id, supplier.Title, supplier.Name, supplier.Phone, supplier.Address);
            }

        }

        private void LoadUsers()
        {
            dataGridViewUsers.Rows.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Role FROM Users";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string role = reader.GetString(2);

                        dataGridViewUsers.Rows.Add(id, name, role);
                    }
                }
            }
        }

        private void LoadProducts()
        {
            dataGridViewProducts.Rows.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Title, Characteristic, Quantity, Cost, CategoryId, SupplierId FROM Products";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string title = reader.GetString(1);
                        string characteristic = reader.GetString(2);
                        int quantity = Convert.ToInt32(reader["Quantity"]);
                        decimal cost = Convert.ToDecimal(reader["Cost"]);
                        int categoryId = Convert.ToInt32(reader["CategoryId"]);
                        int supplierId = Convert.ToInt32(reader["SupplierId"]);

                        string categoryName = Product.GetCategoryNameById(categoryId, connection);
                        string supplierName = Product.GetSupplierNameById(supplierId, connection);

                        dataGridViewProducts.Rows.Add(id, title, characteristic, quantity, cost, categoryName, supplierName);
                        dataGridViewProducts.Rows[0].Selected = false;

                    }
                }
            }
        }

        private void LoadCategoryNames()
        {
            comboBoxCategories.Items.Clear(); // Очищаем элементы в ComboBox

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM Categories";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categoryName = reader.GetString(0);
                        comboBoxCategories.Items.Add(categoryName);
                    }
                }
            }
        }

        private void LoadSupplierNames()
        {
            comboBoxSuppliers.Items.Clear(); // Очищаем элементы в ComboBox

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM Suppliers";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string supplierName = reader.GetString(0);
                        comboBoxSuppliers.Items.Add(supplierName);
                    }
                }
            }
        }

        private void LoadCategoryNamesSearch()
        {
            comboBoxSearchCategories.Items.Clear(); // Очищаем элементы в ComboBox

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM Categories";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categoryName = reader.GetString(0);
                        comboBoxSearchCategories.Items.Add(categoryName);
                    }
                }
            }
        }

        private void LoadSales()
        {
            dataGridViewSales.Rows.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, ProductId, Quantity, TotalCost, SellerId, SaleDate FROM Sales";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int productId = reader.GetInt32(1);
                        int quantity = Convert.ToInt32(reader["Quantity"]);
                        decimal totalCost = Convert.ToDecimal(reader["TotalCost"]);
                        int sellerId = Convert.ToInt32(reader["SellerId"]);
                        DateTime saleDate = Convert.ToDateTime(reader["SaleDate"]);

                        string productName = GetProductNameById(productId, connection);
                        string sellerName = GetSellerNameById(sellerId, connection);

                        dataGridViewSales.Rows.Add(id, productName, quantity, totalCost, sellerName, saleDate);
                    }
                }
            }
        }

        private void LoadUsersComboBox()
        {
            comboBoxUsers.DisplayMember = "Name";

            List<User> allUsers = User.GetAllUsers(connectionString);
            List<User> sellerUsers = allUsers.Where(user => user.Role == User.UserRole.Seller).ToList();

            BindingSource bindingSource = new BindingSource(sellerUsers, null);
            comboBoxUsers.DataSource = bindingSource;
        }


        private void LoadRolesComboBox()
        {
            comboBoxUserRole.Items.Clear();
            comboBoxUserRole.Items.Add(User.UserRole.Administrator);
            comboBoxUserRole.Items.Add(User.UserRole.Seller);
            comboBoxUserRole.SelectedIndex = 0; // Выберите значение по умолчанию
        }

        private string GetProductNameById(int productId, SQLiteConnection connection)
        {
            string query = "SELECT Title FROM Products WHERE Id = @ProductId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }

        private string GetSellerNameById(int sellerId, SQLiteConnection connection)
        {
            string query = "SELECT Name FROM Users WHERE Id = @SellerId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SellerId", sellerId);
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }

        private int GetCurrentUserId()
        {
            User selectedUser = comboBoxUsers.SelectedItem as User;

            if (selectedUser != null)
            {
                return selectedUser.Id;
            }

            return -1; // Вернуть -1 или другое значение по умолчанию, если ни один пользователь не выбран
        }

        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBoxCategoryName.Text;
                string description = textBoxCategoryDescription.Text;

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Введите имя категории.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Category newCategory = new Category(name, description);
                newCategory.AddToDatabase(connectionString); // Передача строки подключения

                // Очищаем поля и обновляем список категорий
                textBoxCategoryName.Clear();
                textBoxCategoryDescription.Clear();
                LoadCategories();
                MessageBox.Show("Категория успешно добавлена.", "Добавление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoryNamesSearch();
                LoadCategoryNames();
            }
            catch (InvalidOperationException ex)
            {
                // Обрабатываем исключение для случая, когда категория уже существует
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении категории:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                int selectedCategoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells["Id"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту категорию?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Category categoryToDelete = new Category();
                    categoryToDelete.Id = selectedCategoryId;
                    categoryToDelete.DeleteFromDatabase(connectionString);

                    LoadCategories(); // Обновляем список после удаления

                    MessageBox.Show("Категория успешно удалена.", "Удаление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxCategoryName.Clear();
                    textBoxCategoryDescription.Clear();

                    LoadCategoryNamesSearch();
                    LoadCategoryNames();
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveCategory_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                int selectedCategoryId = Convert.ToInt32(dataGridViewCategories.SelectedRows[0].Cells["Id"].Value);
                string newName = textBoxCategoryName.Text;
                string newDescription = textBoxCategoryDescription.Text;

                // Выполняем обновление данных
                Category categoryToUpdate = new Category();
                categoryToUpdate.Id = selectedCategoryId;
                categoryToUpdate.Name = newName;
                categoryToUpdate.Description = newDescription;
                categoryToUpdate.UpdateInDatabase(connectionString);

                LoadCategories(); // Обновляем список после обновления

                MessageBox.Show("Информация о категории успешно обновлена.", "Обновление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxCategoryName.Clear();
                textBoxCategoryDescription.Clear();
                LoadCategoryNamesSearch();
                LoadCategoryNames();
            }
        }

        private void dataGridViewCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewCategories.SelectedRows[0];

                // Получаем значения из выбранной строки
                int selectedCategoryId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string selectedName = selectedRow.Cells["Name"].Value.ToString();
                string selectedDescription = selectedRow.Cells["Description"].Value.ToString();

                // Отображаем значения в текстовых полях
                textBoxCategoryName.Text = selectedName;
                textBoxCategoryDescription.Text = selectedDescription;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearch.Text.Trim(); // Получаем поисковый запрос

            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Category> searchResults = Category.SearchCategories(searchQuery, connectionString);

                dataGridViewCategories.Rows.Clear();
                foreach (Category category in searchResults)
                {
                    dataGridViewCategories.Rows.Add(category.Id, category.Name, category.Description);
                }
            }
            else
            {
                LoadCategories(); // Если поисковый запрос пуст, загрузить все категории
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            buttonSearch_Click(sender, e); // Вызываем обработчик кнопки "Поиск"
        }

        private void textBoxSearchSuppliers_TextChanged(object sender, EventArgs e)
        {
            buttonSearchSuppliers_Click(sender, e);
        }

        private void buttonAddSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                string title = textBoxSupplierTitle.Text;
                string name = textBoxSupplierName.Text;
                string phone = textBoxSupplierPhone.Text;
                string address = textBoxSupplierAddress.Text;

                if (string.IsNullOrWhiteSpace(title))
                {
                    MessageBox.Show("Введите название поставщика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Supplier newSupplier = new Supplier(title, name, phone, address);
                newSupplier.AddToDatabase(connectionString);

                textBoxSupplierTitle.Clear();
                textBoxSupplierName.Clear();
                textBoxSupplierPhone.Clear();
                textBoxSupplierAddress.Clear();

                LoadSuppliers();

                MessageBox.Show("Поставщик успешно добавлен.", "Добавление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSupplierNames();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении поставщика:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0)
            {
                int selectedSupplierId = Convert.ToInt32(dataGridViewSuppliers.SelectedRows[0].Cells["Id"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этого поставщика?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Supplier supplierToDelete = new Supplier();
                    supplierToDelete.Id = selectedSupplierId;
                    supplierToDelete.DeleteFromDatabase(connectionString);

                    LoadSuppliers(); // Обновляем список после удаления

                    MessageBox.Show("Поставщик успешно удален.", "Удаление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxSupplierTitle.Clear();
                    textBoxSupplierName.Clear();
                    textBoxSupplierPhone.Clear();
                    textBoxSupplierAddress.Clear();

                    LoadSupplierNames();
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveSupplier_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0)
            {
                int selectedSupplierId = Convert.ToInt32(dataGridViewSuppliers.SelectedRows[0].Cells["Id"].Value);
                string newTitle = textBoxSupplierTitle.Text;
                string newName = textBoxSupplierName.Text;
                string newPhone = textBoxSupplierPhone.Text;
                string newAddress = textBoxSupplierAddress.Text;

                // Выполняем обновление данных
                Supplier supplierToUpdate = new Supplier();
                supplierToUpdate.Id = selectedSupplierId;
                supplierToUpdate.Title = newTitle;
                supplierToUpdate.Name = newName;
                supplierToUpdate.Phone = newPhone;
                supplierToUpdate.Address = newAddress;
                supplierToUpdate.UpdateInDatabase(connectionString);

                LoadSuppliers(); // Обновляем список после обновления

                MessageBox.Show("Информация о поставщике успешно обновлена.", "Обновление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxSupplierTitle.Clear();
                textBoxSupplierName.Clear();
                textBoxSupplierPhone.Clear();
                textBoxSupplierAddress.Clear();

                LoadSupplierNames();
            }
        }

        private void buttonSearchSuppliers_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearchSuppliers.Text.Trim();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Supplier> searchResults = Supplier.SearchSuppliers(searchQuery, connectionString);

                dataGridViewSuppliers.Rows.Clear();
                foreach (Supplier supplier in searchResults)
                {
                    dataGridViewSuppliers.Rows.Add(supplier.Id, supplier.Title, supplier.Name, supplier.Phone, supplier.Address);
                }
            }
            else
            {
                LoadSuppliers();
            }
        }

        private void dataGridViewSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewSuppliers.SelectedRows[0];

                int selectedSupplierId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string selectedSupplierTitle = selectedRow.Cells["Title"].Value.ToString();
                string selectedSupplierName = selectedRow.Cells["Name"].Value.ToString();
                string selectedSupplierPhone = selectedRow.Cells["Phone"].Value.ToString();
                string selectedSupplierAddress = selectedRow.Cells["Address"].Value.ToString();

                textBoxSupplierTitle.Text = selectedSupplierTitle;
                textBoxSupplierName.Text = selectedSupplierName;
                textBoxSupplierPhone.Text = selectedSupplierPhone;
                textBoxSupplierAddress.Text = selectedSupplierAddress;
            }
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBoxUserName.Text;
                string role = comboBoxUserRole.SelectedItem as string; // Получите выбранную роль из ComboBox

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("Введите имя пользователя и выберите роль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                User newUser = new User(name, role);
                newUser.AddToDatabase(connectionString);

                textBoxUserName.Clear();
                comboBoxUserRole.SelectedIndex = -1; // Сброс выбранной роли

                LoadUsers();
                LoadUsersComboBox(); // Обновите данные в ComboBox

                MessageBox.Show("Пользователь успешно добавлен.", "Добавление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении пользователя:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                int selectedUserId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["Id"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    User userToDelete = new User();
                    userToDelete.Id = selectedUserId;
                    userToDelete.DeleteFromDatabase(connectionString);

                    LoadUsers();
                    MessageBox.Show("Пользователь успешно удален.", "Удаление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxUserName.Clear();
                    comboBoxUserRole.SelectedIndex = 0; // Сброс выбранной роли на значение по умолчанию

                    LoadUsersComboBox();

                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                int selectedUserId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["Id"].Value);
                string newName = textBoxUserName.Text;
                string newRole = comboBoxUserRole.SelectedItem.ToString(); // Извлекаем выбранную роль из ComboBox

                User userToUpdate = new User();
                userToUpdate.Id = selectedUserId;
                userToUpdate.Name = newName;
                userToUpdate.Role = newRole;
                userToUpdate.UpdateInDatabase(connectionString);

                LoadUsers();
                MessageBox.Show("Информация о пользователе успешно обновлена.", "Обновление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxUserName.Clear();
                comboBoxUserRole.SelectedIndex = 0; // Сбрасываем выбранную роль на значение по умолчанию


            }
        }


        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewUsers.SelectedRows[0];

                int selectedUserId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string selectedName = selectedRow.Cells["Name"].Value.ToString();
                string selectedRole = selectedRow.Cells["Role"].Value.ToString();

                textBoxUserName.Text = selectedName;
                comboBoxUserRole.SelectedItem = selectedRole; // Установите выбранную роль в ComboBox

                // Другой код, если необходимо
            }
        }


        private void buttonSearchUser_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearchUser.Text.Trim();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<User> searchResults = User.SearchUsers(searchQuery, connectionString);

                dataGridViewUsers.Rows.Clear();
                foreach (User user in searchResults)
                {
                    dataGridViewUsers.Rows.Add(user.Id, user.Name, user.Role);
                }
            }
            else
            {
                LoadUsers();
            }
        }

        private void textBoxSearchUser_TextChanged(object sender, EventArgs e)
        {
            buttonSearchUser_Click(sender, e);
        }

        private void buttonAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string title = textBoxProductTitle.Text;
                string characteristic = textBoxProductCharacteristic.Text;
                int quantity = Convert.ToInt32(numericUpDownQuantity.Value);
                decimal cost = numericUpDownCost.Value;
                string categoryName = comboBoxCategories.SelectedItem.ToString();
                string supplierName = comboBoxSuppliers.SelectedItem.ToString();

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(characteristic) || comboBoxCategories.SelectedItem == null || comboBoxSuppliers.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Product newProduct = new Product
                {
                    Title = title,
                    Characteristic = characteristic,
                    Quantity = quantity,
                    Cost = cost,
                    CategoryName = categoryName,
                    SupplierName = supplierName
                };

                newProduct.AddToDatabase(connectionString);
                ClearProductFields();
                LoadProducts();

                MessageBox.Show("Товар успешно добавлен.", "Добавление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении товара:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                int selectedProductId = Convert.ToInt32(dataGridViewProducts.SelectedRows[0].Cells["Id"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Product productToDelete = new Product();
                    productToDelete.Id = selectedProductId;
                    productToDelete.DeleteFromDatabase(connectionString);

                    LoadProducts();
                    ClearProductFields();

                    MessageBox.Show("Товар успешно удален.", "Удаление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                int selectedProductId = Convert.ToInt32(dataGridViewProducts.SelectedRows[0].Cells["Id"].Value);
                string newTitle = textBoxProductTitle.Text;
                string newcharacteristic = textBoxProductCharacteristic.Text;
                int newQuantity = Convert.ToInt32(numericUpDownQuantity.Value);
                decimal newCost = numericUpDownCost.Value;
                string newCategoryName = comboBoxCategories.SelectedItem.ToString();
                string newSupplierName = comboBoxSuppliers.SelectedItem.ToString();

                Product productToUpdate = new Product
                {
                    Id = selectedProductId,
                    Title = newTitle,
                    Characteristic = newcharacteristic,
                    Quantity = newQuantity,
                    Cost = newCost,
                    CategoryName = newCategoryName,
                    SupplierName = newSupplierName
                };

                productToUpdate.UpdateInDatabase(connectionString);
                LoadProducts();
                ClearProductFields();

                MessageBox.Show("Информация о товаре успешно обновлена.", "Обновление выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];

                int selectedProductId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string selectedTitle = selectedRow.Cells["Title"].Value.ToString();
                string selectedCharacteristic = selectedRow.Cells["Characteristic"].Value.ToString();
                int selectedQuantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value);
                decimal selectedCost = Convert.ToDecimal(selectedRow.Cells["Cost"].Value);
                string selectedCategoryName = selectedRow.Cells["Category"].Value.ToString();
                string selectedSupplierName = selectedRow.Cells["Supplier"].Value.ToString();

                textBoxProductTitle.Text = selectedTitle;
                textBoxProductCharacteristic.Text = selectedCharacteristic;
                numericUpDownQuantity.Value = selectedQuantity;
                numericUpDownCost.Value = selectedCost;
                comboBoxCategories.SelectedItem = selectedCategoryName;
                comboBoxSuppliers.SelectedItem = selectedSupplierName;
            }
        }

        private void buttonSearchProduct_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearchProduct.Text.Trim();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                decimal minPrice = numericUpDownMinPrice.Value;
                decimal maxPrice = numericUpDownMaxPrice.Value;

                bool usePriceRange = checkBoxPriceRange.Checked;

                List<Product> searchResults;

                if (usePriceRange)
                {
                    searchResults = Product.SearchProducts(searchQuery, minPrice, maxPrice, connectionString);
                }
                else
                {
                    searchResults = Product.SearchProducts(searchQuery, connectionString);
                }

                dataGridViewProducts.Rows.Clear();
                foreach (Product product in searchResults)
                {
                    dataGridViewProducts.Rows.Add(product.Id, product.Title, product.Characteristic, product.Quantity, product.Cost, product.CategoryName, product.SupplierName);
                }
            }
            else
            {
                LoadProducts();
            }
        }

        private void textBoxSearchProduct_TextChanged(object sender, EventArgs e)
        {
            buttonSearchProduct_Click(sender, e);
        }

        private void ClearProductFields()
        {
            textBoxProductTitle.Clear();
            textBoxProductCharacteristic.Clear();
            numericUpDownQuantity.Value = 0;
            numericUpDownCost.Value = 0;
            comboBoxCategories.SelectedIndex = -1;
            comboBoxSuppliers.SelectedIndex = -1;
        }

        private void checkBoxPriceRange_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownMinPrice.Enabled = checkBoxPriceRange.Checked;
            numericUpDownMaxPrice.Enabled = checkBoxPriceRange.Checked;
        }

        private void buttonSearchProductPriceRange_Click(object sender, EventArgs e)
        {
            decimal minPrice = numericUpDownMinPrice.Value;
            decimal maxPrice = numericUpDownMaxPrice.Value;

            List<Product> searchResults = Product.SearchProductsByPriceRange(minPrice, maxPrice, connectionString);

            dataGridViewProducts.Rows.Clear();
            foreach (Product product in searchResults)
            {
                dataGridViewProducts.Rows.Add(product.Id, product.Title, product.Characteristic, product.Quantity, product.Cost, product.CategoryName, product.SupplierName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedCategoryName = comboBoxSearchCategories.SelectedItem as string;

            decimal minPrice = checkBoxPriceRange.Checked ? numericUpDownMinPrice.Value : decimal.MinValue;
            decimal maxPrice = checkBoxPriceRange.Checked ? numericUpDownMaxPrice.Value : decimal.MaxValue;

            List<Product> searchResults;

            if (string.IsNullOrEmpty(selectedCategoryName))
            {
                searchResults = Product.SearchProductsByPriceRange(minPrice, maxPrice, connectionString);
            }
            else
            {
                searchResults = Product.SearchProductsByCategoryAndPriceRange(selectedCategoryName, minPrice, maxPrice, connectionString);
            }

            dataGridViewProducts.Rows.Clear();
            foreach (Product product in searchResults)
            {
                dataGridViewProducts.Rows.Add(product.Id, product.Title, product.Characteristic, product.Quantity, product.Cost, product.CategoryName, product.SupplierName);
            }
        }

        private void buttonSearchByCharacteristic_Click(object sender, EventArgs e)
        {
            string characteristicSearchQuery = textBoxSearchCharacteristic.Text.Trim();

            if (!string.IsNullOrEmpty(characteristicSearchQuery))
            {
                List<Product> searchResults = Product.SearchProductsByCharacteristic(characteristicSearchQuery, connectionString);

                dataGridViewProducts.Rows.Clear();
                foreach (Product product in searchResults)
                {
                    dataGridViewProducts.Rows.Add(product.Id, product.Title, product.Characteristic, product.Quantity, product.Cost, product.CategoryName, product.SupplierName);
                }
            }
            else
            {
                LoadProducts();
            }
        }

        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridViewProducts.Rows[e.RowIndex];
                int productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string productName = selectedRow.Cells["Title"].Value.ToString();
                decimal productCost = Convert.ToDecimal(selectedRow.Cells["Cost"].Value);

                // Получите значение количества из NumericUpDown
                int quantity = (int)numericUpDownSaleQuantity.Value;

                // Создаем объект Sale с необходимыми данными
                Sale sale = new Sale
                {
                    ProductId = productId,
                    Quantity = quantity,
                    TotalCost = productCost * quantity,
                    SellerId = GetCurrentUserId(), // Получите ID текущего пользователя
                    SaleDate = DateTime.Now
                };

                // Обновляем DataGridView для отображения продаж
                LoadSales();
            }
        }


        private void buttonAddToSales_Click(object sender, EventArgs e)
        {
            User selectedUser = comboBoxUsers.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int quantity = (int)numericUpDownSaleQuantity.Value; // Преобразование decimal в int

            if (quantity == 0)
            {
                MessageBox.Show("Количество продаж не может быть равно нулю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                foreach (DataGridViewRow selectedRow in dataGridViewProducts.SelectedRows)
                {
                    int productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    decimal productCost = Convert.ToDecimal(selectedRow.Cells["Cost"].Value);

                    // Получите количество товара из базы данных
                    int availableQuantity = GetAvailableQuantity(productId);

                    // Получите количество товара из NumericUpDown
                    int requestedQuantity = (int)numericUpDownSaleQuantity.Value;

                    if (requestedQuantity > availableQuantity)
                    {
                        MessageBox.Show($"Недостаточное количество товара \"{selectedRow.Cells["Title"].Value}\". Доступное количество: {availableQuantity}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue; // Пропустить добавление этого товара и перейти к следующему
                    }

                    // Ограничьте количество продаж максимальным доступным количеством товара
                    int finalQuantity = Math.Min(requestedQuantity, availableQuantity);

                    // Создаем объект Sale с необходимыми данными
                    Sale sale = new Sale
                    {
                        ProductId = productId,
                        Quantity = finalQuantity,
                        TotalCost = productCost * finalQuantity,
                        SellerId = selectedUser.Id,
                        SaleDate = DateTime.Now
                    };

                    // Добавляем запись о продаже в базу данных

                    sale.AddToDatabase(connectionString);
                    // Обновляем DataGridView для отображения продаж
                    Product product = new Product(); // Создаем экземпляр класса Product
                    product.UpdateProductQuantity(connectionString, productId, availableQuantity - finalQuantity);

                    LoadSales();
                    LoadProducts();

                    MessageBox.Show("Продажа успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Сбрасываем ComboBox и устанавливаем значение NumericUpDown в 0
                    comboBoxUsers.SelectedIndex = -1;
                    numericUpDownSaleQuantity.Value = 0;
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении продажи:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для получения доступного количества товара
        private int GetAvailableQuantity(int productId)
        {
            Product product = Product.GetProductById(productId, connectionString);
            if (product != null)
            {
                return product.Quantity;
            }
            return 0;
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                dataGridViewProducts.ClearSelection();
                ClearProductFields();
            }
        }
    }
}
