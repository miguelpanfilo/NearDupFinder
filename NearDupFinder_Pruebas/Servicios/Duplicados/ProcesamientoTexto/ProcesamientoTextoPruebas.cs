

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
            string[] esperado = { "jugador", "jug", "estab", "sistem", "nuev" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            CollectionAssert.AreEquivalent(
                esperado,
                salida,
                "Los tokens resultantes no coinciden con los esperados después de aplicar stopwords y stemming."
            );
        }

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConSoloStopwords_DeberiaDevolverVacio()
        {
            string[] entrada = { "el", "la", "los", "de", "en", "y" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(0, salida.Length, "No debería devolver tokens si todos son stopwords.");
        }

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoVacio_DeberiaDevolverArrayVacio()
        {
            string[] entrada = Array.Empty<string>();

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(0, salida.Length, "Debe devolver array vacío cuando la entrada está vacía.");
        }
        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConRepetidos_DeberiaEliminarDuplicados()
        {
            string[] entrada = { "jugar", "jugando", "jugador", "jugar" };

            
            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            var distintos = salida.Distinct().ToArray();
            Assert.AreEqual(distintos.Length, salida.Length, "No debería haber tokens repetidos tras aplicar el stemming.");
        }
        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConAcentos_DeberiaReducirCorrectamente()
        {
            string[] entrada = { "canciones", "rápidas", "jugando" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(3, salida.Length, 
                $"Se esperaban exactamente 3 tokens");

            CollectionAssert.AreEquivalent(
                new[] { "cancion", "rap", "jug" },
                salida
            );
        }
        [TestMethod]
        public void AplicarStopwordsYStemming_FraseCoherente_DeberiaReducirYEliminarStopwordsCorrectamente()
        {
            string[] entrada = {
                "Los", "niños", "rápidos", "jugaban", "en", "el", "parque", "con", "sus", "amigos"
            };

            string[] esperado = { "niñ", "rap", "jug", "parqu", "con", "sus", "amig" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            CollectionAssert.AreEqual(
                esperado,
                salida
            );
        }








    }
}