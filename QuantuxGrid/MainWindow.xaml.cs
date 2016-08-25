using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuantuxGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> persons = new List<Person>();
        List<Headings> heading = new List<Headings>();
        public MainWindow()
        {
            InitializeComponent();

            persons.Add(new Person()
            {
                Symbol = "Jahanzeb",
                FutureContract = "Sayal",
                Class="test Class",
                SubClass="Test Sub Class",
                ContractMonth="Contract Months",
                TickSize=2.3,
                TickValue=5,
                Exchange="Exchange"
                
            });

            persons.Add(new Person()
            {
                Symbol = "Jahanzeb",
                FutureContract = "Sayal",
                Class = "test Class",
                SubClass = "Test Sub Class",
                ContractMonth = "Contract Months",
                TickSize = 2.3,
                TickValue = 5,
                Exchange = "Exchange"
            });

            persons.Add(new Person()
            {
                Symbol = "Jahanzeb",
                FutureContract = "Sayal",
                Class = "test Class",
                SubClass = "Test Sub Class",
                ContractMonth = "Contract Months",
                TickSize = 2.3,
                TickValue = 5,
                Exchange = "Exchange"
            });

            persons.Add(new Person()
            {
                Symbol = "Jahanzeb",
                FutureContract = "Sayal",
                Class = "test Class",
                SubClass = "Test Sub Class",
                ContractMonth = "Contract Months",
                TickSize = 2.3,
                TickValue = 5,
                Exchange = "Exchange"
            });

            grid.GridItemSource = persons;
            grid.LoadData();
        }
    }
}
