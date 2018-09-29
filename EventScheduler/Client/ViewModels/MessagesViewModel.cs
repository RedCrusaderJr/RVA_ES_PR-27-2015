using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.ViewModels
{
    public class MessagesViewModel
    {
        TextBlock InfoTextBlock { get; set; }

        public MessagesViewModel()
        {
            InfoTextBlock = new TextBlock();
        }
    }
}
