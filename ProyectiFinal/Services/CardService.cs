using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Utils.dtos;
using Utils.dtos.Requests;
using Utils.enums;
using Utils.EstructuraDeDatos.Arboles.ABB;
using Utils.EstructuraDeDatos.Arboles.ArbolesBinarios;
using Utils.EstructuraDeDatos.Arboles.AVL;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;
using Utils.models;
using Utils.validations;

namespace ProyectiFinal.Services
{
    public class CardService
    {

        public ABB bst;
        public AVL avl;
        public AVL cardsAvl;
        public Validations validations;
        ClientService _treeService;

        public CardService(ABB bst, AVL avl, AVL cardsAvl, ClientService _treeService)
        {
            this.bst = bst;
            this.avl = avl;
            this.cardsAvl = cardsAvl;
            this.validations = new Validations();
            this._treeService = _treeService;
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


        public string generateRandomPin()
        {
            Random random = new Random();
            string pin = random.Next(1000, 9999).ToString();
            return pin;
        }


        private static readonly Random random = new Random();

        public string generateRandomExpDate(string date)
        {
            if (!DateTime.TryParseExact(
                    date,
                    "MM/yy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime actualDate))
            {
                throw new ArgumentException("El formato de la fecha debe ser MM/yy.");
            }

            int newYears = random.Next(2, 6);
            int month = random.Next(1, 13);

            int newYear = actualDate.Year + newYears;

            int actualYear = Int32.Parse(DateTime.Now.ToString("yyyy"));
            int actualMonth = Int32.Parse(DateTime.Now.ToString("MM"));

            //Debug.WriteLine("Año actual: " + actualYear);
            //Debug.WriteLine("Año nuevo: " + newYear);

            while (actualYear > newYear)
            {
                //Debug.WriteLine("Año actual: " + actualYear);
                //Debug.WriteLine("Año nuevo: " + newYear);
                newYears = random.Next(2, 6);
                newYear = actualDate.Year + newYears;

            }

            while (actualMonth > month && actualYear == newYear)
            {
                //Debug.WriteLine("Mes actual: " + actualMonth);
                //Debug.WriteLine("Mes nuevo: " + month);
                month = random.Next(actualMonth, 13);
            }


            DateTime newDate = new DateTime(newYear, month, 1);


            return newDate.ToString("MM/yy");
        }

        // CRUD OPERATIONS

        public Response addClientCard(CreateUpdateCards cardInfo)
        {
            Response response = new Response();

            ClientsDPI searchUser = new ClientsDPI();
            searchUser.dpi = cardInfo.dpi;

            Nodo user = avl.search(searchUser);

            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }

            if (user.getDato() is Cliente client)
            {
                Tarjeta card = new Tarjeta();
                card.tipo = cardInfo.tipo;
                card.numeroTarjeta = cardInfo.numeroTarjeta;
                card.fechaExpiracion = cardInfo.fechaExpiracion;
                card.cvv = cardInfo.cvv;
                card.pin = cardInfo.pin;
                card.limiteCredito = cardInfo.limiteCredito;
                card.saldoActual = cardInfo.saldoActual;
                card.estado = 1;


                ValidationResponse isValidCard = validations.isValidCard(card);

                if (!(isValidCard).success)
                {
                    throw new Exception(isValidCard.ToString());
                }

                CardsNumber existingCard = searchAllExistingCards(card.numeroTarjeta);

                if (existingCard != null)
                {
                    response.Message = "La tarjeta ya existe: " + existingCard.ToString();
                    response.Success = false;
                    return response;
                }

                bool added = client.addTarjeta(card);

                if (!added)
                {
                    response.Message = "Error al guardar la tarjeta";
                    response.Success = false;
                    return response;
                }

                CardsNumber cardNumber = new CardsNumber();
                cardNumber.numeroTarjeta = card.numeroTarjeta;
                cardNumber.fechaExpiracion = card.fechaExpiracion;
                cardNumber.cvv = card.cvv;
                cardNumber.pin = card.pin;
                cardNumber.limiteCredito = card.limiteCredito;
                cardNumber.saldoActual = card.saldoActual;
                cardNumber.estado = card.estado;
                cardNumber.tipo = card.tipo;

                cardsAvl.insertar(cardNumber);

                avl.update(user.getDato());
                bst.update(user.getDato());

                response.Message = "Tarjeta agregada correctamente";
                response.Success = true;
                response.Data = card;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }


        }


        public Response retrieveBalances(RetrieveBalances retrieveBalances)
        {
            Response response = new Response();
            ClientsDPI searchUser = new ClientsDPI();
            searchUser.dpi = retrieveBalances.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {
                CardsNumber existingCard = searchAllExistingCards(retrieveBalances.numeroTarjeta);
                if (existingCard == null)
                {
                    response.Message = "La tarjeta no existe";
                    response.Success = false;
                    return response;
                }

                Tarjeta userCard = client.returnTarjeta(retrieveBalances.numeroTarjeta, retrieveBalances.pin);

                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }

                Tarjeta card = new Tarjeta();
                card.numeroTarjeta = retrieveBalances.numeroTarjeta;
                card.pin = retrieveBalances.pin;
                ValidationResponse isValidCard = validations.isValidCardNumber(card.numeroTarjeta);
                ValidationResponse isValidPin = validations.isValidPin(card.pin);

                if (!(isValidCard).success)
                {
                    throw new Exception(isValidCard.ToString());
                }

                if (!(isValidPin).success)
                {
                    throw new Exception(isValidPin.ToString());
                }


                if (existingCard.pin != card.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }


                response.Message = "Consulta exitosa";
                response.Success = true;
                response.Data = existingCard;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }

        public Response retrieveTransactions(RetrieveBalances retrieveBalances)
        {
            Response response = new Response();
            ClientsDPI searchUser = new ClientsDPI();
            searchUser.dpi = retrieveBalances.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {
                Tarjeta userCard = client.returnTarjeta(retrieveBalances.numeroTarjeta, retrieveBalances.pin);
                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }
                CardsNumber existingCard = searchAllExistingCards(userCard.numeroTarjeta);
                if (existingCard == null)
                {
                    response.Message = "La tarjeta no existe";
                    response.Success = false;
                    return response;
                }
                if (existingCard.pin != userCard.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }
                response.Message = "Consulta exitosa";
                response.Success = true;


                CardsNumber copyCard = existingCard.clone();


                Response clientDpi = _treeService.search(searchUser);

                if (clientDpi == null)
                {
                    response.Message = "El cliente no existe";
                    response.Success = false;
                    return response;
                }


                Cliente cliente = (Cliente)clientDpi.Data;

                foreach (Tarjeta tarjeta in cliente.tarjetas)
                {
                    if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                    {
                        copyCard.historialTransacciones = tarjeta.historialTransacciones;
                        copyCard.solicitudes = tarjeta.solicitudes;
                        break;
                    }
                }


                response.Data = copyCard;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }


        public Response makePayments(MakePayments makePayments)
        {
            Response response = new Response();


            CardsNumber existingCard = searchAllExistingCards(makePayments.numeroTarjeta);
            if (existingCard == null)
            {
                response.Message = "La tarjeta no existe";
                response.Success = false;
                return response;
            }

            ClientsDPI searchUser = new ClientsDPI();


            searchUser.dpi = makePayments.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {

                Tarjeta userCard = client.returnTarjeta(makePayments.numeroTarjeta, makePayments.pin);
                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }

                if (existingCard.pin != userCard.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }

                if (existingCard.cvv != makePayments.cvv)
                {
                    response.Message = "El CVV es incorrecto";
                    response.Success = false;
                    return response;
                }

                if (existingCard.fechaExpiracion != makePayments.fechaExpiracion)
                {
                    response.Message = "La fecha de expiracion es incorrecta";
                    response.Success = false;
                    return response;
                }

                if (makePayments.monto > existingCard.saldoActual)
                {
                    response.Message = "El monto excede el saldo actual";
                    response.Success = false;
                    return response;
                }


                if (existingCard.estado != 1)
                {
                    response.Message = "La tarjeta no esta activa";
                    response.Success = false;
                    return response;
                }

                if (makePayments.monto <= 0)
                {
                    response.Message = "El monto debe ser mayor a 0";
                    response.Success = false;
                    return response;
                }

                ValidationResponse isValidExpDate = validations.isValidCardDate(makePayments.fechaExpiracion);

                if (!(isValidExpDate).success)
                {
                    throw new Exception(isValidExpDate.ToString());
                }


                //buildCardInfo(client, existingCard);

                existingCard.saldoActual -= makePayments.monto;

                existingCard.transacciones.add(new Transaccion
                {
                    fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    hora = DateTime.Now.ToString("HH:mm:ss"),
                    monto = makePayments.monto,
                    tipo = makePayments.tipo,
                    descripcion = makePayments.descripcion,
                    autorizada = true

                });

                cardsAvl.update(existingCard);

                ClientsDPI parsedClient = (ClientsDPI)user.getDato();

                NodoLista tarjetaNodo = parsedClient.tarjetasLista.cabeza;
                while (tarjetaNodo != null)
                {
                    Tarjeta tarjeta = (Tarjeta)tarjetaNodo.getItem();
                    if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                    {
                        tarjeta.saldoActual = existingCard.saldoActual;
                        tarjeta.transacciones.add(new Transaccion
                        {
                            fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                            hora = DateTime.Now.ToString("HH:mm:ss"),
                            monto = makePayments.monto,
                            tipo = makePayments.tipo,
                            descripcion = makePayments.descripcion,
                            autorizada = true

                        });
                        break;
                    }
                    tarjetaNodo = tarjetaNodo.getNext();
                }


                avl.update(parsedClient);
                bst.update(parsedClient);
                response.Message = "Pago realizado correctamente";
                response.Success = true;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }

        public Response renovateExpirationDate(Managements request)
        {
            Response response = new Response();
            ClientsDPI searchUser = new ClientsDPI();
            searchUser.dpi = request.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {
                Tarjeta userCard = client.returnTarjeta(request.numeroTarjeta, request.pin);
                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }
                CardsNumber existingCard = searchAllExistingCards(userCard.numeroTarjeta);
                if (existingCard == null)
                {
                    response.Message = "La tarjeta no existe";
                    response.Success = false;
                    return response;
                }
                if (existingCard.pin != userCard.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }

                ValidationResponse isValidExpDate = validations.isValidCardDate(existingCard.fechaExpiracion);


                string newDate = generateRandomExpDate(existingCard.fechaExpiracion);

                //buildCardInfo(client, existingCard);

                existingCard.colaSolicitudes.add(new Solicitud
                {
                    fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                    estado = isValidExpDate.success ? "DENEGADA" : "AUTORIZADA",
                    tipo = TipoSolicitud.Renovacion,
                    fechaRenovacion = isValidExpDate.success ? "" : newDate,
                });

                ClientsDPI parsedClient = (ClientsDPI)user.getDato();

                NodoLista tarjetaNodo = parsedClient.tarjetasLista.cabeza;

                while (tarjetaNodo != null)
                {
                    Tarjeta tarjeta = (Tarjeta)tarjetaNodo.getItem();
                    if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                    {

                        if (!isValidExpDate.success)
                        {
                            tarjeta.fechaExpiracion = newDate;
                        }

                        tarjeta.colaSolicitudes.add(new Solicitud
                        {
                            fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                            estado = isValidExpDate.success ? "DENEGADA" : "AUTORIZADA",
                            tipo = TipoSolicitud.Renovacion,
                            fechaRenovacion = isValidExpDate.success ? "" : newDate,
                        });
                        break;
                    }
                    tarjetaNodo = tarjetaNodo.getNext();
                }


                avl.update(parsedClient);
                bst.update(parsedClient);


                if (isValidExpDate.success)
                {
                    response.Message = "La tarjeta sigue vigente, no necesita renovación.";
                    response.Success = false;
                    return response;
                }

                existingCard.fechaExpiracion = newDate;

                cardsAvl.update(existingCard);

                response.Message = "Tarjeta renovada exitosamente";
                response.Data = new
                {
                    tarjeta = existingCard.numeroTarjeta,
                    fechaExpiracion = existingCard.fechaExpiracion
                };
                response.Success = true;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }

        public Response updatePin(Managements request)
        {
            Response response = new Response();


            CardsNumber existingCard = searchAllExistingCards(request.numeroTarjeta);
            if (existingCard == null)
            {
                response.Message = "La tarjeta no existe";
                response.Success = false;
                return response;
            }

            ClientsDPI searchUser = new ClientsDPI();

            searchUser.dpi = request.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {

                Tarjeta userCard = client.returnTarjeta(request.numeroTarjeta, request.pin);
                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }

                if (existingCard.pin != userCard.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }

                if (existingCard.estado != 1)
                {
                    response.Message = "La tarjeta no esta activa";
                    response.Success = false;
                    return response;
                }

                existingCard.pin = generateRandomPin();

                existingCard.colaSolicitudes.add(new Solicitud
                {
                    fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                    nuevoPin = existingCard.pin,
                    estado = "AUTORIZADA",
                    tipo = TipoSolicitud.Pin,
                });

                cardsAvl.update(existingCard);

                ClientsDPI parsedClient = (ClientsDPI)user.getDato();

                NodoLista tarjetaNodo = parsedClient.tarjetasLista.cabeza;
                while (tarjetaNodo != null)
                {
                    Tarjeta tarjeta = (Tarjeta)tarjetaNodo.getItem();
                    if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                    {
                        tarjeta.pin = existingCard.pin;
                        tarjeta.colaSolicitudes.add(new Solicitud
                        {
                            fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                            nuevoPin = existingCard.pin,
                            estado = "AUTORIZADA",
                            tipo = TipoSolicitud.Pin,
                        });
                        break;
                    }
                    tarjetaNodo = tarjetaNodo.getNext();
                }


                avl.update(parsedClient);
                bst.update(parsedClient);
                response.Message = "Pin actualizado correctamente";
                response.Success = true;
                response.Data = new
                {
                    tarjeta = existingCard.numeroTarjeta,
                    nuevoPin = existingCard.pin
                };
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }


        public Response blockCard(Managements request)
        {
            Response response = new Response();


            CardsNumber existingCard = searchAllExistingCards(request.numeroTarjeta);
            if (existingCard == null)
            {
                response.Message = "La tarjeta no existe";
                response.Success = false;
                return response;
            }

            ClientsDPI searchUser = new ClientsDPI();


            searchUser.dpi = request.dpi;
            Nodo user = avl.search(searchUser);
            if (user == null)
            {
                response.Message = "El cliente no existe o está inactivo.";
                response.Success = false;
                return response;
            }
            if (user.getDato() is Cliente client)
            {

                Tarjeta userCard = client.returnTarjeta(request.numeroTarjeta, request.pin);
                if (userCard == null)
                {
                    response.Message = "La tarjeta no pertenece al usuario indicado.";
                    response.Success = false;
                    return response;
                }

                if (existingCard.pin != userCard.pin)
                {
                    response.Message = "El pin es incorrecto";
                    response.Success = false;
                    return response;
                }


                //buildCardInfo(client, existingCard);


                existingCard.estado = existingCard.estado == 1 ? 0 : 1;

                string message = existingCard.estado == 1 ? "Tarjeta desbloqueada correctamente." : "Tarjeta bloqueada correctamente.";

                existingCard.colaSolicitudes.add(new Solicitud
                {
                    fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                    estado = "AUTORIZADA",
                    tipo = existingCard.estado == 1 ? TipoSolicitud.Desbloqueo : TipoSolicitud.Bloqueo,
                });

                cardsAvl.update(existingCard);

                ClientsDPI parsedClient = (ClientsDPI)user.getDato();

                NodoLista tarjetaNodo = parsedClient.tarjetasLista.cabeza;
                while (tarjetaNodo != null)
                {
                    Tarjeta tarjeta = (Tarjeta)tarjetaNodo.getItem();
                    if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                    {
                        tarjeta.estado = existingCard.estado;
                        tarjeta.colaSolicitudes.add(new Solicitud
                        {
                            fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                            estado = "AUTORIZADA",
                            tipo = existingCard.estado == 1 ? TipoSolicitud.Desbloqueo : TipoSolicitud.Bloqueo,
                        });
                        break;
                    }
                    tarjetaNodo = tarjetaNodo.getNext();
                }


                avl.update(parsedClient);
                bst.update(parsedClient);
                response.Message = message;
                response.Success = true;
                return response;
            }
            else
            {
                response.Message = "El cliente no existe";
                response.Success = false;
                return response;
            }
        }



        public Response incrementCreditLimit(Managements request)
        {
            Response response = new Response();
            CardsNumber existingCard = searchAllExistingCards(request.numeroTarjeta);
            if (existingCard != null)
            {
                ClientsDPI searchUser = new ClientsDPI();
                searchUser.dpi = request.dpi;
                Nodo user = avl.search(searchUser);
                if (user == null)
                {
                    response.Message = "El cliente no existe o está inactivo.";
                    response.Success = false;
                    return response;
                }
                if (user.getDato() is Cliente client)
                {
                    Tarjeta userCard = client.returnTarjeta(request.numeroTarjeta, request.pin);
                    if (userCard == null)
                    {
                        response.Message = "La tarjeta no pertenece al usuario indicado.";
                        response.Success = false;
                        return response;
                    }
                    if (existingCard.pin != userCard.pin)
                    {
                        response.Message = "El pin es incorrecto";
                        response.Success = false;
                        return response;
                    }

                    if (existingCard.estado != 1)
                    {
                        response.Message = "La tarjeta no esta activa";
                        response.Success = false;
                        return response;
                    }

                    if (existingCard.limiteCredito == 0)
                    {
                        response.Message = "La tarjeta no es de crédito.";
                        response.Success = false;
                        return response;
                    }


                    ValidationResponse isValidExpDate = validations.isValidCardDate(existingCard.fechaExpiracion);

                    if (!(isValidExpDate).success)
                    {
                        throw new Exception(isValidExpDate.ToString());
                    }


                    CardsNumber copyCard = existingCard.clone();

                    Response clientDpi = _treeService.search(searchUser);

                    if (clientDpi == null)
                    {
                        response.Message = "El cliente no existe";
                        response.Success = false;
                        return response;
                    }

                    Cliente cliente = (Cliente)clientDpi.Data;

                    foreach (Tarjeta tarjeta in cliente.tarjetas)
                    {
                        if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                        {
                            copyCard.solicitudes = tarjeta.solicitudes;
                            break;
                        }
                    }

                    bool increment = true;
                    response.Message = "Aumento de limite de credito autorizado";
                    response.Success = true;
                    foreach (Solicitud solicitud in copyCard.solicitudes)
                    {
                        //Debug.WriteLine("Solicitud: " + solicitud.tipo);
                        //Debug.WriteLine("Estado: " + solicitud.estado);
                        if (solicitud.tipo == TipoSolicitud.Aumento && solicitud.estado == "AUTORIZADA")
                        {
                            string[] fecha = solicitud.fechaSolicitud.Split("/");
                            string formatedDate = fecha[1] + "/"  + fecha[2];
                            //Debug.WriteLine("Fecha formateada: " + formatedDate);
                            ValidationResponse validIncrement = validations.validIncrementDate(formatedDate);

                            if (!validIncrement.success)
                            {
                                //Debug.WriteLine("No se puede solicitar un aumento de limite de credito, ya que no ha pasado el tiempo requerido. (1 año)");
                                response.Message = "Ya se ha autorizado un aumento de limite de credito, no puede solicitar otro hasta que pase el tiempo requerido. (1 año)";
                                response.Success = false;
                                increment = false;
                                break;
                            }
                            else
                            {
                                //Debug.WriteLine("Se puede solicitar un aumento de limite de credito.");
                                break;
                            }

                        }
                    }


                    if (increment)
                    {
                        existingCard.limiteCredito += 1000;
                    }


                    existingCard.colaSolicitudes.add(new Solicitud
                    {
                        fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                        montoSolicitado = increment ?  existingCard.limiteCredito : 0,
                        estado = increment ? "AUTORIZADA" : "DENEGADA",
                        tipo = TipoSolicitud.Aumento,
                    });
                    cardsAvl.update(existingCard);
                    ClientsDPI parsedClient = (ClientsDPI)user.getDato();
                    NodoLista tarjetaNodo = parsedClient.tarjetasLista.cabeza;
                    while (tarjetaNodo != null)
                    {
                        Tarjeta tarjeta = (Tarjeta)tarjetaNodo.getItem();
                        if (tarjeta.numeroTarjeta == existingCard.numeroTarjeta)
                        {
                            tarjeta.limiteCredito = existingCard.limiteCredito ;
                            tarjeta.colaSolicitudes.add(new Solicitud
                            {
                                fechaSolicitud = DateTime.Now.ToString("dd/MM/yyyy"),
                                montoSolicitado = increment ? existingCard.limiteCredito : 0,
                                estado = increment ? "AUTORIZADA" : "DENEGADA",
                                tipo = TipoSolicitud.Aumento,
                            });
                            break;
                        }
                        tarjetaNodo = tarjetaNodo.getNext();

                    }
                    avl.update(parsedClient);
                    bst.update(parsedClient);
                    response.Data = new
                    {
                        tarjeta = existingCard.numeroTarjeta,
                        nuevoLimiteCredito = existingCard.limiteCredito
                    };
                    return response;
                }
                else
                {   
                    response.Message = "El cliente no existe";
                    response.Success = false;
                    return response;
                }
            }
            else
            {
                response.Message = "La tarjeta no existe";
                response.Success = false;
                return response;
            }
        }
    }
}