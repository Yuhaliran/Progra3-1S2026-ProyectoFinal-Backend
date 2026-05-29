namespace ProyectoFinal.Progra3.Backend.Modelos
{
    public class ColaLectura
    {
        public int IdColaLectura { get; set; }
        public int IdUsuario { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int IdEstadoLectura { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? MeGusto { get; set; }
    }
}
