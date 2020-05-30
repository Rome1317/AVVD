using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using System.Configuration;
using System.Data;

namespace AAVD
{
    class Conexion
    {

        static public Cluster _cluster;
        static public ISession _session;

        static public string _dbServer { set; get; }
        static public string _dbKeySpace { set; get; }

        public void conectar()
        {
            _dbServer = ConfigurationManager.AppSettings["Cluster"].ToString();
            _dbKeySpace = ConfigurationManager.AppSettings["KeySpace"].ToString();
            try
            {

                Console.WriteLine("BD Conectada");
                _cluster = Cluster.Builder()
                    .AddContactPoint(_dbServer)
                    .Build();



                _session = _cluster.Connect(_dbKeySpace);
            }
            catch (Exception ex)
            {


                Console.WriteLine("ERROR AL CONECTAR BD");
            }
        }

        //GONZALEZ SOTO ROMELIA ALEJANDRA
        public void InsertSchool(School data)
        {

            try
            {

                string qry = "INSERT INTO School(Clave,Nombre,RFC,Pais,Ciudad,Fecha_Inaguracion) VALUES(";
                qry = qry + data.Clave.ToString();
                qry = qry + ",'";
                qry = qry + data.Nombre;
                qry = qry + "','";
                qry = qry + data.RFC;
                qry = qry + "','";
                qry = qry + data.Pais;
                qry = qry + "','";
                qry = qry + data.Ciudad;
                qry = qry + "','";
                qry = qry + data.Fecha_Inaguracion;
                qry = qry + "');";

                //string query = "insert into ejemplo(campos,...)  values({0}, '{1}','{2}',{3},'{4}');";
                //qry = string.Format(query, dato.campos,...);

                _session.Execute(qry);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public void UpdateSchool(School data)
        {

            try
            {
                string qry = "UPDATE School SET  Nombre ='" + data.Nombre + "', RFC = '" + data.RFC + "', Pais = '" + data.Pais + "', Ciudad = '" + data.Ciudad + "', Fecha_Inaguracion = '" + data.Fecha_Inaguracion + "' WHERE Clave = " + data.Clave + ";";

                _session.Execute(qry);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public void InsertSchoolDepartment(School data)
        {

            try
            {

                string qry = "UPDATE School SET  Departamentos = Departamentos + {'" + data.Departamentos[0] + "'} WHERE Clave = " + data.Clave + ";";
                _session.Execute(qry);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public bool DeleteSchoolDepartment(School data, string text)
        {
            bool found = false;
 
            int i = 0;

           while(i < data.Departamentos.Count)
            {
                if (data.Departamentos[i] == text)
                {
                    found = true;
                    break;
                }
                i++;
            }

            if (found == true)
            {
                try
                {

                    string qry = "UPDATE School SET  Departamentos = Departamentos - {'" + data.Departamentos[i] + "'} WHERE Clave = " + data.Clave + ";";
                    _session.Execute(qry);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error");
                    throw e;
                }
                finally
                {
                    // desconectar o cerrar la conexion
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void DeleteSchool(School data)
        {

            try
            {


                string qry = "DELETE FROM School WHERE Clave = ";
                qry = qry + data.Clave;
                qry = qry + ";";


                _session.Execute(qry);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public School ShowSchool(School data)
        {

            School ReturnData = new School();

            try
            {
                //NO PONER MAYUSCULAS 

                string qry = "SELECT clave,nombre,rfc,pais,ciudad,fecha_inaguracion,departamentos FROM School WHERE clave = " + data.Clave + ";";
                var sesion = _cluster.Connect(_dbKeySpace);
                var rs = sesion.Execute(qry);

                foreach (var row in rs)
                {
                    ReturnData.Clave = row.GetValue<int>("clave");
                    ReturnData.Nombre = row.GetValue<string>("nombre");
                    ReturnData.RFC = row.GetValue<string>("rfc");
                    ReturnData.Pais = row.GetValue<string>("pais");
                    ReturnData.Ciudad = row.GetValue<string>("ciudad");
                    ReturnData.Date = row.GetValue<Cassandra.LocalDate>("fecha_inaguracion");
                    ReturnData.Departamentos = row.GetValue<List<string>>("departamentos");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }

            return ReturnData;
        }

        public List<School> GetAll()
        {

            List<School> list = new List<School>();

            try
            {
                string query = "select clave,nombre,rfc,pais,ciudad,fecha_inaguracion,departamentos from School;";
                var sesion = _cluster.Connect(_dbKeySpace);
                var rs = sesion.Execute(query);

                foreach (var row in rs)
                {


                    School ReturnData = new School();

                    ReturnData.Clave = row.GetValue<int>("clave");
                    ReturnData.Nombre = row.GetValue<string>("nombre");
                    ReturnData.RFC = row.GetValue<string>("rfc");
                    ReturnData.Pais = row.GetValue<string>("pais");
                    ReturnData.Ciudad = row.GetValue<string>("ciudad");
                    ReturnData.Date = row.GetValue<Cassandra.LocalDate>("fecha_inaguracion");
                    ReturnData.Departamentos = row.GetValue<List<string>>("departamentos");

                    list.Add(ReturnData); 

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }

            return list;
        }

        public void SchoolList(List<School> temp, DataTable view)
        {
            foreach (var Row in temp)
            {

                DataRow row = view.NewRow();

                row["Clave"] = Row.Clave;
                row["Nombre"] = Row.Nombre;
                row["RFC"] = Row.RFC;

                if (Row.Departamentos != null)
                {
                    int d = 1;
                    for (int i = 0; i < Row.Departamentos.Count; i++)
                    {
                        string depts = "Depto. ";
                        depts += d;
                        d++;
                        row[depts] = Row.Departamentos[i];
                    }
                }
                row["Fecha_Inaguracion"] = Row.Date;
                row["Pais"] = Row.Pais;
                row["Ciudad"] = Row.Ciudad;

                view.Rows.Add(row);
            }
        }

        public void DeleteSchoolCoulumn (School data, string text)
        {
            try
            {
                var delete = _session.Prepare("delete " + text + " from School where clave = ?");
                var batch = new BatchStatement();
                batch.Add(delete.Bind(data.Clave));
                // Console.WriteLine(batch);
                _session.Execute(batch);

            }
            catch (Exception e)
            {
                Console.WriteLine("error");

                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexión
            }
        }

        //CALVILLO LUMBRERAS EDGAR DONNATO

        public void InsertProfessor(Professor data)
        {

            try
            {


                string qry = "insert into Professor(curp,nombre,apellidop,apellidom,sexo,fecha_nacimiento) values(";
                qry = qry + "'";
                qry = qry + data.CURP;
                qry = qry + "','";
                qry = qry + data.Nombre;
                qry = qry + "','";
                qry = qry + data.ApellidoP;
                qry = qry + "','";
                qry = qry + data.ApellidoM;
                qry = qry + "','";
                qry = qry + data.Sexo;
                qry = qry + "','";
                qry = qry + data.Fecha_Nacimiento;
                qry = qry + "');";


                //string query = "insert into ejemplo(campos,...)  values({0}, '{1}','{2}',{3},'{4}');";
                //qry = string.Format(query, dato.campos,...);

                _session.Execute(qry);
                Console.WriteLine("Profesor creado con exito");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al insertar");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public void UpdateProfessor(Professor data)
        {

            try
            {

                string qry = "update Professor set  Nombre ='" + data.Nombre + "', ApellidoP = '" + data.ApellidoP + "', ApellidoM = '" + data.ApellidoM + "',Sexo = '" + data.Sexo + "', Fecha_Nacimiento = '" + data.Fecha_Nacimiento + "' WHERE CURP = '" + data.CURP + "';";


                _session.Execute(qry);
                Console.WriteLine("Profesor actualizado con exito");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al actualizar");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public void DeleteProfessor(Professor data)
        {

            try
            {


                string qry = "delete from Professor where CURP = ";
                qry = qry + "'";
                qry = qry + data.CURP;
                qry = qry + "';";


                _session.Execute(qry);
                Console.WriteLine("Profesor eliminado con exito");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al eliminar");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public void InsertEmail(Professor data)
        {

            try
            {

                string qry = "UPDATE Professor SET  Email = Email + {'" + data.Emails[0] + "'} WHERE curp = '" + data.CURP + "';";
                _session.Execute(qry);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexion
            }

        }

        public bool DeleteEmail(Professor data, string text)
        {
            bool found = false;

            int i = 0;

            while (i < data.Emails.Count)
            {
                if (data.Emails[i] == text)
                {
                    found = true;
                    break;
                }
                i++;
            }

            if (found == true)
            {

                try
                {

                    string qry = "UPDATE Professor SET  Email = Email - {'" + data.Emails[i] + "'} WHERE CURP = '" + data.CURP + "';";
                    _session.Execute(qry);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error");
                    throw e;
                }
                finally
                {
                    // desconectar o cerrar la conexion
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public Professor ShowProfessor(Professor data)
        {

            Professor ReturnData = new Professor();

            try
            {
              
                string qry = "SELECT curp,nombre,apellidop,apellidom,sexo,fecha_nacimiento,email FROM Professor WHERE curp = '" + data.CURP + "';";
                var sesion = _cluster.Connect(_dbKeySpace);
                var rs = sesion.Execute(qry);

                foreach (var row in rs)
                {
                    ReturnData.CURP = row.GetValue<string>("curp");
                    ReturnData.Nombre = row.GetValue<string>("nombre");
                    ReturnData.ApellidoP = row.GetValue<string>("apellidop");
                    ReturnData.ApellidoM = row.GetValue<string>("apellidom");
                    ReturnData.Sexo = row.GetValue<string>("sexo");
                    ReturnData.Fecha = row.GetValue<Cassandra.LocalDate>("fecha_nacimiento");
                    ReturnData.Emails = row.GetValue<List<string>>("email");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }

            return ReturnData;
        }

        public List<Professor> GetProfessors()
        {

            List<Professor> list = new List<Professor>();

            try
            {
                string query = "select  curp,nombre,apellidop,apellidom,sexo,fecha_nacimiento,email from Professor;";
                var sesion = _cluster.Connect(_dbKeySpace);
                var rs = sesion.Execute(query);

                foreach (var row in rs)
                {


                    Professor ReturnData = new Professor();

                    ReturnData.CURP = row.GetValue<string>("curp");
                    ReturnData.Nombre = row.GetValue<string>("nombre");
                    ReturnData.ApellidoP = row.GetValue<string>("apellidop");
                    ReturnData.ApellidoM = row.GetValue<string>("apellidom");
                    ReturnData.Sexo = row.GetValue<string>("sexo");
                    ReturnData.Fecha = row.GetValue<Cassandra.LocalDate>("fecha_nacimiento");
                    ReturnData.Emails = row.GetValue<List<string>>("email");

                    list.Add(ReturnData);

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                throw e;
            }

            return list;
        }

        public void ShowALL(List<Professor> temp, DataTable view)
        {
           
            foreach (var Row in temp)
            {

                DataRow row = view.NewRow();

                row["CURP"] = Row.CURP;
                row["Nombre"] = Row.Nombre;
                row["Apellido Paterno"] = Row.ApellidoP;
                row["Apellido Materno"] = Row.ApellidoM;
                row["Sexo"] = Row.Sexo;
                row["Fecha Nacimiento"] = Row.Fecha;
                if (Row.Emails != null)
                {
                    int e = 1;
                    for (int i = 0; i < Row.Emails.Count; i++)
                    {
                        string email = "Email ";
                        email += e;
                        e++;
                        row[email] = Row.Emails[i];
                    }
                }

                view.Rows.Add(row);
            }
        }

        public void DeleteProfessorCoulumn(Professor data, string text)
        {

            try
            {
                var delete = _session.Prepare("delete " + text + " from Professor where curp = ?");
                var batch = new BatchStatement();
                batch.Add(delete.Bind(data.CURP));
                // Console.WriteLine(batch);
                _session.Execute(batch);

            }
            catch (Exception e)
            {
                Console.WriteLine("error");

                throw e;
            }
            finally
            {
                // desconectar o cerrar la conexión
            }

        }

    }
    
}
