using DB.SqlConn;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace DataGenerator
{
    class Generator
    {
        OracleConnection connection = DBUtils.GetDBConnection();
        Random rnd = new Random();
        FileController fileController = new FileController();
        Form1 f = new Form1();

        public void Generate(string selectedState, int rowsToInsert)
        {
            connection.Open();
            switch (selectedState)
            {
                case "Dostawa":
                    GenerateDostawa(rowsToInsert);
                    break;
                case "Dostawcy":
                    GenerateDostawcy(rowsToInsert);
                    break;
                case "Kategorie":
                    GenerateKategorie(rowsToInsert);
                    break;
                case "Klienci":
                    GenerateKlienci(rowsToInsert);
                    break;
                case "Posada":
                    GeneratePosada(rowsToInsert);
                    break;
                case "Pracownicy":
                    GeneratePracownicy(rowsToInsert);
                    break;
                case "Producenty":
                    GenerateProducenty(rowsToInsert);
                    break;
                case "Towary":
                    GenerateTowary(rowsToInsert);
                    break;
                case "Zamowenia":
                    GenerateZamowenia(rowsToInsert);
                    break;          
            }
            connection.Close();
        }

        private void GenerateDostawa(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD-MM-YYYY'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO DOSTAWA (id_dostawy, data, dostawca) VALUES (:id, :data, :dostawca)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter data = new OracleParameter();
            data.OracleDbType = OracleDbType.Varchar2;
            OracleParameter dostawca = new OracleParameter();
            dostawca.OracleDbType = OracleDbType.Int32;

            if (CheckRowsCount("Dostawcy") < 3)
            {
                GenerateDostawcy(5);
            }

            DateTime startDate = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - startDate).Days;

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Dostawa") + 1;
                data.Value = startDate.AddDays(rnd.Next(range)).ToString("dd-MM-yyyy");
                dostawca.Value = rnd.Next(1, CheckRowsCount("Dostawcy") + 1);
                

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(data);
                cmd.Parameters.Add(dostawca);
                
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateDostawcy(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO DOSTAWCY (id_dostawcy, nazwa, telefon, adres) VALUES (:id, :nazwa, :telefon, :adres)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter nazwa = new OracleParameter();
            nazwa.OracleDbType = OracleDbType.Varchar2;
            OracleParameter telefon = new OracleParameter();
            telefon.OracleDbType = OracleDbType.Int32;
            OracleParameter adres = new OracleParameter();
            adres.OracleDbType = OracleDbType.Varchar2;

            string[] nazwaSet = fileController.ReadFile("Nazwy dostawców.txt");
            string[] adresSet = fileController.ReadFile("Adresy.txt");

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Dostawcy") + 1;
                nazwa.Value = nazwaSet[rnd.Next(0, nazwaSet.Length)];
                telefon.Value = rnd.Next(100000000, 999999999);
                adres.Value = adresSet[rnd.Next(0, adresSet.Length)];

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(nazwa);
                cmd.Parameters.Add(telefon);
                cmd.Parameters.Add(adres);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GeneratePosada(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO POSADA (id_posady, nazwa, wynagrodzenie) VALUES (:id, :posada, :wynagrodzenie)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter posada = new OracleParameter();
            posada.OracleDbType = OracleDbType.Varchar2;
            OracleParameter wynagrodzenie = new OracleParameter();
            wynagrodzenie.OracleDbType = OracleDbType.Int32;

            string[] posadaSet = fileController.ReadFile("Posady.txt");

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Posada") + 1;
                posada.Value = posadaSet[rnd.Next(0, posadaSet.Length)];
                wynagrodzenie.Value = rnd.Next(35, 70) * 100;

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(posada);
                cmd.Parameters.Add(wynagrodzenie);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateKategorie(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO KATEGORIE (id_kategorii, nazwa) VALUES (:id, :nazwa)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter nazwa = new OracleParameter();
            nazwa.OracleDbType = OracleDbType.Varchar2;

            string[] nazwaSet = fileController.ReadFile("Nazwy kategorje.txt");

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Kategorie") + 1;
                nazwa.Value = nazwaSet[rnd.Next(0, nazwaSet.Length)];

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(nazwa);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateKlienci(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO KLIENCI(id_klienta, imie, nazwisko, telefon) values(:id, :imie, :nazwisko, :telefon)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter imie = new OracleParameter();
            imie.OracleDbType = OracleDbType.Varchar2;
            OracleParameter nazwisko = new OracleParameter();
            nazwisko.OracleDbType = OracleDbType.Varchar2;
            OracleParameter telefon = new OracleParameter();
            telefon.OracleDbType = OracleDbType.Int32;

            string[] names = fileController.ReadFile("Imiona.txt");
            string[] surnames = fileController.ReadFile("Nazwiska.txt");

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Klienci") + 1;
                imie.Value = names[rnd.Next(0, names.Length)];
                nazwisko.Value = surnames[rnd.Next(0, surnames.Length)];
                telefon.Value = rnd.Next(100000000, 999999999);

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(imie);
                cmd.Parameters.Add(nazwisko);
                cmd.Parameters.Add(telefon);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GeneratePracownicy(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD-MM-YYYY'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO PRACOWNICY (id_pracownika, imie, nazwisko, data_urodzenia, telefon, posada) " +
                "VALUES (:id, :imie, :nazwisko, :data, :telefon, :idPosada)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter imie = new OracleParameter();
            imie.OracleDbType = OracleDbType.Varchar2;
            OracleParameter nazwisko = new OracleParameter();
            nazwisko.OracleDbType = OracleDbType.Varchar2;
            OracleParameter data = new OracleParameter();
            data.OracleDbType = OracleDbType.Varchar2;
            OracleParameter telefon = new OracleParameter();
            telefon.OracleDbType = OracleDbType.Int32;
            OracleParameter idPosada = new OracleParameter();
            idPosada.OracleDbType = OracleDbType.Int32;

            string[] names = fileController.ReadFile("Imiona.txt");
            string[] surnames = fileController.ReadFile("Nazwiska.txt");

            DateTime startDate = new DateTime(1980, 1, 1);
            DateTime endDate = new DateTime(2001, 1, 1);
            int range = (endDate - startDate).Days;

            if (CheckRowsCount("Posada") < 3)
            {
                GeneratePosada(5);
            }

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Pracownicy") + 1;
                imie.Value = names[rnd.Next(0, names.Length)];
                nazwisko.Value = surnames[rnd.Next(0, surnames.Length)];
                data.Value = startDate.AddDays(rnd.Next(range)).ToString("dd-MM-yyyy");
                telefon.Value = rnd.Next(100000000, 999999999);
                idPosada.Value = rnd.Next(1, CheckRowsCount("Posada") + 1);

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(imie);
                cmd.Parameters.Add(nazwisko);
                cmd.Parameters.Add(data);
                cmd.Parameters.Add(telefon);
                cmd.Parameters.Add(idPosada);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateProducenty(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO PRODUCENTY (id_producenta, nazwa) VALUES (:id, :nazwa)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter nazwa = new OracleParameter();
            nazwa.OracleDbType = OracleDbType.Varchar2;

            string[] nazwaSet = fileController.ReadFile("Nazwy producentów.txt");

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Producenty") + 1;
                nazwa.Value = nazwaSet[rnd.Next(0, nazwaSet.Length)];

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(nazwa);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateTowary(int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO TOWARY (id_towaru, dostawa, kategoria, producent, nazwa, koszt_zakupu, koszt_sprzedazu, ilosc) " +
                "VALUES (:id, :idDostawa, :idKategoria, :idProducent, :nazwa, :zakup, :sprzedaz, :ilosc)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter idDostawa = new OracleParameter();
            idDostawa.OracleDbType = OracleDbType.Int32;
            OracleParameter idKategoria = new OracleParameter();
            idKategoria.OracleDbType = OracleDbType.Int32;
            OracleParameter idProducent = new OracleParameter();
            idProducent.OracleDbType = OracleDbType.Int32;
            OracleParameter nazwa = new OracleParameter();
            nazwa.OracleDbType = OracleDbType.Varchar2;
            OracleParameter zakup = new OracleParameter();
            zakup.OracleDbType = OracleDbType.Int32;
            OracleParameter sprzedaz = new OracleParameter();
            sprzedaz.OracleDbType = OracleDbType.Int32;
            OracleParameter ilosc = new OracleParameter();
            ilosc.OracleDbType = OracleDbType.Int32;

            if (CheckRowsCount("Dostawa") < 3)
            {
                GenerateDostawa(5);
            }
            if (CheckRowsCount("Kategorie") < 3)
            {
                GenerateKategorie(5);
            }
            if (CheckRowsCount("Producenty") < 3)
            {
                GenerateProducenty(5);
            }

            string[] nazwaSet = fileController.ReadFile("Nazwy towarów.txt");
            int zakupTmp;

            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Towary") + 1;
                idDostawa.Value = rnd.Next(1, CheckRowsCount("Dostawa") + 1);
                idKategoria.Value = rnd.Next(1, CheckRowsCount("Kategorie") + 1);
                idProducent.Value = rnd.Next(1, CheckRowsCount("Producenty") + 1);
                nazwa.Value = nazwaSet[rnd.Next(0, nazwaSet.Length)];
                zakupTmp = rnd.Next(50, 5000);
                zakup.Value = zakupTmp;
                sprzedaz.Value = zakupTmp + (zakupTmp / 100) * 20;
                ilosc.Value = rnd.Next(1, 20);

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(idDostawa);
                cmd.Parameters.Add(idKategoria);
                cmd.Parameters.Add(idProducent);
                cmd.Parameters.Add(nazwa);
                cmd.Parameters.Add(zakup);
                cmd.Parameters.Add(sprzedaz);
                cmd.Parameters.Add(ilosc);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }

        private void GenerateZamowenia (int rowsToInsert)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD-MM-YYYY'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO ZAMOWENIA (id_zamowenia, towar, pracownik, data_stworzenia, klient) " +
                "VALUES (:id, :idTowar, :idPracownik, :data, :idKlient)";

            OracleParameter id = new OracleParameter();
            id.OracleDbType = OracleDbType.Int32;
            OracleParameter idTowar = new OracleParameter();
            idTowar.OracleDbType = OracleDbType.Int32;
            OracleParameter idPracownik = new OracleParameter();
            idPracownik.OracleDbType = OracleDbType.Int32;
            OracleParameter data = new OracleParameter();
            data.OracleDbType = OracleDbType.Varchar2;
            OracleParameter idKlient = new OracleParameter();
            idKlient.OracleDbType = OracleDbType.Int32;

            if (CheckRowsCount("Towary") < 3)
            {
                GenerateTowary(5);
            }
            if (CheckRowsCount("Pracownicy") < 3)
            {
                GeneratePracownicy(5);
            }
            if (CheckRowsCount("Klienci") < 3)
            {
                GenerateKlienci(5);
            }

            DateTime startDate = DateTime.Today;
 
            for (int i = 0; i < rowsToInsert; i++)
            {
                id.Value = CheckRowsCount("Zamowenia") + 1;
                idTowar.Value = rnd.Next(1, CheckRowsCount("Towary") + 1);
                idPracownik.Value = rnd.Next(1, CheckRowsCount("Pracownicy") + 1);
                data.Value = startDate.AddDays(-i).ToString("dd-MM-yyyy");
                idKlient.Value = rnd.Next(1, CheckRowsCount("Klienci") + 1);

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(idTowar);
                cmd.Parameters.Add(idPracownik);
                cmd.Parameters.Add(data);
                cmd.Parameters.Add(idKlient);;

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            cmd.Dispose();
        }


        private int CheckRowsCount(string tableName)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT count(*) FROM " + tableName;
            OracleDataReader reader = cmd.ExecuteReader();
            int existingRows = 0;
            while (reader.Read())
            {
                object exitingRowsObject = reader["COUNT(*)"];
                existingRows = Convert.ToInt32(exitingRowsObject);
            }
            return existingRows;
        }

        public string Delete(string table)
        {
            connection.Open();
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM " + table;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            return "OK";
        }
    }
}