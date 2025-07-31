using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class UsuariosDbContext
    {
        private readonly string _connectionString;

        public UsuariosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<IM253E01Usuario> List()
        {
            var usuarios = new List<IM253E01Usuario>();
            using (var con = new SqlConnection(_connectionString))
            // Revertir a la consulta original SIN Edad ni Foto
            using (var cmd = new SqlCommand("SELECT [Id],[Nombre],[Direccion],[Telefono],[Correo] FROM [IM253E01Usuario]", con))
            {
                con.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuarios.Add(new IM253E01Usuario
                        {
                            Id = (Guid)dr["Id"],
                            Nombre = dr["Nombre"].ToString(),
                            Direccion = dr["Direccion"] == DBNull.Value ? null : dr["Direccion"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Correo = dr["Correo"] == DBNull.Value ? null : dr["Correo"].ToString()
                            // Eliminar el mapeo para Edad y Foto
                            // Edad = dr["Edad"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["Edad"]),
                            // Foto = dr["Foto"] == DBNull.Value ? null : dr["Foto"].ToString()
                        });
                    }
                }
            }
            return usuarios;
        }

        public IM253E01Usuario Details(Guid id)
        {
            var usuario = new IM253E01Usuario();
            using (var con = new SqlConnection(_connectionString))
            // Revertir a la consulta original SIN Edad ni Foto
            using (var cmd = new SqlCommand("SELECT [Id],[Nombre],[Direccion],[Telefono],[Correo] FROM [IM253E01Usuario] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                con.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        usuario.Id = (Guid)dr["Id"];
                        usuario.Nombre = dr["Nombre"].ToString();
                        usuario.Direccion = dr["Direccion"] == DBNull.Value ? null : dr["Direccion"].ToString();
                        usuario.Telefono = dr["Telefono"].ToString();
                        usuario.Correo = dr["Correo"] == DBNull.Value ? null : dr["Correo"].ToString();
                        // Eliminar el mapeo para Edad y Foto
                        // usuario.Edad = dr["Edad"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["Edad"]);
                        // usuario.Foto = dr["Foto"] == DBNull.Value ? null : dr["Foto"].ToString();
                    }
                }
            }
            return usuario;
        }

        public void Create(IM253E01Usuario usuario)
        {
            using (var con = new SqlConnection(_connectionString))
            // Revertir a la consulta original SIN Edad ni Foto
            using (var cmd = new SqlCommand("INSERT INTO [IM253E01Usuario] ([Id],[Nombre],[Direccion],[Telefono],[Correo]) VALUES (@id,@nombre,@direccion,@telefono,@correo)", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                cmd.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = usuario.Nombre;
                cmd.Parameters.Add("@direccion", SqlDbType.NVarChar).Value = (object)usuario.Direccion ?? DBNull.Value;
                cmd.Parameters.Add("@telefono", SqlDbType.NVarChar).Value = usuario.Telefono;
                cmd.Parameters.Add("@correo", SqlDbType.NVarChar).Value = (object)usuario.Correo ?? DBNull.Value;
                // Eliminar los parámetros para Edad y Foto
                // cmd.Parameters.Add("@edad", SqlDbType.Int).Value = (object)usuario.Edad ?? DBNull.Value;
                // cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object)usuario.Foto ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Edit(IM253E01Usuario usuario)
        {
            using (var con = new SqlConnection(_connectionString))
            // Revertir a la consulta original SIN Edad ni Foto
            using (var cmd = new SqlCommand("UPDATE [IM253E01Usuario] SET [Nombre] = @nombre, [Direccion] = @direccion, [Telefono] = @telefono, [Correo] = @correo WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = usuario.Id;
                cmd.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = usuario.Nombre;
                cmd.Parameters.Add("@direccion", SqlDbType.NVarChar).Value = (object)usuario.Direccion ?? DBNull.Value;
                cmd.Parameters.Add("@telefono", SqlDbType.NVarChar).Value = usuario.Telefono;
                cmd.Parameters.Add("@correo", SqlDbType.NVarChar).Value = (object)usuario.Correo ?? DBNull.Value;
                // Eliminar los parámetros para Edad y Foto
                // cmd.Parameters.Add("@edad", SqlDbType.Int).Value = (object)usuario.Edad ?? DBNull.Value;
                // cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object)usuario.Foto ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM [IM253E01Usuario] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}