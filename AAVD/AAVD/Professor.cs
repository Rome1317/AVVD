using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAVD
{
    class Professor
    {
        public string CURP { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string  Sexo { get; set; }
        public string Fecha_Nacimiento { get; set; }
        public List<string> Emails { get; set; }
        public Cassandra.LocalDate Fecha { get; set; } 

    }
}
