
using Microsoft.EntityFrameworkCore;
using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Pruebas.Repositorios
{
    [TestClass]
    public class RepositorioUsuariosPruebas
    {
        private DbContextOptions<SqlContext> CrearOpciones(string nombreDb) =>
            new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(databaseName: nombreDb)
                .Options;

        [TestMethod]
        public void ObtenerIdMaximo_ConUsuarios_RetornaIdMaximo()
        {
            var options = CrearOpciones("RepoUsuarios_Db1");

            Usuario u1;
            Usuario u2;
            Usuario u3;

            using (var context = new SqlContext(options))
            {
                u1 = Usuario.Crear(
                    "Juan",
                    "Pérez",
                    new Email("juan@test.com"),
                    new Fecha(1990, 1, 1)
                );

                u2 = Usuario.Crear(
                    "Ana",
                    "López",
                    new Email("ana@test.com"),
                    new Fecha(1992, 5, 10)
                );

                u3 = Usuario.Crear(
                    "Luis",
                    "García",
                    new Email("luis@test.com"),
                    new Fecha(1988, 3, 20)
                );

                context.Usuarios.AddRange(u1, u2, u3);
                context.SaveChanges();
            }

            using (var context = new SqlContext(options))
            {
                var repo = new RepositorioUsuarios(context);

                int resultado = repo.ObtenerIdMaximo();

                int idEsperado = new[] { u1.Id, u2.Id, u3.Id }.Max();

                Assert.AreEqual(idEsperado, resultado);
            }
        }

        [TestMethod]
        public void ObtenerIdMaximo_SinUsuarios_RetornaCero()
        {
            var options = CrearOpciones("RepoUsuarios_Db2");

            using var context = new SqlContext(options);
            var repo = new RepositorioUsuarios(context);

            int resultado = repo.ObtenerIdMaximo();

            Assert.AreEqual(0, resultado);
        }
    }
}
