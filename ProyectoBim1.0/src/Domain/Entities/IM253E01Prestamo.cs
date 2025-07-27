/*CREATE TABLE [IM253E01Prestamos] (
    [Id] [uniqueidentifier] NOT NULL,
    [UsuarioId] [uniqueidentifier] NOT NULL,
    [LibroId] [uniqueidentifier] NOT NULL,
    [FechaPrestamo] [smalldatetime] NOT NULL,
    [FechaDevolucion] [smalldatetime] NULL,

    CONSTRAINT PK_IM253E01Prestamos PRIMARY KEY ([Id]),
    CONSTRAINT FK_IM253E01Prestamos_IM253E01Usuario FOREIGN KEY ([UsuarioId]) REFERENCES [IM253E01Usuario] ([Id]),
    CONSTRAINT FK_IM253E01Prestamos_IM253E01Libro FOREIGN KEY ([LibroId]) REFERENCES [IM253E01Libro] ([Id])
);
*/

namespace Domain.Entities
{
    public class IM253E01Prestamo
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; } // Asegúrate de que esta propiedad exista
        public DateTime? FechaDevolucion { get; set; } // Nullable
        public IM253E01Usuario? Usuario { get; set; }
        public IM253E01Libro? Libro { get; set; }
    }
}

