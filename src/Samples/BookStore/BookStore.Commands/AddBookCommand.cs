using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;

using Taro;

namespace BookStore.Commands
{
    public class AddBookCommand
    {
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string CreatorId { get; set; }

        public AddBookCommand()
        {
        }
    }
}
