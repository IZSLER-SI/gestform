Public Interface ICustomControl

    'deve essere implementata da tutti i controlli di tipo custom che vengono creati.

    Property FieldName() As String      'deve contenere il nome campo DB
    Sub SetEnabled()
    Sub SetDisabled()
    Function IsEnabled() As Boolean
    Sub SetHasError(errorType As StlFormView.ErrorTypes)
    Sub SetIsOK()
    Sub SetErrorText(errorType As StlFormView.ErrorTypes, ByVal errorText As String)
    Sub ClearErrorText()
    Property MaxLength() As Integer
    Property Value() As String          'il valore deve essere qui
    Sub SetFocus()
    Function IsValid() As Boolean

End Interface
