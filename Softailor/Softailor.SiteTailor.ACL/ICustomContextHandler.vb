Imports System.Xml

Public Interface ICustomContextHandler
    'restituisce una TD da includere nell'elenco
    Function GetPanelTd(ByVal page As System.Web.UI.Page, ByVal functionAuthorization As Softailor.SiteTailor.ACL.SiteTailorFunctionAuthorization) As String
    'pulizia dei dati immagazzinati nella session
    Sub ClearSession()
    'restituisce un eventuale URL per la redirezione quando si sta in Default.aspx
    Function DefaultPageRedirectUrl() As String
    'restituisce un eventuale URL per la redirezione quando si sta nella master page (ad es evento non selezionato)
    Function MasterPageRedirectUrl(ByVal page As System.Web.UI.Page, ByVal functionAuthorization As Softailor.SiteTailor.ACL.SiteTailorFunctionAuthorization) As String
    'processa la lista delle funzioni e dei menu
    Sub ProcessFunctionsAndMenus(dataDbConn As SqlConnection, ByRef funzColl As Dictionary(Of String, SiteTailorFunctionAuthorization), ByVal userMenuXDocOriginal As XmlDocument, ByRef userMenuXDocProcessed As XmlDocument)

    'restituzione di id_CLIENTE e id_EDIZIONE per la gestione dei custom file (dyn data)
    Function Get_dyn_id_CLIENTE() As Integer
    Function Get_dyn_id_EDIZIONE() As Integer
    Function Get_dyn_MaxFileSizeKb() As Integer
    Function Get_dyn_ExtensionList() As String

End Interface
