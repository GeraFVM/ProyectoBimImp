using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class LibrosDbContext
    {
        private readonly string _connectionString;

        public LibrosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<IM253E01Libro> List()
        {
            var data = new List<IM253E01Libro>();

            // Consulta SELECT solo con las columnas existentes en la DB
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM [IM253E01Libro]", con))
            {
                try
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new IM253E01Libro
                            {
                                Id = (Guid)dr["Id"],
                                Autor = dr["Autor"]?.ToString(),     // Lectura de Autor (puede ser null)
                                Editorial = dr["Editorial"]?.ToString(), // Lectura de Editorial (puede ser null)
                                ISBN = dr["ISBN"]?.ToString(),       // Lectura de ISBN (puede ser null)
                                Foto = dr["Foto"]?.ToString()        // Lectura de Foto (puede ser null)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar libros", ex);
                }
            }
            return data;
        }

        public IM253E01Libro Details(Guid id)
        {
            var libro = new IM253E01Libro();

            // Consulta SELECT solo con las columnas existentes en la DB
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM [IM253E01Libro] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;

                try
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            libro.Id = (Guid)dr["Id"];
                            libro.Autor = dr["Autor"]?.ToString();
                            libro.Editorial = dr["Editorial"]?.ToString();
                            libro.ISBN = dr["ISBN"]?.ToString();
                            libro.Foto = dr["Foto"]?.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener detalles del libro con ID: {id}", ex);
                }
            }
            return libro;
        }

        public void Create(IM253E01Libro libro)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                // Consulta INSERT solo con las columnas existentes en la DB
                "INSERT INTO [IM253E01Libro] ([Id],[Autor],[Editorial],[ISBN],[Foto]) " +
                "VALUES (@id,@autor,@editorial,@isbn,@foto)", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = libro.Id;
                // Usamos DBNull.Value para campos nulos
                cmd.Parameters.Add("@autor", SqlDbType.NVarChar).Value = (object)libro.Autor ?? DBNull.Value;
                cmd.Parameters.Add("@editorial", SqlDbType.NVarChar).Value = (object)libro.Editorial ?? DBNull.Value;
                cmd.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = (object)libro.ISBN ?? DBNull.Value;
                cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object)libro.Foto ?? DBNull.Value;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear libro", ex);
                }
            }
        }

        public void Update(IM253E01Libro libro)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                // Consulta UPDATE solo con las columnas existentes en la DB
                "UPDATE [IM253E01Libro] SET [Autor] = @autor, [Editorial] = @editorial, " +
                "[ISBN] = @isbn, [Foto] = @foto WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = libro.Id;
                // Usamos DBNull.Value para campos nulos
                cmd.Parameters.Add("@autor", SqlDbType.NVarChar).Value = (object)libro.Autor ?? DBNull.Value;
                cmd.Parameters.Add("@editorial", SqlDbType.NVarChar).Value = (object)libro.Editorial ?? DBNull.Value;
                cmd.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = (object)libro.ISBN ?? DBNull.Value;
                cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object)libro.Foto ?? DBNull.Value;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al actualizar libro con ID: {libro.Id}", ex);
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM [IM253E01Libro] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al eliminar libro con ID: {id}", ex);
                }
            }
        }
    }
}