using NearDupFinder_Dominio.Clases;

namespace NormalizacionPruebas;

[TestClass]
public class NormalizacionPruebas{
[TestMethod]
        public void NormalizarItem_TituloConMayusculas_MinusculasCorrectas()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "LAPTOP",
                Descripcion = "LAPTOP",
                Marca = "MarcaX",
                Modelo = "Modelo1",
                Categoria = "Categoria1"
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("laptop", resultado.Titulo);
            Assert.AreEqual("laptop", resultado.Descripcion);

            Assert.AreEqual("marcax", resultado.Marca);
            Assert.AreEqual("modelo1", resultado.Modelo);
            Assert.AreEqual("categoria1", resultado.Categoria);
        }

        [TestMethod]
        public void NormalizarItem_TituloConTildesYN_SeNormalizaCorrectamente()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "Cómputañó",
                Descripcion = "Désc",
                Marca = "Ñandú",
                Modelo = "Módelo",
                Categoria = "Tecnología"
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("computano", resultado.Titulo);
            Assert.AreEqual("desc", resultado.Descripcion);
            Assert.AreEqual("nandu", resultado.Marca);
            Assert.AreEqual("modelo", resultado.Modelo);
            Assert.AreEqual("tecnologia", resultado.Categoria);
        }

        [TestMethod]
        public void NormalizarItem_TituloConSimbolosEspeciales_NormalizaCorrectamente()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "Lap_tóp!123#",
                Descripcion = "De$c",
                Marca = "To!shIBa",
                Modelo = "MÓDeLo#1",
                Categoria = "TeCnología!"
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("lap top 123", resultado.Titulo);
            Assert.AreEqual("de c", resultado.Descripcion);
            Assert.AreEqual("to shiba", resultado.Marca);
            Assert.AreEqual("modelo 1", resultado.Modelo);
            Assert.AreEqual("tecnologia", resultado.Categoria);
        }

        [TestMethod]
        public void NormalizarItem_EspaciosMultiples_ColapsaYRecorta()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "  Lap_tóp   123!!  ",
                Descripcion = "de   sc",
                Marca = "  To!shIBa  ",
                Modelo = " MÓDeLo   #1 ",
                Categoria = "  TeCnología! "
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("lap top 123", resultado.Titulo);
            Assert.AreEqual("de sc", resultado.Descripcion);
            
            Assert.AreEqual("to shiba", resultado.Marca);
            Assert.AreEqual("modelo 1", resultado.Modelo);
            Assert.AreEqual("tecnologia", resultado.Categoria);
        }

        [TestMethod]
        public void NormalizarItem_ItemSoloConSimbolos_LanzaExcepcionConMensaje()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "!@#$%^&*()",
                Descripcion = "!!!!!",
                Marca = "***###",
                Modelo = "###$$$",
                Categoria = "!!@@"
            };

            var ex = Assert.ThrowsException<InvalidOperationException>(() => sistema.NormalizarItem(item));

            Assert.AreEqual("El título o la descripción no puede quedar vacío tras normalizar.", ex.Message);
        }


        [TestMethod]
        public void NormalizarItem_MarcaModeloCategoriaVacios_NoLanzaExcepcion()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "Laptop",
                Descripcion = "Computadora potente",
                Marca = "!@#$$%", // se normaliza a vacío
                Modelo = "###$$$", // se normaliza a vacío
                Categoria = "!!@@" // se normaliza a vacío
            };

            // No debe lanzar excepción
            var resultado = sistema.NormalizarItem(item);

            // Verificar normalización
            Assert.AreEqual("laptop", resultado.Titulo);
            Assert.AreEqual("computadora potente", resultado.Descripcion);
            Assert.AreEqual(string.Empty, resultado.Marca);
            Assert.AreEqual(string.Empty, resultado.Modelo);
            Assert.AreEqual(string.Empty, resultado.Categoria);
        }
        [TestMethod]
        public void NormalizarItem_TextoYaNormalizado_NoCambia()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "laptop",
                Descripcion = "computadora potente",
                Marca = "marca",
                Modelo = "modelo",
                Categoria = "categoria"
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("laptop", resultado.Titulo);
            Assert.AreEqual("computadora potente", resultado.Descripcion);
            Assert.AreEqual("marca", resultado.Marca);
            Assert.AreEqual("modelo", resultado.Modelo);
            Assert.AreEqual("categoria", resultado.Categoria);
        }
        [TestMethod]
        public void NormalizarItem_CombinacionCaracteres_NormalizaTodo()
        {
            var sistema = new Sistema();
            var item = new Item
            {
                Titulo = "ÁÉÍÓÚñÜ!@#    $$%^&*()    ",
                Descripcion = "áéíóúÑü123",
                Marca = "Márcá$%",
                Modelo = "Módel#1",
                Categoria = "Cátégoría!"
            };

            var resultado = sistema.NormalizarItem(item);

            Assert.AreEqual("aeiounu", resultado.Titulo);
            Assert.AreEqual("aeiounu123", resultado.Descripcion);
            Assert.AreEqual("marca", resultado.Marca);
            Assert.AreEqual("model 1", resultado.Modelo);
            Assert.AreEqual("categoria", resultado.Categoria);
        }





    }


