using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class OrderListDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string PlanName { get; set; }
        public double Price { get; set; }
        public double PriceWithTax { get; set; }
        public double FinalPrice { get; set; }
        public string PaymnetType { get; set; }
    }
}
