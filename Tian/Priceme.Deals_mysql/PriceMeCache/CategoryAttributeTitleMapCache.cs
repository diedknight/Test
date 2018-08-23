using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class CategoryAttributeTitleMapCache
    {
        private int _MapID;

        public int MapID
        {
            get { return _MapID; }
            set { _MapID = value; }
        }
        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }
        private int _AttributeTitleID;

        public int AttributeTitleID
        {
            get { return _AttributeTitleID; }
            set { _AttributeTitleID = value; }
        }
        private bool _IsPrimary;

        public bool IsPrimary
        {
            get { return _IsPrimary; }
            set { _IsPrimary = value; }
        }
        private int _AttributeOrder;

        public int AttributeOrder
        {
            get { return _AttributeOrder; }
            set { _AttributeOrder = value; }
        }

        private bool _IsSlider;
        public bool IsSlider
        {
            get { return _IsSlider; }
            set { _IsSlider = value; }
        }

        private float _Step;
        public float Step
        {
            get { return _Step; }
            set { _Step = value; }
        }

        private float _Step2;
        public float Step2
        {
            get { return _Step2; }
            set { _Step2 = value; }
        }

        public int Step3
        {
            get;
            set;
        }

        private float _MinValue;
        public float MinValue
        {
            get { return _MinValue; }
            set { _MinValue = value; }
        }

        private float _MaxValue;
        public float MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; }
        }
    }
}