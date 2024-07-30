Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient

Public Interface IStlSearchFormItem

    'genitore
    Property SearchForm() As StlSearchForm

    'numero d'ordine
    Property OrderIndex() As Integer

    'nome campo (discriminante)
    Property NOMECAMP() As String

    'deve restituire una collezione di celle contenenti i controlli (labels, controlli, etc)
    'eventualmente già "riempiti" con i dati
    Function GetVisibleControlsCells(ByVal dataSqlConnection As SqlConnection) As List(Of HtmlTableCell)

    'deve restituire i controlli "hidden" della ricerca
    Function GetHiddenControls() As List(Of Control)

    'evidenzia (a modo suo...) il controllo per segnalare un errore
    Sub HiLite()

    'pulisce il controllo/i controlli visibili
    Sub ClearVisible()

    'copia i dati dal visibile all'hidden
    Sub CopyValuesToHiddenControls()

    'restituisce la condizione where, oppure stringa vuota
    Function GetWhereClause() As String

    'restituisce la lista dei valori per il nuovo record
    'si arrangia da solo a non fornire dati se i campi non "compongono" il nuovo (in tal caso
    'fornisce una lista vuota)
    Function GetNewItemSqlValues(ByVal dataSqlConnection As SqlConnection) As Dictionary(Of String, Object)

    'valida i dati del controllo per la ricerca
    'restituisce stringa vuota o la descrizione dell'errore; NON evidenzia il campo
    Function ValidateForSearch(ByVal dataSqlConnection As SqlConnection) As String

    'valida i dati del controllo per la creazione
    'restituisce stringa vuota o la descrizione dell'errore; NON evidenzia il campo
    Function ValidateForNewItem(ByVal dataSqlConnection As SqlConnection) As String

    'cancella eventuali valori non utilizzati nel Nuovo
    'ed aggiusta eventuali stringhe (rimozione %)
    Sub PrepareVisibleValueForNewItem()

    'restituisce il valore contenuto nel controllo visibile oppure Nothing
    'da chiamare SOLO dopo la validazione
    Function GetValidKeyValue() As KeyValuePair(Of String, Object)

End Interface
