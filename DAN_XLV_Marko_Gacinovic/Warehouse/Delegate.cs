using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Warehouse
{
    class Delegate
    {
        public delegate void Notification();

        public event Notification OnNotification;

        public void WarehouseFull(string article)
        {
            OnNotification += () =>
            {
                MessageBox.Show("Article '" + article + "' can not be stored because the amount is over 100");
            };
            OnNotification.Invoke();
        }

        
    }
}
