using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAVD
{
    public class School
    {

        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string RFC { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Fecha_Inaguracion { get; set; } //DATE
        public Cassandra.LocalDate Date { get; set; }
        public List<string> Departamentos { get; set; } //SET


    }
}

