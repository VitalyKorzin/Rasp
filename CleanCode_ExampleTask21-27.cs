using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;

public class Form
{
    private readonly string _dataBaseFileName = "db.sqlite";
    private readonly int _passportIDLength = 10;
    private readonly TextBox _passportIDTextBox;
    private readonly TextBox _resultTextBox;

    private void OnButtonClick(object sender, EventArgs e)
    {
        string passportID = GetPassportID();

        if (string.IsNullOrEmpty(passportID))
        {
            MessageBox.Show("Введите серию и номер паспорта");
        }
        else if (passportID.Length != _passportIDLength)
        {
            _resultTextBox.Text = "Неверный формат серии или номера паспорта";
        }
        else
        {
            try
            {
                ShowPassportAvailability(GetPassportsTable(passportID));
            }
            catch (SQLiteException exception)
            {
                if (exception.ErrorCode == 1)
                    MessageBox.Show($"Файл {_dataBaseFileName} не найден. Положите файл в папку вместе с exe.");
            }
        }
    }

    private string GetPassportID()
        => _passportIDTextBox.Text.Trim().Replace(" ", string.Empty);

    private void ShowPassportAvailability(DataTable passportsTable)
    {
        if (passportsTable.Rows.Count > 0)
        {
            _resultTextBox.Text = $"По паспорту «{_passportIDTextBox.Text}» доступ к бюллетеню на дистанционном электронном голосовании ";

            if (Convert.ToBoolean(passportsTable.Rows[0].ItemArray[1]))
                _resultTextBox.Text += "ПРЕДОСТАВЛЕН";
            else
                _resultTextBox.Text += "НЕ ПРЕДОСТАВЛЯЛСЯ";
        }
        else
        {
            _resultTextBox.Text = $"Паспорт «{_passportIDTextBox.Text}» в списке участников дистанционного голосования НЕ НАЙДЕН";
        }
    }

    private DataTable GetPassportsTable(string passportID)
    {
        string commandText = string.Format($"select * from passports where num='{ComputeSha256Hash(passportID)}' limit 1;");
        string connectionString = string.Format($"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{_dataBaseFileName}");
        var connection = new SQLiteConnection(connectionString);
        connection.Open();
        var adapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
        var passportsTable = new DataTable();
        adapter.Fill(passportsTable);
        connection.Close();
        return passportsTable;
    }

    private string ComputeSha256Hash(string message)
    {
        SHA256 hashAlgorithm = new SHA256CryptoServiceProvider();
        hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hashAlgorithm.Hash);
    }
}