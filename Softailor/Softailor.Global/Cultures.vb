Public Class Cultures
    Public Shared Function CulturaItalian() As System.Globalization.CultureInfo
        Dim cHUM As New System.Globalization.CultureInfo("it-IT", False)
        cHUM.DateTimeFormat.DateSeparator = "/"
        cHUM.DateTimeFormat.TimeSeparator = "."
        cHUM.NumberFormat.CurrencyDecimalDigits = 2
        cHUM.NumberFormat.CurrencyDecimalSeparator = ","
        cHUM.NumberFormat.CurrencyGroupSeparator = "."
        cHUM.NumberFormat.CurrencySymbol = ""
        cHUM.NumberFormat.NumberDecimalSeparator = ","
        cHUM.NumberFormat.NumberGroupSeparator = ""
        Return cHUM
    End Function

    Public Shared Function CulturaItalianConSeparatoreMigliaia() As System.Globalization.CultureInfo
        Dim cHUM As New System.Globalization.CultureInfo("it-IT", False)
        cHUM.DateTimeFormat.DateSeparator = "/"
        cHUM.DateTimeFormat.TimeSeparator = "."
        cHUM.NumberFormat.CurrencyDecimalDigits = 2
        cHUM.NumberFormat.CurrencyDecimalSeparator = ","
        cHUM.NumberFormat.CurrencyGroupSeparator = "."
        cHUM.NumberFormat.CurrencySymbol = ""
        cHUM.NumberFormat.NumberDecimalSeparator = ","
        cHUM.NumberFormat.NumberGroupSeparator = "."
        Return cHUM
    End Function

    Public Shared Function CulturaEnglish() As System.Globalization.CultureInfo
        Dim cHUM As New System.Globalization.CultureInfo("en-US", False)
        cHUM.DateTimeFormat.DateSeparator = "/"
        cHUM.DateTimeFormat.TimeSeparator = ":"
        cHUM.NumberFormat.CurrencyDecimalDigits = 2
        cHUM.NumberFormat.CurrencyDecimalSeparator = "."
        cHUM.NumberFormat.CurrencyGroupSeparator = ","
        cHUM.NumberFormat.CurrencySymbol = ""
        cHUM.NumberFormat.NumberDecimalSeparator = "."
        cHUM.NumberFormat.NumberGroupSeparator = ""
        Return cHUM
    End Function

    Public Shared Function CulturaFrench() As System.Globalization.CultureInfo
        Dim cHUM As New System.Globalization.CultureInfo("fr-FR", False)
        cHUM.DateTimeFormat.DateSeparator = "/"
        cHUM.DateTimeFormat.TimeSeparator = ":"
        cHUM.NumberFormat.CurrencyDecimalDigits = 2
        cHUM.NumberFormat.CurrencyDecimalSeparator = "."
        cHUM.NumberFormat.CurrencyGroupSeparator = ","
        cHUM.NumberFormat.CurrencySymbol = ""
        cHUM.NumberFormat.NumberDecimalSeparator = "."
        cHUM.NumberFormat.NumberGroupSeparator = ""
        Return cHUM
    End Function

    Public Shared Function CulturaXml() As System.Globalization.CultureInfo
        Dim cHUM As New System.Globalization.CultureInfo("en-US", False)
        cHUM.DateTimeFormat.DateSeparator = "-"
        cHUM.DateTimeFormat.TimeSeparator = ":"
        cHUM.NumberFormat.CurrencyDecimalDigits = 2
        cHUM.NumberFormat.CurrencyDecimalSeparator = "."
        cHUM.NumberFormat.CurrencyGroupSeparator = ""
        cHUM.NumberFormat.CurrencySymbol = ""
        cHUM.NumberFormat.NumberDecimalSeparator = "."
        cHUM.NumberFormat.NumberGroupSeparator = ""
        Return cHUM
    End Function
End Class
