using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS11.Model
{
    class output
    {
        List<string> _files;
        string _status;
        int _nFiles;
        string _msg;
        string _completion;

        public List<string> files
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
            }
        }

        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
            }
        }
        public string completion
        {
            get
            {
                return _completion;
            }
            set
            {
                _completion = value;
            }
        }
        public int nFiles
        {
            get
            {
                return _nFiles;
            }
            set
            {
                _nFiles = value;
            }
        }
        
    }
}
