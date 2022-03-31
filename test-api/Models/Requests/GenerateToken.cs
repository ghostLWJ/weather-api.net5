using System;
using System.ComponentModel.DataAnnotations;

namespace test_api.Models.Requests
{
    public class GenerateToken
    {
        [Required]
        public string IP;
    }
}
