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
using System.Windows.Shapes;

namespace WpfApp_Chebotarev
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public int UserId { get; set; }

        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        public ChangePasswordWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }
    }
}
