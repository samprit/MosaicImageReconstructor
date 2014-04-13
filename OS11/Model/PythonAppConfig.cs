using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS11.Model
{
    class PythonAppConfig
    {
        string _arcLengthmultiplier;
        string _noiseReduction;
        string _scaleFactor;
        string _approxPolyparameter;
        string _multiEdge;
        string _backgroundThreshold;

        public string arcLengthMultiplier
        {
            get
            {
                return _arcLengthmultiplier;
            }
            set
            {
                _arcLengthmultiplier = value;
            }
        }
        public string noiseReduction
        {
            get
            {
                return _noiseReduction;
            }
            set
            {
                _noiseReduction = value;
            }
        }

        public string scaleFactor
        {
            get
            {
                return _scaleFactor;
            }
            set
            {
                _scaleFactor = value;
            }
        }

        public string approxPolyparameter
        {
            get
            {
                return _approxPolyparameter;
            }
            set
            {
                _approxPolyparameter = value;
            }
        }

        public string multiEdge
        {
            get
            {
                return _multiEdge;
            }
            set
            {
                _multiEdge = value;
            }
        }
        public string backgroundThreshold
        {
            get
            {
                return _backgroundThreshold;
            }
            set
            {
                _backgroundThreshold = value;
            }
        }
    }
}
