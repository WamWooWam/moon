using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace System.Windows.Test
{
	public sealed partial class UserControl1 : UserControl
	{
		public UserControl1()
		{
            //this.InitializeComponent();

            System.Windows.Application.LoadComponent(this, Resource1.UserControl1);
        }
	}
}
