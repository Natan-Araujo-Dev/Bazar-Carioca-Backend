using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    [Owned]
    public class Address
    {
        public string Neighborhood { get; set; }

        public string Street { get; set; }

        public int Number {  get; set; }
    }
}
