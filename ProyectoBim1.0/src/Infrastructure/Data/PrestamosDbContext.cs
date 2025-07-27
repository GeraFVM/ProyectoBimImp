using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class PrestamosDbContext
    {
        private readonly string _connectionString;

        public PrestamosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<IM253E01Prestamo> List()
        {
            var prestamos = new List<IM253E01Prestamo>();
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT [Id],[UsuarioId],[LibroId],[FechaPrestamo],[FechaDevolucion] FROM [IM253E01Prestamo]", con))
            {
                con.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        prestamos.Add(new IM253E01Prestamo
                        {
                            Id = (Guid)dr["Id"],
                            UsuarioId = (Guid)dr["UsuarioId"],
                            LibroId = (Guid)dr["LibroId"],
                            FechaPrestamo = (DateTime)dr["FechaPrestamo"],
                            FechaDevolucion = dr["FechaDevolucion"] as DateTime?
                        });
                    }
                }
            }
            return prestamos;
        }

        public IM253E01Prestamo Details(Guid id)
        {
            var prestamo = new IM253E01Prestamo();
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT [Id],[UsuarioId],[LibroId],[FechaPrestamo],[FechaDevolucion] FROM [IM253E01Prestamo] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                con.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        prestamo.Id = (Guid)dr["Id"];
                        prestamo.UsuarioId = (Guid)dr["UsuarioId"];
                        prestamo.LibroId = (Guid)dr["LibroId"];
                        prestamo.FechaPrestamo = (DateTime)dr["FechaPrestamo"];
                        prestamo.FechaDevolucion = dr["FechaDevolucion"] as DateTime?;
                    }
                }
            }
            return prestamo;
        }

        public void Create(IM253E01Prestamo prestamo)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("INSERT INTO [IM253E01Prestamo] ([Id],[UsuarioId],[LibroId],[FechaPrestamo],[FechaDevolucion]) VALUES (@id,@usuarioId,@libroId,@fechaPrestamo,@fechaDevolucion)", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                cmd.Parameters.Add("@usuarioId", SqlDbType.UniqueIdentifier).Value = prestamo.UsuarioId;
                cmd.Parameters.Add("@libroId", SqlDbType.UniqueIdentifier).Value = prestamo.LibroId;
                cmd.Parameters.Add("@fechaPrestamo", SqlDbType.DateTime2).Value = prestamo.FechaPrestamo;
                cmd.Parameters.Add("@fechaDevolucion", SqlDbType.DateTime2).Value = (object)prestamo.FechaDevolucion ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Edit(IM253E01Prestamo prestamo)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE [IM253E01Prestamo] SET [UsuarioId] = @usuarioId, [LibroId] = @libroId, [FechaPrestamo] = @fechaPrestamo, [FechaDevolucion] = @fechaDevolucion WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = prestamo.Id;
                cmd.Parameters.Add("@usuarioId", SqlDbType.UniqueIdentifier).Value = prestamo.UsuarioId;
                cmd.Parameters.Add("@libroId", SqlDbType.UniqueIdentifier).Value = prestamo.LibroId;
                cmd.Parameters.Add("@fechaPrestamo", SqlDbType.DateTime2).Value = prestamo.FechaPrestamo;
                cmd.Parameters.Add("@fechaDevolucion", SqlDbType.DateTime2).Value = (object)prestamo.FechaDevolucion ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM [IM253E01Prestamo] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
