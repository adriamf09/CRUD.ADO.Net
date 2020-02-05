using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FormADO
{
    public class ConsultasSQL
    {
        public List<Persona> PersonasSelectAll()
        {
            var connection = ConnectionFactory.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from vwPersonas where IsDeleted = 0";

            connection.Open();

            var dataReader = command.ExecuteReader();
            List<Persona> lista = new List<Persona>();

            while (dataReader.Read())
            {
                Persona person = new Persona();
                person.IdPersona = (int)dataReader["IdPersona"];
                person.Nombre = (string)dataReader["Nombre"];
                person.FechaNacimiento = (DateTime)dataReader["FechaNacimiento"];
                person.Pais = (string)dataReader["Pais"];
                person.Sexo = (string)dataReader["Sexo"];
                person.Telefono = (string)dataReader["Telefono"];
                person.Email = (string)dataReader["Email"];
                person.Foto = (byte[])dataReader["Foto"];
                lista.Add(person);
            }
            dataReader.Close();
            connection.Close();
            return lista;
        }

        public List<Pais> PaisSelectAll()
        {
            var connection = ConnectionFactory.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Paises";
            
            connection.Open();

            var dataReader = command.ExecuteReader();
            List<Pais> lista = new List<Pais>();
            while (dataReader.Read())
            {
                Pais pais = new Pais();
                pais.IdPais = (int)dataReader["IdPais"];
                pais.Nombre = (string)dataReader["Nombre"];

                lista.Add(pais);
            }
            dataReader.Close();
            connection.Close();
            return lista;
        }

        public bool Insertar(string nombre, DateTime fdn, int idPais, string sexo, string telefono, string email, byte[] foto)
        {
            var connection = ConnectionFactory.GetConnection();
            var command = connection.CreateCommand();

            command.CommandText = "insert into personas values(@nombre, @fdn, @idPais, @sexo, @telefono, @email, @foto)";
            command.Parameters.AddWithValue("@nombre", nombre);
            command.Parameters.AddWithValue("@fdn", fdn);
            command.Parameters.AddWithValue("@idPais", idPais);
            command.Parameters.AddWithValue("@sexo", sexo);
            command.Parameters.AddWithValue("@telefono", telefono);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@foto", foto);

            connection.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            connection.Close();
            if (filasAfectadas > 0) return true;
            else return false;
        }

        public bool Eliminar(int id)
        {
            var connection = ConnectionFactory.GetConnection();
            var command = connection.CreateCommand();

            command.CommandText = "update Personas set IsDeleted = 1 where IdPersona =@id";
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            connection.Close();
            if (filasAfectadas > 0) return true;
            else return false;
        }

        public bool Actualizar(int id, string nombre, DateTime fdn, int idPais, string sexo, string telefono, string email, byte [] foto)
        {
            var connection = ConnectionFactory.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "update personas set Nombre = @nombre, FechaNacimiento = @fdn, IdPais= @idPais, " +
                "Sexo= @sexo, Telefono= @telefono, Email= @email, Foto= @foto where IdPersona = @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nombre", nombre);
            command.Parameters.AddWithValue("@fdn", fdn);
            command.Parameters.AddWithValue("@idPais", idPais);
            command.Parameters.AddWithValue("@sexo", sexo);
            command.Parameters.AddWithValue("@telefono", telefono);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@foto", foto);

            connection.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            connection.Close();
            if (filasAfectadas > 0) return true;
            else return false;
        }

        //byte[] data = File.ReadAllBytes("");
    }
}
