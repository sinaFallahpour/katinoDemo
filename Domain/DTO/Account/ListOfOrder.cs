using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfOrder
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public decimal PriceWithTax { get; set; }
        public string OrderType { get; set; }



        public string RefrenceUserName { get; set; }
        public decimal RefrenceAmount { get; set; }


    }
    public class OrderDetails : ListOfOrder
    {
        public decimal Discount { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public string PayTo { get; set; }
        public string Issuccess { get; set; }
    }
    public class OrderListForAdmin:ListOfOrder
    {
        public string  CompanyName { get; set; }
        public string Issuccess { get; set; }

    }
}
