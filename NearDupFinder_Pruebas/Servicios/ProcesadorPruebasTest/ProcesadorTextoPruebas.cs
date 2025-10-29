 using Lucene.Net.Analysis.Snowball;
 using Lucene.Net.Util;
 using NearDupFinder_LogicaDeNegocio.Servicios.ProcesadorDeTexto;

 [TestClass]
    public class ProcesadorTextoPruebas
    {
        private ProcesadorTexto _procesador = null!;

        [TestInitialize]
        public void Setup()
        {
            var analyzer = new SnowballAnalyzer(LuceneVersion.LUCENE_48, "Spanish");
            _procesador = new ProcesadorTexto(analyzer);
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

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConSoloStopwords_DeberiaDevolverVacio()
        {
            string[] entrada = { "el", "la", "los", "de", "en", "y" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(0, salida.Length, "No debería devolver tokens si todo son stopwords.");
        }

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoVacio_DeberiaDevolverArrayVacio()
        {
            string[] entrada = System.Array.Empty<string>();

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(0, salida.Length, "Debe devolver array vacío cuando la entrada está vacía.");
        }

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConAcentos_DeberiaReducirCorrectamente()
        {
            string[] entrada = { "canciones", "rápidas", "jugando" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            Assert.AreEqual(3, salida.Length, "Debe haber exactamente tres tokens resultantes.");
            Assert.AreEqual("cancion", salida[0], "Debería reducir 'canciones' a 'cancion'.");
            Assert.AreEqual("rap", salida[1], "Debería reducir 'rápidas' a 'rap'.");
            Assert.AreEqual("jug", salida[2], "Debería reducir 'jugando' a 'jug'.");
        }


        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConRepetidos_DeberiaEliminarDuplicados()
        {
            string[] entrada = { "jugar", "jugando", "jugador", "jugar" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            var distintos = salida.Distinct().ToArray();
            Assert.AreEqual(distintos.Length, salida.Length, "No debería haber tokens repetidos tras el stemming.");
        }

        [TestMethod]
        public void AplicarStopwordsYStemming_TextoConMayusculas_DeberiaPasarAMinusculas()
        {
            string[] entrada = { "Sistema", "Producto", "USUARIO" };

            string[] salida = _procesador.AplicarStopwordsYStemming(entrada);

            foreach (var token in salida)
                Assert.IsTrue(token.All(char.IsLower), $"El token '{token}' debe estar en minúsculas.");
        }
    }


