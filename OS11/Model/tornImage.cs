using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OS11.Model
{
	public class tornImage
	{
		//[XmlElement("_tornImage")]
        Uri _tornImageUri;
		int _tornImageIndex;
		public tornImage(Uri _tornImageUri)
		{
			this._tornImageUri = _tornImageUri;
		}
		public Uri tornImageUri
        {
            get
            {
                return _tornImageUri;
            }
            set
            {
                _tornImageUri = value;
            }
        }
		public int tornImageIndex
        {
            get
            {
                return _tornImageIndex;
            }
            set
            {
                _tornImageIndex = value;
            }
        }
	}
}