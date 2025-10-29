using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Util;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.ProcesadorDeTexto;
using System.Collections.Generic;
using System.Linq;

namespace NearDupFinder_Pruebas.Servicios.Duplicados
{
    [TestClass]
    public class TokenizacionIntegracionPruebas
    {
        private GestorDuplicados _gestor = null!;

        private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria) =>
            new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };

        private static Catalogo CrearCatalogo(params Item[] items)
        {
            var catalogo = new Catalogo("Catalogo");
            foreach (var it in items) catalogo.AgregarItem(it);
            return catalogo;
        }

        [TestInitialize]
        public void Setup()
        {
            var analyzer = new SnowballAnalyzer(LuceneVersion.LUCENE_48, "Spanish");
            IProcesadorTexto proc = new ProcesadorTexto(analyzer);
            _gestor = new GestorDuplicados(proc);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Tokenizar_TokensDeUnCaracter_SeDescartan_Y_StopwordsSeEliminan()
        {
            var itemA = CrearItem("a b c de f 1 2", "x y z 12 q r s", "m", "x", "cat");
            var itemB = CrearItem("a b c de f 1 2", "x y z 12 q r s", "m", "x", "cat");

            var catalogo = CrearCatalogo(itemA, itemB);
            var pares = _gestor.DetectarDuplicados(itemA, catalogo);

            Assert.AreEqual(0, pares.Count,
                "No debería detectar duplicado: título vacío tras filtrado y score total < 0.60.");
        }


        [TestMethod]
        public void Tokenizar_TituloConMultiplesEspacios_ColapsaSeparadores_ConStemming()
        {
            var itemA = CrearItem("   Iphone   17   ", "Celular   de   última   generación   ", "m", "x", "cat");
            var itemB = CrearItem("Iphone 17", "Celular de última generación", "m", "x", "cat");
            var catalogo = CrearCatalogo(itemA, itemB);

            var pares = _gestor.DetectarDuplicados(itemA, catalogo);
            var titulo = pares[0].TokensCompartidosTitulo;
        
            Assert.AreEqual(2, titulo.Length, "El título debe tokenizarse en exactamente 2 tokens.");
            Assert.AreEqual("iphon", titulo[0], "Primer token del título debe ser 'iphon'.");
            Assert.AreEqual("17", titulo[1], "Segundo token del título debe ser '17'.");

        }
        
        [TestMethod]
        public void Tokenizar_DescripcionConMultiplesEspacios_ColapsaSeparadores_ConStemming()
        {
            var itemA = CrearItem("   Iphone   17   ", "Celular   de   última   generación   ", "m", "x", "cat");
            var itemB = CrearItem("Iphone 17", "Celular de última generación", "m", "x", "cat");
            var catalogo = CrearCatalogo(itemA, itemB);

            var pares = _gestor.DetectarDuplicados(itemA, catalogo);
            var desc = pares[0].TokensCompartidosDescripcion;

            Assert.IsTrue(Array.IndexOf(desc, "de") < 0, "La stopword 'de' no debe aparecer tras el procesamiento.");

            Assert.AreEqual(3, desc.Length, "La descripción debe tokenizarse en exactamente 3 tokens.");
            Assert.AreEqual("celul", desc[0], "Primer token debe ser la raíz 'celul'.");
            Assert.AreEqual("ultim", desc[1], "Segundo token debe ser la raíz 'ultim'.");
            Assert.AreEqual("generacion", desc[2], "Tercer token debe ser 'generacion'.");
        }



        [TestMethod]
        public void Tokenizar_TextoConNumeros_MantieneAlfanumericos_ConStemming()
        {
            var itemA = CrearItem("ps5 slim 1tb", "ssd 512", "m", "x", "cat");
            var itemB = CrearItem("ps5 slim 1tb", "ssd 512", "m", "x", "cat");

            var catalogo = CrearCatalogo(itemA, itemB);
            var pares = _gestor.DetectarDuplicados(itemA, catalogo);

            var titulo = pares[0].TokensCompartidosTitulo;
            var desc   = pares[0].TokensCompartidosDescripcion;

            CollectionAssert.AreEquivalent(new[] { "ps5", "slim", "1tb" }, titulo,
                "Los alfanuméricos deben conservarse.");
            CollectionAssert.AreEquivalent(new[] { "ssd", "512" }, desc,
                "Los tokens numéricos y alfabéticos simples deben conservarse.");
        }
        [TestMethod]
        public void Tokenizar_ConEspaciosYStopwords_SeNormalizaCorrectamente()
        {
            var itemA = CrearItem("   iphone 17   ", "   celular   de   ultima   generacion   ", "m", "x", "cat");
            var itemB = CrearItem("iphone 17",      "celular ultima generacion",               "m", "x", "cat");
            var catalogo = CrearCatalogo(itemA, itemB);

            var resultado = _gestor.DetectarDuplicados(itemA, catalogo)[0];
            var titulo = resultado.TokensCompartidosTitulo;
            var desc   = resultado.TokensCompartidosDescripcion;

            Assert.IsTrue(Array.IndexOf(titulo, "iphon") >= 0, "Se esperaba raíz exacta de 'iphone'.");
            Assert.IsTrue(Array.IndexOf(titulo, "17") >= 0, "Se esperaba el número '17'.");

            Assert.IsTrue(Array.IndexOf(desc, "celul") >= 0, "Raíz exacta de 'celular' esperada.");
            Assert.IsTrue(Array.IndexOf(desc, "ultim") >= 0, "Raíz exacta de 'ultima' esperada.");
            Assert.IsTrue(Array.IndexOf(desc, "generacion") >= 0, "Raíz exacta de 'generacion' esperada.");
            Assert.IsTrue(Array.IndexOf(desc, "de") < 0, "La stopword 'de' no debería aparecer.");
        }


    }
}
