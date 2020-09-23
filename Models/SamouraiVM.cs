using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samsam.Models
{
    public class SamouraiVM
    {
        public Samourai  Samourai { get; set; }
        public List<Arme> Armes { get; set; } = new List<Arme>();
        public int? IdArme { get; set; }
        public List<ArtMartial> ArtMartials { get; set; } = new List<ArtMartial>();
        public List<int> IdArtMartials { get; set; } = new List<int>();
    }
}