/*
CREATE TABLE [IM253E01Usuario] (
    [Id] [uniqueidentifier] NOT NULL,
    [Nombre] [nvarchar](256) NOT NULL,
    [Direccion] [nvarchar] NULL,
    [Telefono] [nvarchar] NOT NULL,
    [Correo] [nvarchar] NOT NULL,

    CONSTRAINT PK_IM253E01Usuario PRIMARY KEY ([Id])
);
*/

namespace Domain.Entities
{
    public class IM253E01Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        // Elimina estas líneas:
        // public string? Foto { get; set; }
        // public int? Edad { get; set; }
    }
}
