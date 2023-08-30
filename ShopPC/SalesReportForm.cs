using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopPC
{
    public partial class SalesReportForm : Form
    {
        public SalesReportForm(List<SalesSummaryEntry> salesSummaryData)
        {
            InitializeComponent();

            dataGridViewSalesSummary.DataSource = salesSummaryData;

            // После заполнения DataGridView данными
            dataGridViewSalesSummary.Columns["SellerName"].HeaderText = "Продавец"; // Заменяет название колонки "SellerName" на "Продавец"
            dataGridViewSalesSummary.Columns["TotalQuantity"].HeaderText = "Количество"; // Заменяет название колонки "TotalQuantity" на "Количество"
            dataGridViewSalesSummary.Columns["TotalSalesAmount"].HeaderText = "Сумма продаж"; // Заменяет название колонки "TotalSalesAmount" на "Сумма продаж"

        }

    }
}
