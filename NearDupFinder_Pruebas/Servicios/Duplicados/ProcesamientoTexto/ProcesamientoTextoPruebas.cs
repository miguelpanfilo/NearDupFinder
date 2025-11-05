

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
        
        [TestMethod]
        public void AplicarStopwordsYStemming_TextoComun_DeberiaReducirYEliminarStopwords()
        {
            string[] entrada = { "Los", "jugadores", "estaban", "jugando", "en", "el", "sistema", "nuevo" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.IsTrue(salida.Any(t => t.StartsWith("jug")), "Debería reducir 'jugadores' y 'jugando' a su raíz.");
            Assert.IsTrue(salida.Any(t => t.StartsWith("sistem")), "Debería reducir 'sistema' a su raíz.");
            Assert.IsFalse(salida.Contains("los"), "Debería eliminar la stopword 'los'.");
            Assert.IsFalse(salida.Contains("en"), "Debería eliminar la stopword 'en'.");
        }




    }
}