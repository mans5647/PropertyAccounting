using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAccounting
{
    class Property
    {
        public int ID { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public DateTime PropertyPurchaseDate { get; set; }
        public decimal PropertyCost { get; set; }
        public int DepartmentID { get; set; }
        public string TechFeatures { get; set; }
        public string ImagePath { get; set; }
        public string CertificatePath { get; set; }
    }
}
