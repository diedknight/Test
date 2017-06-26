using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMe
{
    public class AttributeParameter : IComparable<AttributeParameter>
    {
        string attributeName;
        string attributeValue;
        int listOrder;

        public AttributeParameter()
        {

        }

        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }

        public int ListOrder
        {
            get { return listOrder; }
            set { listOrder = value; }
        }

        #region IComparable<AttributeParameter> 成员

        public int CompareTo(AttributeParameter other)
        {
            return this.listOrder.CompareTo(other.listOrder);
        }

        #endregion
    }

    public class AttributeParameterCollection : List<AttributeParameter>
    {
        public string ToTitleString()
        {
            this.Sort();
            string h1 = "";
            if (this.Count > 2)
            {
                h1 = " with";
                for (int i = 0; i < this.Count - 1; i++)
                {
                    h1 += " " + this[i].AttributeName.Trim() + " " + this[i].AttributeValue.Trim() + ",";
                }
                h1 = h1.TrimEnd(',');
                h1 += " and " + this[this.Count - 1].AttributeName.Trim() + " " + this[this.Count - 1].AttributeValue.Trim();
                return h1;
            }
            else if (this.Count == 2)
            {
                h1 = " with";
                h1 += " " + this[0].AttributeName.Trim() + " " + this[0].AttributeValue.Trim() + " and";
                h1 += " " + this[1].AttributeName.Trim() + " " + this[1].AttributeValue.Trim();
            }
            else if (this.Count == 1)
            {
                h1 = " with";
                h1 += " " + this[0].AttributeName.Trim() + " " + this[0].AttributeValue.Trim();
            }

            return h1;
        }

        public string ToH1String()
        {
            return ToTitleString();
        }

        public string ToURLString()
        {
            this.Sort();
            if (this.Count > 0)
            {
                string urlString = "-";
                foreach (AttributeParameter ap in this)
                {
                    urlString += "-" + ap.AttributeName.Trim().Replace(" ", "-") + "-" + ap.AttributeValue.Trim().Replace(" ", "-");
                }
                urlString = UrlController.FilterInvalidUrlPathChar(urlString.Replace("/", "-"));
                return urlString;
            }
            return "";
        }

        public string ToDescription(PriceMeCache.CategoryCache category)
        {
            this.Sort();
            string desString = "";
            if (this.Count > 2)
            {
                desString = "Find External " + category.CategoryName + " with ";
                for (int i = 0; i < this.Count - 1; i++)
                {
                    desString += " " + this[i].AttributeName.Trim() + " " + this[i].AttributeValue.Trim() + ",";
                }
                desString = desString.TrimEnd(',');
                desString += " and " + this[this.Count - 1].AttributeName.Trim() + " " + this[this.Count - 1].AttributeValue.Trim();
                desString += "on PriceMe.";
            }
            else if (this.Count == 2)
            {
                desString = "Find External " + category.CategoryName + " with ";
                desString += " " + this[0].AttributeName.Trim() + " " + this[0].AttributeValue.Trim() + " and";
                desString += " " + this[1].AttributeName.Trim() + " " + this[1].AttributeValue.Trim();
                desString += " on PriceMe.";
            }
            else if (this.Count == 1)
            {
                desString = "Find External " + category.CategoryName + " with ";
                desString += " " + this[0].AttributeName.Trim() + " " + this[0].AttributeValue.Trim();
                desString += " on PriceMe.";
            }

            return desString;
        }
    }
}