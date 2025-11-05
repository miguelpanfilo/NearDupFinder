

using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

namespace NearDupFinder_Pruebas.Servicios.Duplicados.ProcesamientoTexto
{
    [TestClass]
    public class ProcesamientoTextoPruebas
    {
        private ProcesadorTexto _procesador = null!;

        [TestInitialize]
        public void Setup()
        {
            _procesador = new ProcesadorTexto();
        }

        [TestMethod]
        public void AplicarStopwords_TextoConStopwords_DeberiaEliminarSoloLasStopwords()
        {
            string[] entrada = { "el", "gato", "de", "la", "casa" };

            string[] salida = _procesador.AplicarStopwords(entrada);

            CollectionAssert.AreEquivalent(
                new[] { "gato", "casa" },
                salida
            );
        }
        [TestMethod]
        public void AplicarStopwords_TextoConStopwordsJuntas_NoDeberiaEliminarSoloLasStopwords()
        {
            string[] entrada = { "elelelelel", "gato", "dede", "la", "casa" };

            string[] salida = _procesador.AplicarStopwords(entrada);

            CollectionAssert.AreEquivalent(
                new[] { "elelelelel", "gato", "dede", "casa" },
                salida
            );
        }
        [TestMethod]
        public void AplicarStemming_PalabrasDerivadasRepetidasRaiz_DeberiaReducirUnaRaiz()
        {
            string[] entrada = { "jugando", "jugando", "jugando" };

            string[] salida = _procesador.AplicarStemming(entrada);

            CollectionAssert.AreEquivalent(
                new[] { "jug" },
                salida
            );
        }


    }
}