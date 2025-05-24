using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Utils.dtos;
using Utils.enums;
using Utils.models;

namespace Utils.validations
{
    public class Validations
    {
        public Validations() { }

        public ValidationResponse isValidClient(Cliente cliente)
        {
            var validDPI = isValidDpi(cliente.dpi);
            var validNIT = isValidNit(cliente.nit.ToString());
            var validEmail = isValidEmail(cliente.email);
            var validStatus = isValidStatus(cliente.estado);

            if (!validDPI.success) return validDPI;
            if (!validNIT.success) return validNIT;
            if (!validEmail.success) return validEmail;
            if (!validStatus.success) return validStatus;

            return new ValidationResponse("El cliente es válido", true);
        }

        public ValidationResponse isValidCard(Tarjeta tarjeta)
        {
            var validCardNumber = isValidCardNumber(tarjeta.numeroTarjeta);
            var validCvv = isValidCvv(tarjeta.cvv);
            var validPin = isValidPin(tarjeta.pin);
            var validExpirationDate = isValidExpirationDate(tarjeta.fechaExpiracion);
            var validBalances = isValidBalances(tarjeta);
            var validStatus = isValidStatus(tarjeta.estado);
            var validExpDate = isValidExpDate(tarjeta.fechaExpiracion);

            if (!validBalances.success) return validBalances;
            if (!validCardNumber.success) return validCardNumber;
            if (!validCvv.success) return validCvv;
            if (!validPin.success) return validPin;
            if (!validExpirationDate.success) return validExpirationDate;
            if (!validStatus.success) return validStatus;
            if (!validExpDate.success) return validExpDate;

            return new ValidationResponse("La tarjeta es válida", true);
        }

        public ValidationResponse isValidBalances(Tarjeta tarjeta)
        {
            if (Enum.TryParse<TipoTarjeta>(tarjeta.tipo.ToString(), true, out var tipoTarjeta))
            {
                bool validBalances = tipoTarjeta == TipoTarjeta.Credito
                    ? tarjeta.saldoActual >= 0 && tarjeta.limiteCredito > 0 && tarjeta.saldoActual <= tarjeta.limiteCredito
                    : tipoTarjeta == TipoTarjeta.Debito ?  tarjeta.saldoActual >= 0 && tarjeta.limiteCredito == 0 : false;

                if (!validBalances)
                {
                    return new ValidationResponse($"Los saldos de la tarjeta {tarjeta.numeroTarjeta} no son válidos", false);
                }
            }
            else
            {
                return new ValidationResponse($"El tipo de tarjeta {tarjeta.tipo} no es válido", false);
            }

            return new ValidationResponse($"Los saldos son válidos", true);
        }


        public ValidationResponse isValidDpi(string dpi)
        {
            string newDpi = Regex.Replace(dpi, @"\D", "");

            if (!long.TryParse(newDpi, out _))
            {
                return new ValidationResponse($"El DPI {dpi} solo debe contener números", false);
            }

            if (newDpi.Length != 13)
            {
                return new ValidationResponse($"El DPI {dpi} debe tener 13 dígitos", false);
            }

            return new ValidationResponse($"El DPI {dpi} es válido", true);
        }

        public ValidationResponse isValidNit(string nit)
        {
            string newNit = Regex.Replace(nit, @"\D", "");

            if (!long.TryParse(newNit, out _))
            {
                return new ValidationResponse($"El NIT {nit} solo debe contener números", false);
            }

            if (newNit.Length != 9)
            {
                return new ValidationResponse($"El NIT {nit} debe tener 9 dígitos", false);
            }

            return new ValidationResponse($"El NIT {nit} es válido", true);
        }

        public ValidationResponse isValidEmail(string email)
        {
            string newEmail = Regex.Replace(email, @"\s+", "");
            if (string.IsNullOrEmpty(newEmail))
            {
                return new ValidationResponse($"El correo {email} no puede estar vacío", false);
            }

            if (!Regex.IsMatch(newEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return new ValidationResponse($"El correo {email} no es válido", false);
            }

            return new ValidationResponse($"El correo {email} es válido", true);
        }

        public ValidationResponse isValidCardNumber(string cardNumber)
        {
            string newCardNumber = Regex.Replace(cardNumber, @"\D", "");

            if (!long.TryParse(newCardNumber, out _))
            {
                return new ValidationResponse($"El número {cardNumber} solo debe contener números", false);
            }

            if (newCardNumber.Length != 16)
            {
                return new ValidationResponse($"El número de tarjeta {cardNumber} debe tener 16 dígitos", false);
            }

            return new ValidationResponse($"El número de tarjeta {cardNumber} es válido", true);
        }

        public ValidationResponse isValidCvv(string cvv)
        {
            string newCvv = Regex.Replace(cvv, @"\D", "");

            if (!int.TryParse(newCvv, out _))
            {
                return new ValidationResponse($"El CVV {cvv} solo debe contener números", false);
            }

            if (newCvv.Length != 3)
            {
                return new ValidationResponse($"El CVV {cvv} debe tener 3 dígitos", false);
            }

            return new ValidationResponse($"El CVV {cvv} es válido", true);
        }

        public ValidationResponse isValidPin(string pin)
        {
            string newPin = Regex.Replace(pin, @"\D", "");

            if (!int.TryParse(newPin, out _))
            {
                return new ValidationResponse($"El PIN {pin} solo debe contener números", false);
            }

            if (newPin.Length != 4)
            {
                return new ValidationResponse($"El PIN {pin} debe tener 4 dígitos", false);
            }

            return new ValidationResponse($"El PIN {pin} es válido", true);
        }

        public ValidationResponse isValidExpirationDate(string expirationDate)
        {
            //if (!DateTime.TryParse(expirationDate, out DateTime date))
            //{
            //    return new ValidationResponse($"La fecha de expiración {expirationDate} no es válida", false);
            //}

            //if (date < DateTime.Now)
            //{
            //    return new ValidationResponse($"La fecha de expiración {expirationDate} no puede ser menor a la fecha actual", false);
            //}


            bool isValid = Regex.IsMatch(expirationDate, @"^(0[1-9]|1[0-2])/([2-9][0-9])$");

            if (!isValid)
            {
                return new ValidationResponse($"La fecha de expiración {expirationDate} no es válida", false);
            }

            return new ValidationResponse($"La fecha de expiración {expirationDate} es válida", true);
        }

        public ValidationResponse isValidStatus(int status)
        {
            if (status != 0 && status != 1)
            {
                return new ValidationResponse($"El estado {status} no es válido: Status 0: Inactivo, Status 1: Activo", false);
            }
            return new ValidationResponse($"El estado {status} es válido", true);
        }


        public ValidationResponse isValidExpDate(string expDate)
        {
            if (DateTime.TryParseExact(
                    expDate,
                    "MM/yy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                int lastDay = DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month);
                DateTime finalDate = new DateTime(parsedDate.Year, parsedDate.Month, lastDay);

                //if (finalDate < DateTime.Today)
                //{
                //    return new ValidationResponse($"Tarjeta expirada: ${expDate}", false);
                //}

                return new ValidationResponse($"La fecha de expiración {expDate} es válida", true);


            }

            return new ValidationResponse($"La fecha de expiración {expDate} no es válida", false);
        }




        public ValidationResponse isValidCardDate(string expDate)
        {
            if (DateTime.TryParseExact(
                    expDate,
                    "MM/yy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                int lastDay = DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month);
                DateTime finalDate = new DateTime(parsedDate.Year, parsedDate.Month, lastDay);

                if (finalDate < DateTime.Today)
                {
                    return new ValidationResponse($"Tarjeta expirada: ${expDate}", false);
                }

                return new ValidationResponse($"La fecha de expiración {expDate} es válida", true);


            }

            return new ValidationResponse($"La fecha de expiración {expDate} no es válida", false);
        }

        public ValidationResponse validIncrementDate(string expDate)
        {
            if (DateTime.TryParseExact(
                    expDate,
                    "MM/yy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                int lastDay = DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month);
                DateTime expirationDate = new DateTime(parsedDate.Year, parsedDate.Month, lastDay);

                DateTime oneYearFromToday = DateTime.Today.AddYears(1);

                if (expirationDate <= oneYearFromToday)
                {
                    return new ValidationResponse(
                        $"La fecha de expiración {expDate} debe ser mayor a un año desde hoy",
                        false
                    );
                }

                return new ValidationResponse(
                    $"La fecha de expiración {expDate} es válida y está a más de un año",
                    true
                );
            }

            return new ValidationResponse(
                $"La fecha de expiración {expDate} no está en formato MM/yy",
                false
            );
        }

    }
}
