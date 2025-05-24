using System.Diagnostics;
using Utils.dtos;
using Utils.EstructuraDeDatos.Arboles.ABB;
using Utils.EstructuraDeDatos.Arboles.ArbolesBinarios;
using Utils.EstructuraDeDatos.Arboles.AVL;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.EstructuraDeDatos.ListasEnlazadas;
using Utils.EstructuraDeDatos.ListasEnlazadas.Pilas;
using Utils.models;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;
using Utils.validations;
using Utils.EstructuraDeDatos.ListasEnlazadas.Colas;
using System.Text.RegularExpressions;
using Utils.EstructuraDeDatos.DispercionesHash;

namespace ProyectiFinal.Services
{
    public class ClientService
    {
        public ABB bst;
        public AVL avl;
        public AVL cardsAvl;
        public Validations validations;

        Hash hashNit;
        Hash hashEmail;
        Hash hashPhone;


        public ClientService()
        {
            bst = new ABB();
            avl = new AVL();
            cardsAvl = new AVL();
            validations = new Validations();
            hashNit = new Hash();
            hashEmail = new Hash();
            hashPhone = new Hash();
        }

        public ClientsDPI convertObject(Cliente client)
        {
            ClientsDPI clientsDPI = new ClientsDPI();
            clientsDPI.dpi = client.dpi;
            clientsDPI.nombre = client.nombre;
            clientsDPI.apellido = client.apellido;
            clientsDPI.direccion = client.direccion;
            clientsDPI.telefono = client.telefono;
            clientsDPI.email = client.email;
            clientsDPI.genero = client.genero;
            clientsDPI.nacimiento = client.nacimiento;
            clientsDPI.nacionalidad = client.nacionalidad;
            clientsDPI.nit = client.nit;
            clientsDPI.estado = client.estado;
            //clientsDPI.tarjetas = client.tarjetas;
            return clientsDPI;
        }

        public void buildCardInfo(Cliente cliente, List<Tarjeta> tarjetasJson)
        {
            NodoLista actual = cliente.tarjetasLista.obtenerCabeza();

            while (actual != null)
            {
                Tarjeta tarjeta = (Tarjeta)actual.getItem();

                List<Transaccion> transaccionesJson = new List<Transaccion>();

                NodoLista nodoTransaccion = tarjeta.transacciones.actual;
                

                while (nodoTransaccion != null)
                {
                    Transaccion transaccion = (Transaccion)nodoTransaccion.getItem();

                    if(transaccion != null)
                    {
                        transaccionesJson.Add(transaccion);
                        nodoTransaccion = nodoTransaccion.getNext();
                    }
                }


                List<Solicitud> solicitudesJson = new List<Solicitud>();

                NodoLista nodoLista = tarjeta.colaSolicitudes.actual;

                while (nodoLista != null)
                {
                    Solicitud solicitud = (Solicitud)nodoLista.getItem();

                    if (solicitud != null)
                    {
                        solicitudesJson.Add(solicitud);
                        nodoLista = nodoLista.getNext();
                    }
                }


                tarjeta.historialTransacciones = transaccionesJson;
                tarjeta.solicitudes = solicitudesJson;
                tarjetasJson.Add(tarjeta);
                actual = actual.getNext();
            }
        }


        public CardsNumber searchAllExistingCards(string cardNumber)
        {
            CardsNumber card = new CardsNumber();
            card.numeroTarjeta = Regex.Replace(cardNumber, @"\D", "");
            Nodo nodo = cardsAvl.search(card);
            if (nodo != null)
            {
                card = (CardsNumber)nodo.getDato();
                return card;
            }
            else
            {
                return null;
            }
        }


        // CRUD OPERATIONS

        public Response findAll()
        {
            List<Cliente> clientes = new List<Cliente>();
            List<Nodo> nodos = ArbolBinario.inOrden(bst.getRaiz());

            if (nodos.Count == 0)
            {
                return new Response("No hay clientes", false);
            }

            foreach (var nodo in nodos)
            {
                Cliente cliente = (Cliente)nodo.getDato();

                //Debug.WriteLine("Cliente: " + cliente.ToString());

                List<Tarjeta> tarjetasJson = new List<Tarjeta>();

                buildCardInfo(cliente, tarjetasJson);

                cliente.tarjetas = tarjetasJson;
                clientes.Add(cliente);
            }


            return new Response("Clientes encontrados", true, clientes, clientes.Count);
        }

        public Response findActiveUsers()
        {
            List<Cliente> clientes = new List<Cliente>();
            List<NodoAVL> nodos = AVL.inOrden(avl.raizArbol());

            if (nodos.Count == 0)
            {
                return new Response("No hay clientes activos", false);
            }

            foreach (var nodo in nodos)
            {
                //Debug.WriteLine("Iteracion: " + nodo.ToString());

                Cliente cliente = (Cliente)nodo.getDato();

                List<Tarjeta> tarjetasJson = new List<Tarjeta>();

                buildCardInfo(cliente, tarjetasJson);

                cliente.tarjetas = tarjetasJson;
                clientes.Add(cliente);
            }

            return new Response("Clientes activos encontrados", true, clientes, clientes.Count);
        }


        public Response search(Cliente cliente)
        {
            ClientsDPI clientsDPI = convertObject(cliente);
            Nodo nodo = bst.search(clientsDPI);

            if (nodo == null)
            {
                return new Response($"El cliente con el DPI {cliente.dpi} no existe", false);
            }
            else
            {
                Cliente clienteEncontrado = (Cliente)nodo.getDato();

                List<Tarjeta> tarjetasJson = new List<Tarjeta>();

                buildCardInfo(clienteEncontrado, tarjetasJson);

                clienteEncontrado.tarjetas = tarjetasJson;

                return new Response("El cliente existe", true, clienteEncontrado, 1);
            }
        }


        public Response insert(Cliente cliente)
        {

            cliente.estado = 1;

            ValidationResponse isValidClient = validations.isValidClient(cliente);

            if (!(isValidClient).success)
            {
                throw new Exception(isValidClient.ToString());
            }

            if (hashNit.Buscar(cliente.nit, "nit") != null)
            {
                throw new Exception($"El cliente con el NIT {cliente.nit} ya existe.");
            }

            if (hashEmail.Buscar(cliente.email, "email") != null)
            {
                throw new Exception($"El cliente con el correo {cliente.email} ya existe.");
            }

            if (hashPhone.Buscar(cliente.telefono, "telefono") != null)
            {
                throw new Exception($"El cliente con el teléfono {cliente.telefono} ya existe.");
            }


            ClientsDPI clientsDPI = convertObject(cliente);

            Nodo nodo = bst.search(clientsDPI);
            if (nodo != null)
            {
                throw new Exception("El cliente con el DPI " + cliente.dpi + " ya existe");
            }
            else
            {

                if (clientsDPI.estado == 1)
                {
                    avl.insertar(clientsDPI);
                }

                bst.insert(clientsDPI);

                hashNit.Insertar(cliente, cliente.nit);
                hashEmail.Insertar(cliente, cliente.email);
                hashPhone.Insertar(cliente, cliente.telefono);



            }
            return new Response("Cliente insertado correctamente", true, cliente, 1);
        }

        public Response update(Cliente cliente)
        {

            ValidationResponse isValidClient = validations.isValidClient(cliente);
            if (!(isValidClient).success)
            {
                throw new Exception(isValidClient.ToString());
            }

            ClientsDPI clientsDPI = convertObject(cliente);

            Nodo nodoExistente = bst.search(clientsDPI);
            if (nodoExistente == null)
            {
                throw new Exception($"El cliente con el DPI {cliente.dpi} no existe");
            }


            object exisitingByNit = hashNit.Buscar(cliente.nit, "nit");
            object exisitingByEmail = hashEmail.Buscar(cliente.email, "email");
            object exisitingByPhone = hashPhone.Buscar(cliente.telefono, "telefono");


            bool sameNit = exisitingByNit != null && ((Cliente)exisitingByNit).dpi == cliente.dpi;
            bool sameEmail = exisitingByEmail != null && ((Cliente)exisitingByEmail).dpi == cliente.dpi;
            bool samePhone = exisitingByPhone != null && ((Cliente)exisitingByPhone).dpi == cliente.dpi;


            if (exisitingByNit != null && !sameNit)
            {
                throw new Exception($"El cliente con el NIT {cliente.nit} ya existe.");
            }

            if (exisitingByEmail != null && !sameEmail)
            {
                throw new Exception($"El cliente con el correo {cliente.email} ya existe.");
            }

            if (exisitingByPhone != null && !samePhone)
            {
                throw new Exception($"El cliente con el teléfono {cliente.telefono} ya existe.");
            }

            Cliente clienteExistente = (Cliente)nodoExistente.getDato();

            cliente.estado = clienteExistente.estado;
            clientsDPI.estado = clienteExistente.estado;

            clientsDPI.tarjetasLista = clienteExistente.tarjetasLista;
            bst.update(clientsDPI);

            if (clienteExistente.nit != cliente.nit)
            {
                hashNit.Eliminar(clienteExistente.nit, "nit");
                hashNit.Insertar(clientsDPI, cliente.nit);
            }
            else
            {
                hashNit.Actualizar(clientsDPI, cliente.nit, "nit");
            }

            if (clienteExistente.email != cliente.email)
            {
                hashEmail.Eliminar(clienteExistente.email, "email");
                hashEmail.Insertar(clientsDPI, cliente.email);
            }
            else
            {
                hashEmail.Actualizar(clientsDPI, cliente.email, "email");
            }

            if (clienteExistente.telefono != cliente.telefono)
            {
                hashPhone.Eliminar(clienteExistente.telefono, "telefono");
                hashPhone.Insertar(clientsDPI, cliente.telefono);
            }
            else
            {
                hashPhone.Actualizar(clientsDPI, cliente.telefono, "telefono");
            }

            if (clientsDPI.estado == 1)
            {
                NodoAVL nodoAVL = avl.search(clientsDPI);
                if (nodoAVL == null)
                {
                    avl.insertar(clientsDPI);
                }
                else
                {
                    avl.update(clientsDPI);
                }
            }

            if(clientsDPI.estado == 0)
            {
                NodoAVL nodoAVL = avl.search(clientsDPI);
                if (nodoAVL != null)
                {
                    avl.eliminar(clientsDPI);
                }
            }

            return new Response("Cliente actualizado correctamente", true, cliente, 1);
        }

        public Response delete(Cliente cliente)
        {
            ClientsDPI clientsDPI = convertObject(cliente);

            Cliente clienteEncontrado;

            Nodo nodo = bst.search(clientsDPI);
            if (nodo == null)
            {
                throw new Exception("El cliente con el DPI " + cliente.dpi + " no existe");
            }
            else
            {
                clienteEncontrado = (Cliente)nodo.getDato();

                hashNit.Eliminar(clienteEncontrado.nit, "nit");
                hashEmail.Eliminar(clienteEncontrado.email, "email");
                hashPhone.Eliminar(clienteEncontrado.telefono, "telefono");

                bst.delete(clientsDPI);

                NodoLista actual = clienteEncontrado.tarjetasLista.obtenerCabeza();

                while (actual != null)
                {
                    Tarjeta tarjeta = (Tarjeta)actual.getItem();
                    CardsNumber copyCard = new CardsNumber
                    {
                        numeroTarjeta = tarjeta.numeroTarjeta,
                        fechaExpiracion = tarjeta.fechaExpiracion,
                        cvv = tarjeta.cvv,
                        pin = tarjeta.pin,
                        limiteCredito = tarjeta.limiteCredito,
                        saldoActual = tarjeta.saldoActual,
                        estado = tarjeta.estado,
                        tipo = tarjeta.tipo,
                    };
                    cardsAvl.eliminar(copyCard);
                    actual = actual.getNext();
                }

                    NodoAVL nodoAVL = avl.search(clientsDPI);

                if (nodoAVL != null)
                {
                    avl.eliminar(clientsDPI);
                }
            }
            return new Response("Cliente eliminado correctamente", true, clienteEncontrado, 1);
        }


        public Response updateStatus(Cliente cliente)
        {
            ValidationResponse isValidClient = validations.isValidStatus(cliente.estado);
            if (!(isValidClient).success)
            {
                throw new Exception(isValidClient.ToString());
            }

            ClientsDPI clientsDPI = convertObject(cliente);

            Nodo nodoExistente = bst.search(clientsDPI);
            if (nodoExistente == null)
            {
                throw new Exception($"El cliente con el DPI {cliente.dpi} no existe");
            }

            Cliente clienteExistente = (Cliente)nodoExistente.getDato();

            ClientsDPI parsedClient = convertObject(clienteExistente);
            parsedClient.tarjetasLista = clienteExistente.tarjetasLista;
            bst.update(parsedClient);

            int newStatus = clienteExistente.estado == 1 ? 0 : 1;

            parsedClient.estado = newStatus;


            if (newStatus == 1)
            {
                NodoAVL nodoAVL = avl.search(parsedClient);
                if (nodoAVL == null)
                {
                    avl.insertar(parsedClient);
                }
                else
                {
                    avl.update(parsedClient);
                }
            }

            if (newStatus == 0)
            {
                NodoAVL nodoAVL = avl.search(parsedClient);
                if (nodoAVL != null)
                {
                    avl.eliminar(parsedClient);
                }
            }


            return new Response("Cliente actualizado correctamente", true, new
            {
                dpi = cliente.dpi,
                estado = newStatus
            }, 1);
        }

        // Upload initial data

        public void insertMany(Clientes clientes)
        {


            foreach (var cliente in clientes.clientes)
            {

                ValidationResponse validClient = validations.isValidClient(cliente);



                if (!(validClient).success)
                {
                    throw new Exception(validClient.ToString());
                }



                if (hashNit.Buscar(cliente.nit, "nit") != null)
                {
                    throw new Exception($"El cliente con el NIT {cliente.nit} ya existe.");
                }

                if (hashEmail.Buscar(cliente.email, "email") != null)
                {
                    throw new Exception($"El cliente con el correo {cliente.email} ya existe.");
                }

                if (hashPhone.Buscar(cliente.telefono, "telefono") != null)
                {
                    throw new Exception($"El cliente con el teléfono {cliente.telefono} ya existe.");
                }


                ClientsDPI clientsDPI = convertObject(cliente);

                Nodo nodo = bst.search(clientsDPI);
                NodoAVL nodoAVL = avl.search(clientsDPI);

                if (nodo != null || nodoAVL != null)
                {
                    throw new Exception($"El cliente con el DPI {clientsDPI.dpi} ya existe");
                }

                ListaEnlazada tarjetasLista = new ListaEnlazada();

                if (cliente.tarjetas != null)
                {
                    foreach (var tarjetaDto in cliente.tarjetas)
                    {

                        CardsNumber existingCard = searchAllExistingCards(tarjetaDto.numeroTarjeta);
                        if (existingCard != null)
                        {
                            throw new Exception("La tarjeta con el numero "+tarjetaDto.numeroTarjeta+" ya existe.");
                        }

                        Tarjeta tarjeta = new Tarjeta
                        {
                            numeroTarjeta = tarjetaDto.numeroTarjeta,
                            fechaExpiracion = tarjetaDto.fechaExpiracion,
                            cvv = tarjetaDto.cvv,
                            pin = tarjetaDto.pin,
                            limiteCredito = tarjetaDto.limiteCredito,
                            saldoActual = tarjetaDto.saldoActual,
                            estado = tarjetaDto.estado,
                            tipo = tarjetaDto.tipo,
                            transacciones = new Pila(),
                            colaSolicitudes = new Cola()
                        };


                        ValidationResponse isValidCard = validations.isValidCard(tarjeta);

                        if (!(isValidCard).success)
                        {
                            throw new Exception(isValidCard.ToString());
                        }


                        if (tarjetaDto.historialTransacciones != null)
                        {
                            foreach (var transaccion in tarjetaDto.historialTransacciones)
                            {
                                tarjeta.transacciones.add(transaccion);
                            }
                        }

                        if(tarjetaDto.solicitudes != null)
                        {
                            foreach(var solicitud in tarjetaDto.solicitudes)
                            {
                                tarjeta.colaSolicitudes.add(solicitud);
                            }
                        }

                        tarjetasLista.Insertar(tarjeta);

                        CardsNumber cardsNumber = new CardsNumber();
                        cardsNumber.numeroTarjeta = tarjeta.numeroTarjeta;
                        cardsNumber.fechaExpiracion = tarjeta.fechaExpiracion;
                        cardsNumber.cvv = tarjeta.cvv;
                        cardsNumber.pin = tarjeta.pin;
                        cardsNumber.limiteCredito = tarjeta.limiteCredito;
                        cardsNumber.saldoActual = tarjeta.saldoActual;
                        cardsNumber.estado = tarjeta.estado;
                        cardsNumber.tipo = tarjeta.tipo;
                        //cardsNumber.transacciones = tarjeta.transacciones;
                        //cardsNumber.solicitudes = tarjeta.solicitudes;

                        cardsAvl.insertar(cardsNumber);

                    }

                    clientsDPI.tarjetasLista = tarjetasLista;

                    if (clientsDPI.estado == 1)
                    {
                        //Debug.WriteLine("Cliente activo: " + clientsDPI.dpi.ToString());
                        avl.insertar(clientsDPI);
                    }
                    bst.insert(clientsDPI);
                    hashNit.Insertar(cliente, cliente.nit);
                    hashEmail.Insertar(cliente, cliente.email);
                    hashPhone.Insertar(cliente, cliente.telefono);

                }

            }

            hashNit.imprimir();
            hashEmail.imprimir();
            hashPhone.imprimir();
        }
    }
}
