namespace NearDupFinder_Dominio.Clases
{
    public class ItemEditDto
    {
        public int Id { get; set; } // aquí podés setear el Id aunque Item solo tenga get
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
    }
}